using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Gedcom.Entities;
using Gedcom.Entities.Gedcom;
using Gedcom.Extenders;

namespace Gedcom.Services
{
    public class GedcomParser
    {
        private GedcomChunkLevels _gedcomChunkLevels;
        private List<GedcomChunk> _idChunks;

        public List<Person> Persons { get; private set; }
        public List<Relation> Relations { get; private set; }


        public void Parse(string filePath)
        {
            var topChunks = GenerateChunks(filePath);
            ParseTopChunks(topChunks);
        }

        private List<GedcomChunk> GenerateChunks(string filePath)
        {
            _gedcomChunkLevels = new GedcomChunkLevels();
            Persons = new List<Person>();
            Relations = new List<Relation>();
            _idChunks = new List<GedcomChunk>();

            var gedcomLines = File.ReadAllLines(filePath).Select(GedcomLine.Parse);
            var topChunks = new List<GedcomChunk>();

            foreach (var gedcomLine in gedcomLines)
            {
                var chunk = new GedcomChunk(gedcomLine);
                if (gedcomLine.Level == 0)
                {
                    topChunks.Add(chunk);
                }
                else
                {
                    var parent = _gedcomChunkLevels.GetParentChunk(chunk);
                    parent.SubChunks.Add(chunk);
                }
                _gedcomChunkLevels.Set(chunk);
            }

            GedcomPrinter.PrintChunks(topChunks);
            GedcomPrinter.PrintTypes(topChunks);

            return topChunks;
        }


        private void ParseTopChunks(ICollection<GedcomChunk> chunks)
        {
            foreach (var chunk in chunks)
            {
                switch (chunk.Type)
                {
                    case "INDI":
                        ParseIndividual(chunk);
                        break;
                    case "FAM":
                        ParseFamily(chunk);
                        break;

                    // Deliberately skip as irrelevant for our usecase
                    case "HEAD":
                    case "GEDC":
                    case "SUBN":
                    case "SUBM":
                    case "SOUR":
                    case "REPO":
                    case "CSTA":
                    case "TRLR":
                    case "NOTE":
                    case "OBJE":
                        break;

                    default:
                        throw new NotImplementedException($"ParseTopChunks: Type='{chunk.Type}' is not handled");
                }

                // Save for lookup
                if (chunk.Reference.IsSpecified())
                {
                    _idChunks.Add(chunk);
                }
            }
        }

        private void ParseIndividual(GedcomChunk indiChunk)
        {
            var person = new Person { Id = indiChunk.Id };

            foreach (var chunk in indiChunk.SubChunks)
            {
                switch (chunk.Type)
                {
                    case "NAME":
                        var nameSections = chunk.Data.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (nameSections.Length > 0)
                        {
                            person.FirstName = nameSections[0];
                        }
                        if (nameSections.Length > 1)
                        {
                            person.LastName = nameSections[1];
                        }
                        break;

                    case "SEX":
                        person.Gender = chunk.Data;
                        break;

                    case "BIRT":
                        person.Birth = ParseDatePlace(chunk);
                        break;

                    case "DEAT":
                        person.Death = ParseDatePlace(chunk);
                        break;

                    case "BURI":
                        person.Buried = ParseDatePlace(chunk);
                        break;

                    case "CHAN":
                        person.Changed = ParseDateTime(chunk);
                        break;

                    case "CHR":
                        person.Baptized = ParseDatePlace(chunk);
                        break;

                    case "EDUC":
                        person.Education = chunk.Data;
                        break;

                    case "RELI":
                        person.Religion = chunk.Data;
                        break;

                    case "NOTE":
                        person.Note = ParseNote(chunk);
                        break;

                    case "OCCU":
                        person.Occupation = chunk.Data;
                        break;

                    case "HEAL":
                        person.Health = chunk.Data;
                        break;

                    case "TITL":
                        person.Title = chunk.Data;
                        break;


                    // Deliberately skip as irrelevant for our usecase
                    case "FAMS":
                    case "FAMC":
                    case "OBJE":
                    case "HIST":
                        break;

                    default:
                        throw new NotImplementedException($"ParseIndividual: Type='{chunk.Type}' is not handled");
                }
            }

            Persons.Add(person);
        }

        private void ParseFamily(GedcomChunk famChunk)
        {
            DatePlace marriage = null;
            var parents = new List<Person>();
            var children = new List<Person>();
            DatePlace divorce = null;

            foreach (var chunk in famChunk.SubChunks)
            {
                switch (chunk.Type)
                {
                    case "HUSB":
                        var husband = Persons.SingleOrDefault(p => p.Id == chunk.Reference);
                        if (husband != null)
                        {
                            parents.Add(husband);
                        }
                        break;

                    case "WIFE":
                        var wife = Persons.SingleOrDefault(p => p.Id == chunk.Reference);
                        if (wife != null)
                        {
                            parents.Add(wife);
                        }
                        break;

                    case "CHIL":
                        var child = Persons.SingleOrDefault(p => p.Id == chunk.Reference);
                        if (child != null)
                        {
                            children.Add(child);
                        }
                        break;

                    case "MARR":
                        marriage = ParseDatePlace(chunk);
                        break;

                    case "DIV":
                        divorce = ParseDatePlace(chunk);
                        break;

                    // Deliberately skip as irrelevant for our usecase


                    default:
                        throw new NotImplementedException($"ParseFamily: Type='{chunk.Type}' is not handled");
                }
            }

            // Spouses
            if (parents.Count == 2)
            {
                Relations.Add(new SpouseRelation { FamilyId = famChunk.Id, From = parents[0], To = parents[1], Marriage = marriage, Divorce = divorce });
            }

            // Parents / Children
            foreach (var parent in parents)
            {
                foreach (var child in children)
                {
                    var childRelation = new ChildRelation { FamilyId = famChunk.Id, From = child, To = parent };
                    AddStatus(childRelation);
                    Relations.Add(childRelation);
                }
            }

            // Siblings
            foreach (var child1 in children)
            {
                foreach (var child2 in children.Where(c => c.Id != child1.Id))
                {
                    Relations.Add(new SiblingRelation { FamilyId = famChunk.Id, From = child1, To = child2 });
                }
            }
        }

        private static DatePlace ParseDatePlace(GedcomChunk chunk)
        {
            return new DatePlace
            {
                Date = chunk.SubChunks.SingleOrDefault(c => c.Type == "DATE")?.Data,
                Place = chunk.SubChunks.SingleOrDefault(c => c.Type == "PLAC")?.Data
            };
        }

        private static string ParseDateTime(GedcomChunk chunk)
        {
            return (chunk.SubChunks.SingleOrDefault(c => c.Type == "DATE")?.Data + " " + chunk.SubChunks.SingleOrDefault(c => c.Type == "TIME")?.Data).Trim();
        }

        private string ParseNote(GedcomChunk incomingChunk)
        {
            var noteChunk = incomingChunk;
            if (incomingChunk.Reference.IsSpecified())
            {
                noteChunk = _idChunks.SingleOrDefault(c => c.Id == noteChunk.Reference);
                if (noteChunk == null)
                {
                    throw new Exception($"Unable to find Note with Id='{incomingChunk.Reference}'");
                }
            }
            var sb = new StringBuilder();
            foreach (var chunk in noteChunk.SubChunks)
            {
                switch (chunk.Type)
                {
                    case "CONC":
                        sb.Append(" " + chunk.Data);
                        break;
                    case "CONT":
                        sb.AppendLine(chunk.Data);
                        break;
                    default:
                        throw new NotImplementedException($"ParseNote: Type='{noteChunk.Type}' is not handled");
                }
            }
            return sb.ToString();
        }

        private void AddStatus(ChildRelation childRelation)
        {
            var childChunk = _idChunks.SingleOrDefault(c => c.Id == childRelation.From.Id);
            if (childChunk != null)
            {
                foreach (var chunk1 in childChunk.SubChunks)
                {
                    switch (chunk1.Type)
                    {
                        case "FAMC":
                            foreach (var chunk2 in chunk1.SubChunks)
                            {
                                switch (chunk2.Type)
                                {
                                    case "PEDI":
                                        childRelation.Pedigree = chunk2.Data;
                                        break;
                                    case "STAT":
                                        childRelation.Validity = chunk2.Data;
                                        break;
                                    case "ADOP":
                                        var adoptionInfo = new List<string>();
                                        foreach (var chunk3 in chunk1.SubChunks)
                                        {
                                            switch (chunk3.Type)
                                            {
                                                case "DATE":
                                                    adoptionInfo.Add(ParseDateTime(chunk3));
                                                    break;
                                                case "STAT":
                                                case "NOTE":
                                                    adoptionInfo.Add(chunk3.Data);
                                                    break;
                                            }
                                        }
                                        childRelation.Adoption = string.Join(", ", adoptionInfo);
                                        break;
                                }
                            }
                            break;
                    }
                }
            }
        }
    }
}
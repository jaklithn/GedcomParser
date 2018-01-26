using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GedcomParser.Entities;
using GedcomParser.Entities.Internal;
using GedcomParser.Extenders;


namespace GedcomParser.Services
{
    public class FileParser
    {
        private GedcomChunkLevels _gedcomChunkLevels;
        private List<GedcomChunk> _idChunks;

        public PersonContainer PersonContainer { get; set; }


        public void Parse(string filePath)
        {
            var topChunks = GenerateChunks(filePath);
            ParseTopChunks(topChunks);
        }

        internal List<GedcomChunk> GenerateChunks(string filePath)
        {
            _gedcomChunkLevels = new GedcomChunkLevels();
            PersonContainer = new PersonContainer();
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

            return topChunks;
        }


        private void ParseTopChunks(ICollection<GedcomChunk> chunks)
        {
            foreach (var chunk in chunks)
            {
                switch (chunk.Type)
                {
                    case "FAM":
                        ParseFamily(chunk);
                        break;

                    case "INDI":
                        ParseIndividual(chunk);
                        break;

                    // Deliberately skipped as irrelevant for our usecase
                    case "_GRP":
                    case "_PLC":
                    case "CSTA":
                    case "GEDC":
                    case "HEAD":
                    case "NOTE":
                    case "OBJE":
                    case "REPO":
                    case "SUBM":
                    case "SUBN":
                    case "SOUR":
                    case "TRLR":
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
                    case "_UID":
                        person.Uid = chunk.Data;
                        break;

                    case "ADOP":
                        person.Adoption = ParseAdoption(chunk);
                        break;

                    case "BAPM":
                        person.Baptized = ParseDatePlace(chunk);
                        break;

                    case "BIRT":
                        person.Birth = ParseDatePlace(chunk);
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

                    case "DEAT":
                        person.Death = ParseDatePlace(chunk);
                        break;

                    case "EDUC":
                        person.Education = chunk.Data;
                        break;

                    case "EMIG":
                        person.Emigrated = ParseDatePlace(chunk);
                        break;

                    case "FACT":
                        person.Note = ParseNote(person.Note, chunk);
                        break;

                    case "GRAD":
                        person.Graduation = ParseDatePlace(chunk);
                        break;

                    case "HEAL":
                        person.Health = chunk.Data;
                        break;

                    case "IDNO":
                        person.IdNumber = chunk.Data;
                        break;

                    case "IMMI":
                        person.Immigrated = ParseDatePlace(chunk);
                        break;

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

                    case "NATU":
                        person.BecomingCitizen = ParseDatePlace(chunk);
                        break;

                    case "NOTE":
                        person.Note = ParseNote(person.Note, chunk);
                        break;

                    case "OCCU":
                        person.Occupation = chunk.Data;
                        break;

                    case "RESI":
                        person.Residence = ParseDatePlace(chunk);
                        break;

                    case "RELI":
                        person.Religion = chunk.Data;
                        break;

                    case "SEX":
                        person.Gender = chunk.Data;
                        break;

                    case "TITL":
                        person.Title = chunk.Data;
                        break;


                    // Deliberately skipped as irrelevant for our usecase
                    case "_GRP":
                    case "CONF":
                    case "DSCR":
                    case "EVEN":
                    case "FAMS":
                    case "FAMC":
                    case "HIST":
                    case "NCHI":
                    case "NMR":
                    case "OBJE":
                    case "PAGE":
                    case "RIN":
                    case "SOUR":
                        break;

                    default:
                        throw new NotImplementedException($"ParseIndividual: Type='{chunk.Type}' is not handled");
                }
            }

            PersonContainer.Persons.Add(person);
        }

        private void ParseFamily(GedcomChunk famChunk)
        {
            DatePlace marriage = null;
            string relation = null;
            string note = null;
            DatePlace divorce = null;
            var parents = new List<Person>();
            var children = new List<Person>();

            foreach (var chunk in famChunk.SubChunks)
            {
                switch (chunk.Type)
                {
                    case "CHIL":
                        var child = PersonContainer.Persons.SingleOrDefault(p => p.Id == chunk.Reference);
                        if (child != null)
                        {
                            children.Add(child);
                        }
                        break;

                    case "DIV":
                        divorce = ParseDatePlace(chunk);
                        break;

                    case "HUSB":
                        var husband = PersonContainer.Persons.SingleOrDefault(p => p.Id == chunk.Reference);
                        if (husband != null)
                        {
                            parents.Add(husband);
                        }
                        break;

                    case "_REL":
                        relation = chunk.Data;
                        break;

                    case "MARR":
                        marriage = ParseDatePlace(chunk);
                        break;

                    case "NOTE":
                        note = ParseNote(note, chunk);
                        break;

                    case "WIFE":
                        var wife = PersonContainer.Persons.SingleOrDefault(p => p.Id == chunk.Reference);
                        if (wife != null)
                        {
                            parents.Add(wife);
                        }
                        break;

                    // Deliberately skipped as irrelevant for our usecase
                    case "CHAN":
                    case "DSCR":
                    case "EVEN":
                    case "FAMS":
                    case "FAMC":
                    case "HIST":
                    case "MARS":
                    case "NCHI":
                    case "NMR":
                    case "OBJE":
                    case "PAGE":
                    case "SOUR":

                        break;

                    default:
                        throw new NotImplementedException($"ParseFamily: Type='{chunk.Type}' is not handled");
                }
            }

            // Spouses
            if (parents.Count == 2)
            {
                PersonContainer.SpouseRelations.Add(new SpouseRelation {
                    FamilyId = famChunk.Id,
                    From = parents[0],
                    To = parents[1],
                    Marriage = marriage,
                    Divorce = divorce,
                    Relation = relation,
                    Note = note
                });
            }

            // Parents / Children
            foreach (var parent in parents)
            {
                foreach (var child in children)
                {
                    var childRelation = new ChildRelation { FamilyId = famChunk.Id, From = child, To = parent };
                    AddStatus(childRelation);
                    PersonContainer.ChildRelations.Add(childRelation);
                }
            }

            // Siblings
            foreach (var child1 in children)
            {
                foreach (var child2 in children.Where(c => c.Id != child1.Id))
                {
                    PersonContainer.SiblingRelations.Add(new SiblingRelation { FamilyId = famChunk.Id, From = child1, To = child2 });
                }
            }
        }

        private DatePlace ParseDatePlace(GedcomChunk chunk)
        {
            var datePlace = new DatePlace
            {
                Date = chunk.SubChunks.SingleOrDefault(c => c.Type == "DATE")?.Data,
                Place = chunk.SubChunks.SingleOrDefault(c => c.Type == "PLAC")?.Data
            };
            var map = chunk.SubChunks.SingleOrDefault(c => c.Type == "MAP");
            if (map != null)
            {
                datePlace.Latitude = map.SubChunks.SingleOrDefault(c => c.Type == "LATI")?.Data;
                datePlace.Longitude = map.SubChunks.SingleOrDefault(c => c.Type == "LONG")?.Data;
            }
            var note = chunk.SubChunks.SingleOrDefault(c => c.Type == "NOTE");
            if (note != null)
            {
                datePlace.Note = ParseNote(datePlace.Note, note);
            }
            return datePlace;
        }

        private static string ParseDateTime(GedcomChunk chunk)
        {
            return (chunk.SubChunks.SingleOrDefault(c => c.Type == "DATE")?.Data + " " + chunk.SubChunks.SingleOrDefault(c => c.Type == "TIME")?.Data).Trim();
        }

        private Address ParseAddress(GedcomChunk addressChunk)
        {
            // Top level node can also contain a full address or first part of it ...
            var address = new Address { Street = addressChunk.Data };

            foreach (var chunk in addressChunk.SubChunks)
            {
                switch (chunk.Type)
                {
                    case "CONT":
                    case "ADR1":
                    case "ADR2":
                    case "ADR3":
                        address.Street += Environment.NewLine + chunk.Data;
                        break;

                    case "CITY":
                        address.City += chunk.Data;
                        break;

                    case "STAE":
                        address.State += chunk.Data;
                        break;

                    case "POST":
                        address.ZipCode += chunk.Data;
                        break;

                    case "CTRY":
                        address.Country += chunk.Data;
                        break;

                    case "PHON":
                        address.Phone.Add(chunk.Data);
                        break;

                    case "EMAIL":
                        address.Email.Add(chunk.Data);
                        break;

                    case "FAX":
                        address.Fax.Add(chunk.Data);
                        break;

                    case "WWW":
                        address.Web.Add(chunk.Data);
                        break;

                    // Deliberately skipped as irrelevant for our usecase


                    default:
                        throw new NotImplementedException($"ParseAddress: Type='{chunk.Type}' is not handled");
                }
            }

            return address;
        }

        private Adoption ParseAdoption(GedcomChunk adoptionChunk)
        {
            var adoption = new Adoption { DatePlace = ParseDatePlace(adoptionChunk) };

            foreach (var chunk in adoptionChunk.SubChunks)
            {
                switch (chunk.Type)
                {
                    case "NOTE":
                        adoption.Note = ParseNote(adoption.Note, chunk);
                        break;

                    case "TYPE":
                        adoption.Type = chunk.Data;
                        break;

                    // Deliberately skipped as irrelevant for our usecase
                    case "DATE":
                    case "FAMC":
                    case "PLAC":
                    case "SOUR":
                        break;

                    default:
                        throw new NotImplementedException($"ParseAdoption: Type='{chunk.Type}' is not handled");
                }
            }

            return adoption;
        }

        private string ParseNote(string previousNote, GedcomChunk incomingChunk)
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
                if (UnwantedBlob(chunk))
                {
                    sb.AppendLine("(Skipped blob content)");
                    break;
                }
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

            return previousNote.IsSpecified() ? previousNote + Environment.NewLine + sb : sb.ToString();
        }

        private static bool UnwantedBlob(GedcomChunk chunk)
        {
            // TODO: We should make this check more intelligent :) 
            return chunk.Data?.Contains("<span") ?? false;
        }

        /// <summary>
        /// Lookup possible information on child legal status.
        /// It is stored in separate chunks outside the Individual and Family chunks.
        /// </summary>
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
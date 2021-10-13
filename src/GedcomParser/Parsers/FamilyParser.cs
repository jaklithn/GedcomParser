﻿using System;
using System.Collections.Generic;
using System.Linq;
using GedcomParser.Entities;
using GedcomParser.Entities.Internal;


namespace GedcomParser.Parsers
{
    public static class FamilyParser
    {
        internal static void ParseFamily(this ResultContainer resultContainer, GedcomChunk famChunk)
        {
            DatePlace engagement            = null;
            DatePlace marriage              = null;
            DatePlace marriageContract      = null;
            DatePlace marriageSettlement    = null;
            DatePlace marriageBann          = null;
            DatePlace marriageLicense       = null;
            string relation                 = null;
            string note                     = null;
            DatePlace divorceFiled          = null;
            DatePlace divorce               = null;
            DatePlace annulment             = null;
            var parents                     = new List<Person>();
            var children                    = new List<Person>();

            foreach (var chunk in famChunk.SubChunks)
            {
                switch (chunk.Type)
                {
                    case "CHIL":
                        var child = resultContainer.Persons.SingleOrDefault(p => p.Id == chunk.Reference);
                        if (child != null)
                        {
                            children.Add(child);
                        }

                        break;

                    case "DIV":
                        divorce = resultContainer.ParseDatePlace(chunk);
                        break;

                    case "DIVF":
                        divorceFiled = resultContainer.ParseDatePlace(chunk);
                        break;

                    case "ANUL":
                        annulment = resultContainer.ParseDatePlace(chunk);
                        break;

                    case "HUSB":
                        var husband = resultContainer.Persons.SingleOrDefault(p => p.Id == chunk.Reference);
                        if (husband != null)
                        {
                            parents.Add(husband);
                        }

                        break;

                    case "_REL":
                        relation = chunk.Data;
                        break;

                    case "MARR":
                        marriage = resultContainer.ParseDatePlace(chunk);
                        break;

                    case "MARC":
                        marriageContract = resultContainer.ParseDatePlace(chunk);
                        break;

                    case "MARB":
                        marriageBann = resultContainer.ParseDatePlace(chunk);
                        break;

                    case "MARL":
                        marriageLicense = resultContainer.ParseDatePlace(chunk);
                        break;

                    case "MARS":
                        marriageSettlement = resultContainer.ParseDatePlace(chunk);
                        break;

                    case "ENGA":
                        engagement = resultContainer.ParseDatePlace(chunk);
                        break;

                    case "NOTE":
                        note = resultContainer.ParseNote(note, chunk);
                        break;

                    case "WIFE":
                        var wife = resultContainer.Persons.SingleOrDefault(p => p.Id == chunk.Reference);
                        if (wife != null)
                        {
                            parents.Add(wife);
                        }

                        break;

                    // Deliberately skipped for now
                    case "CHAN":
                    case "DSCR":
                    case "EVEN":
                    case "FAMS":
                    case "FAMC":
                    case "HIST":
                    case "NCHI":
                    case "NMR":
                    case "OBJE":
                    case "PAGE":
                    case "SOUR":
                        resultContainer.Warnings.Add($"Skipped Family Type='{chunk.Type}'");
                        break;

                    default:
                        resultContainer.Errors.Add($"Failed to handle Family Type='{chunk.Type}'");
                        break;
                }
            }

            // Spouses
            if (parents.Count == 2)
            {
                resultContainer.SpouseRelations.Add(new SpouseRelation
                {
                    FamilyId            = famChunk.Id,
                    From                = parents[0],
                    To                  = parents[1],
                    Engagement          = engagement,
                    Marriage            = marriage,
                    MarriageContract    = marriageContract,
                    MarriageSettlement  = marriageSettlement,
                    MarriageBann        = marriageBann,
                    MarriageLicense     = marriageLicense,
                    DivorceFiled        = divorceFiled,
                    Divorce             = divorce,
                    Annulment           = annulment,
                    Relation            = relation,
                    Note                = note
                });
            }

            // Parents / Children
            foreach (var parent in parents)
            {
                foreach (var child in children)
                {
                    var childRelation = new ChildRelation { FamilyId = famChunk.Id, From = child, To = parent };
                    AddStatus(resultContainer, childRelation);
                    resultContainer.ChildRelations.Add(childRelation);
                }
            }

            // Siblings
            foreach (var child1 in children)
            {
                foreach (var child2 in children.Where(c => c.Id != child1.Id))
                {
                    resultContainer.SiblingRelations.Add(new SiblingRelation { FamilyId = famChunk.Id, From = child1, To = child2 });
                }
            }
        }

        /// <summary>
        /// Lookup possible information on child legal status.
        /// It is stored in separate chunks outside the Individual and Family chunks.
        /// </summary>
        private static void AddStatus(this ResultContainer resultContainer, ChildRelation childRelation)
        {
            var childChunk = resultContainer.GetIdChunk(childRelation.From.Id);
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
                                                    adoptionInfo.Add(chunk3.ParseDateTime());
                                                    break;
                                                case "STAT":
                                                case "NOTE":
                                                    adoptionInfo.Add(chunk3.Data);
                                                    break;
                                            }
                                        }

                                        childRelation.Adoption = string.Join(", ", adoptionInfo);
                                        break;

                                    default:
                                        resultContainer.Errors.Add($"Failed to handle Status Type='{chunk2.Type}'");
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
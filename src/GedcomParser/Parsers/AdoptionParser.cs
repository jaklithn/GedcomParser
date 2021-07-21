using System;
using GedcomParser.Entities;
using GedcomParser.Entities.Internal;


namespace GedcomParser.Parsers
{
    public static class AdoptionParser
    {
        internal static Adoption ParseAdoption(this ResultContainer resultContainer, GedcomChunk adoptionChunk)
        {
            var adoption = new Adoption {DatePlace = resultContainer.ParseDatePlace(adoptionChunk)};

            foreach (var chunk in adoptionChunk.SubChunks)
            {
                switch (chunk.Type)
                {
                    case "NOTE":
                        adoption.Note = resultContainer.ParseNote(adoption.Note, chunk);
                        break;

                    case "TYPE":
                        adoption.Type = chunk.Data;
                        break;

                    // Deliberately skipped for now
                    case "DATE":
                    case "FAMC":
                    case "PLAC":
                    case "SOUR":
                        resultContainer.Warnings.Add($"Skipped Adoption Type='{chunk.Type}'");
                        break;

                    default:
                        resultContainer.Errors.Add($"Failed to handle Adoption Type='{chunk.Type}'");
                        break;
                }
            }

            return adoption;
        }
    }
}
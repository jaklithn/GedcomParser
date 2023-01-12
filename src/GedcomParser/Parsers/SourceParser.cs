using System;
using System.Collections.Generic;
using System.Linq;
using GedcomParser.Entities;
using GedcomParser.Entities.Internal;
using GedcomParser.Extensions;

namespace GedcomParser.Parsers
{
    public static class SourceParser
    {
        internal static void ParseSource(this ResultContainer resultContainer, GedcomChunk indiChunk)
        {
            var source = new Source { Id = indiChunk.Id };

            foreach (var chunk in indiChunk.SubChunks)
            {
                switch (chunk.Type)
                {
                    case "AUTH":
                        source.Author = chunk.Data;
                        break;

                    case "ABBR":
                        source.Abbreviation = chunk.Data;
                        break;

                    case "NAME":
                        source.Name = chunk.Data;
                        break;

                    case "TITL":
                        source.Title = chunk.Data;
                        break;

                    case "PUBL":
                        source.Publication = chunk.Data;
                        break;

                    case "TEXT":
                        source.Text = chunk.Data;
                        break;

                    case "REFN":
                        source.Reference = chunk.Data;
                        break;

                    case "REPO":
                        source.RepositoryId = chunk.Data;
                        break;

                    case "RIN":
                        source.RecordId = chunk.Data;
                        break;

                    // Deliberately skipped for now
                    case "_TYPE":
                    case "_MEDI":
                        resultContainer.Warnings.Add($"Skipped Source Record Type='{chunk.Type}'");
                        break;

                    default:
                        resultContainer.Errors.Add($"Failed to handle Source Record Type='{chunk.Type}'");
                        break;
                }
            }

            resultContainer.Sources.Add(source);
        }

        internal static string GetEventType(GedcomChunk chunk)
        {
            return chunk.SubChunks.SingleOrDefault(c => c.Type == "TYPE")?.Data.ToLower();
        }
    }
}
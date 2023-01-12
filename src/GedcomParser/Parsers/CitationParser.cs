using System;
using System.Linq;
using GedcomParser.Entities;
using GedcomParser.Entities.Internal;
using GedcomParser.Extensions;

namespace GedcomParser.Parsers
{
    public static class CitationParser
    {
        internal static Citation ParseCitation(this ResultContainer resultContainer, GedcomChunk incomingChunk)
        {
            var sourceChunk = incomingChunk;
            var citation = new Citation();

            if (incomingChunk.Reference.IsSpecified())
            {
                citation.Id = incomingChunk.Reference;

                // Parse source citation chunk
                foreach (var chunk in incomingChunk.SubChunks)
                {
                    switch (chunk.Type)
                    {
                        case "PAGE":
                            citation.Page = chunk.Data;
                            break;

                        case "DATA":
                        case "EVEN":
                        case "QUAY":
                            resultContainer.Warnings.Add($"Skipped Source Citation Type='{chunk.Type}'");
                            break;

                        default:
                            resultContainer.Errors.Add($"Failed to handle Source Citation Type='{chunk.Type}'");
                            break;
                    }
                }

                // Add citation reference to specific source
                sourceChunk = resultContainer.GetIdChunk(sourceChunk.Reference);
                if (sourceChunk == null)
                {
                    throw new Exception($"Unable to find Source Chunk with Id='{incomingChunk.Reference}'");
                }

                // Check sources
                var source = resultContainer.Sources.Find(s => s.Id == sourceChunk.Id);
                if(source == null)
                {
                    throw new Exception($"Unable to find Source Object with Id='{incomingChunk.Reference}'");
                }

                source.Citations.Add(citation);
            }

            return citation;
        }

        internal static string GetEventType(GedcomChunk chunk)
        {
            return chunk.SubChunks.SingleOrDefault(c => c.Type == "TYPE")?.Data.ToLower();
        }
    }
}
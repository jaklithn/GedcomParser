using System;
using System.Collections.Generic;
using System.Linq;
using GedcomParser.Entities;
using GedcomParser.Entities.Internal;
using GedcomParser.Extensions;

namespace GedcomParser.Parsers
{
    public static class EventParser
    {
        internal static Event ParseEvent(this ResultContainer resultContainer, GedcomChunk incomingChunk)
        {
            var currentEvent = new Event { Name = incomingChunk.Data };

            foreach (var chunk in incomingChunk.SubChunks)
            {
                switch (chunk.Type)
                {
                    case "DATE":
                        currentEvent.DatePlace = resultContainer.ParseDatePlace(chunk);
                        break;

                    case "DSCR":
                        currentEvent.Description = resultContainer.ParseText(chunk.Data, chunk);
                        break;

                    case "ROLE":
                        currentEvent.Role = resultContainer.ParseText(chunk.Data, chunk);
                        break;

                    case "TYPE":
                        currentEvent.Type = resultContainer.ParseText(chunk.Data, chunk);
                        break;

                    // Deliberately skipped for now
                    //resultContainer.Warnings.Add($"Skipped Event Type='{chunk.Type}'");
                    //break;

                    default:
                        resultContainer.Errors.Add($"Failed to handle Event Type='{chunk.Type}'");
                        break;
                }
            }

            return currentEvent;
        }

        internal static string GetEventType(GedcomChunk chunk)
        {
            return chunk.SubChunks.SingleOrDefault(c => c.Type == "TYPE")?.Data.ToLower();
        }
    }
}
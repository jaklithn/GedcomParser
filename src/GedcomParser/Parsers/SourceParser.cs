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
                    /*
                     n <XREF:SOUR> SOUR
                       +1 DATA
                            +2 EVEN <EVENTS_RECORDED>
                                +3 DATE <DATE_PERIOD>
                                +3 PLAC <SOURCE_JURISDICTION_PLACE>
                            +2 AGNC <RESPONSIBLE_AGENCY>
                            +2 <<NOTE_STRUCTURE>>
                       +1 AUTH <SOURCE_ORIGINATOR>
                       +1 TITL <SOURCE_DESCRIPTIVE_TITLE>
                       +1 ABBR <SOURCE_FILED_BY_ENTRY>
                       +1 PUBL <SOURCE_PUBLICATION_FACTS>
                       +1 TEXT <TEXT_FROM_SOURCE>
                       +1 <<SOURCE_REPOSITORY_CITATION>>
                       +1 REFN <USER_REFERENCE_NUMBER>
                            +2 TYPE <USER_REFERENCE_TYPE>
                       +1 RIN <AUTOMATED_RECORD_ID>
                       +1 <<CHANGE_DATE>>
                       +1 <<NOTE_STRUCTURE>>
                       +1 <<MULTIMEDIA_LINK>>
                     */

                    case "DATA":
                        var currentEvent = chunk.SubChunks.SingleOrDefault(c => c.Type == "EVEN");
                        if (currentEvent != null)
                            source.Events.Add(resultContainer.ParseEvent(currentEvent));

                        var agency = chunk.SubChunks.SingleOrDefault(c => c.Type == "AGNC");
                        if (currentEvent != null)
                            source.ResponsibleAgency = resultContainer.ParseText(agency.Data, agency);

                        var note = chunk.SubChunks.SingleOrDefault(c => c.Type == "NOTE");
                        if (note != null)
                            source.Notes.Add(resultContainer.ParseNote(note.Data, note));

                        break;


                    case "AUTH":
                        source.Author = chunk.Data;
                        break;

                    case "TITL":
                        source.Title = resultContainer.ParseText(chunk.Data, chunk);
                        break;

                    case "ABBR":
                        source.Abbreviation = chunk.Data;
                        break;

                    case "PUBL":
                        source.Publication = chunk.Data;
                        break;

                    case "TEXT":
                        source.Text = resultContainer.ParseText(chunk.Data, chunk);
                        break;

                    case "REPO":
                        source.RepositoryId = chunk.Data; // TODO - Add repository class
                        break;

                    case "REFN":
                        source.Reference = chunk.Data;
                        break;

                    case "RIN":
                        source.AutorecordId = chunk.Data;
                        break;

                    case "NAME":
                        source.Name = chunk.Data;
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

        internal static string GetSourceType(GedcomChunk chunk)
        {
            return chunk.SubChunks.SingleOrDefault(c => c.Type == "TYPE")?.Data.ToLower();
        }
    }
}
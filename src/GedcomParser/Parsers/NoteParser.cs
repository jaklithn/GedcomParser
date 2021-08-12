using System;
using System.Text;
using GedcomParser.Entities.Internal;
using GedcomParser.Extensions;


namespace GedcomParser.Parsers
{
    public static class NoteParser
    {
        internal static string ParseNote(this ResultContainer resultContainer, string previousNote, GedcomChunk incomingChunk)
        {
            var noteChunk = incomingChunk;
            if (incomingChunk.Reference.IsSpecified())
            {
                noteChunk = resultContainer.GetIdChunk(noteChunk.Reference);
                if (noteChunk == null)
                {
                    throw new Exception($"Unable to find Note with Id='{incomingChunk.Reference}'");
                }
            }

            var sb = new StringBuilder();
            foreach (var chunk in noteChunk.SubChunks)
            {
                if (chunk.IsUnwantedBlob())
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

                    // Deliberately skipped for now
                    case "_PLC":
                    case "DATE":
                    case "PLAC":
                    case "SOUR":
                        resultContainer.Warnings.Add($"Skipped Note Type='{chunk.Type}'");
                        break;

                    default:
                        resultContainer.Errors.Add($"Failed to handle Note Type='{chunk.Type}'");
                        break;
                }
            }

            return previousNote.IsSpecified() ? previousNote + Environment.NewLine + sb : sb.ToString();
        }

        private static bool IsUnwantedBlob(this GedcomChunk chunk)
        {
            // TODO: We should probably make this check more intelligent 
            return chunk.Data?.Contains("<span") ?? false;
        }
    }
}
using System;
using System.Text;
using GedcomParser.Entities.Internal;
using GedcomParser.Extensions;


namespace GedcomParser.Parsers
{
    public static class TextParser
    {
        internal static string ParseText(this ResultContainer resultContainer, string previousText, GedcomChunk incomingChunk)
        {
            var textChunk = incomingChunk;
            if (incomingChunk.Reference.IsSpecified())
            {
                textChunk = resultContainer.GetIdChunk(textChunk.Reference);
                if (textChunk == null)
                {
                    throw new Exception($"Unable to find Text with Id='{incomingChunk.Reference}'");
                }
            }

            var sb = new StringBuilder();
            foreach (var chunk in textChunk.SubChunks)
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
                        resultContainer.Warnings.Add($"Skipped {incomingChunk.Type} Type='{chunk.Type}'");
                        break;

                    default:
                        resultContainer.Errors.Add($"Failed to handle {incomingChunk.Type} Type='{chunk.Type}'");
                        break;
                }
            }

            return previousText.IsSpecified() ? previousText + Environment.NewLine + sb : sb.ToString();
        }

        private static bool IsUnwantedBlob(this GedcomChunk chunk)
        {
            // TODO: We should probably make this check more intelligent 
            return chunk.Data?.Contains("<span") ?? false;
        }
    }
}
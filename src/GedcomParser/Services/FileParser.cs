using System.Collections.Generic;
using System.IO;
using System.Linq;
using GedcomParser.Entities;
using GedcomParser.Entities.Internal;
using GedcomParser.Parsers;


namespace GedcomParser.Services
{
    public static class FileParser
    {
        public static Result ParseFromFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            return ParseLines(lines);
        }

        public static Result ParseLines(IEnumerable<string> lines)
        {
            var gedcomLines = lines.Select(GedcomLine.Parse);
            return ParseGedcomLines(gedcomLines);
        }

        private static Result ParseGedcomLines(IEnumerable<GedcomLine> gedcomLines)
        {
            var chunks = GetChunks(gedcomLines);
            var result = chunks.ParseChunks();

            return result.AsResult();
        }

        internal static IEnumerable<GedcomChunk> GetChunks(IEnumerable<GedcomLine> gedcomLines)
        {
            var gedcomChunkLevels = new GedcomChunkLevels();

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
                    var parent = gedcomChunkLevels.GetParentChunk(chunk);
                    parent.SubChunks.Add(chunk);
                }

                gedcomChunkLevels.Set(chunk);
            }

            return topChunks;
        }
    }
}
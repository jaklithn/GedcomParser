using System.Collections.Generic;
using System.Linq;
using GedcomParser.Entities.Internal;


namespace GedcomParser.Services
{
    public static class TypeParser
    {
        public static IDictionary<string, List<string>> ParseTypeStructure(IEnumerable<string> lines)
        {
            var gedcomLines = lines.Select(GedcomLine.Parse);
            var chunks = FileParser.GetChunks(gedcomLines);

            var types = new Dictionary<string, HashSet<string>>();
            foreach (var chunk in chunks)
            {
                chunk.ParseTypeNodes(types);
            }

            return types.OrderBy(t => t.Key).ToDictionary(t => t.Key, t => t.Value.OrderBy(v => v).ToList());
        }

        private static void ParseTypeNodes(this GedcomChunk chunk, IDictionary<string, HashSet<string>> types)
        {
            var type = chunk.GetType(types);
            foreach (var subChunk in chunk.SubChunks)
            {
                type.Add(subChunk.Type);
                subChunk.ParseTypeNodes(types);
            }
        }

        private static HashSet<string> GetType(this GedcomChunk chunk, IDictionary<string, HashSet<string>> types)
        {
            if (!types.ContainsKey(chunk.Type))
            {
                types.Add(chunk.Type, new HashSet<string>());
            }

            return types[chunk.Type];
        }
    }
}
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gedcom.Entities.Gedcom;

namespace Gedcom.Services
{
    public static class GedcomPrinter
    {
        public static void PrintChunks(IEnumerable<GedcomChunk> chunks)
        {
            foreach (var chunk in chunks)
            {
                Debug.WriteLine($"{new string(' ', chunk.Level * 2)}{chunk.Level} {chunk.Id} {chunk.Type} {chunk.Data}");
                PrintChunks(chunk.SubChunks);
            }
        }

        public static void PrintTypes(List<GedcomChunk> chunks)
        {
            var parentTypes = new Dictionary<string, HashSet<string>>();
            var levels = new GedcomChunkLevels();

            AddToTypeTree(parentTypes, levels, chunks);

            Debug.WriteLine("======================================================================");
            Debug.WriteLine("  THESE ARE TYPES DETECTED IN THE TEST FILE");
            Debug.WriteLine("  (A plus sign after a property indicates it is itself a complex type)");
            Debug.WriteLine("======================================================================");

            foreach (var minimalLevel in parentTypes.OrderBy(x => x.Key))
            {
                Debug.WriteLine(minimalLevel.Key);
                foreach (var subType in minimalLevel.Value.OrderBy(x => x))
                {
                    var suffix = parentTypes.Keys.Contains(subType) ? "+" : string.Empty;
                    Debug.WriteLine($"    {subType}{suffix}");
                }
                Debug.WriteLine("");
            }
            Debug.WriteLine("======================================================================");
        }

        private static void AddToTypeTree(Dictionary<string, HashSet<string>> parentTypes, GedcomChunkLevels levels, List<GedcomChunk> chunks)
        {
            foreach (var chunk in chunks)
            {
                if (chunk.Level == 0)
                {
                    if (!parentTypes.ContainsKey(chunk.Type))
                    {
                        parentTypes.Add(chunk.Type, new HashSet<string>());
                    }
                }
                else
                {
                    var parentChunk = levels.GetParentChunk(chunk);
                    if (!parentTypes.ContainsKey(parentChunk.Type))
                    {
                        parentTypes.Add(parentChunk.Type, new HashSet<string>());
                    }
                    var parentType = parentTypes[parentChunk.Type];
                    parentType.Add(chunk.Type);
                }

                levels.Set(chunk);
                AddToTypeTree(parentTypes, levels, chunk.SubChunks);
            }
        }
    }
}
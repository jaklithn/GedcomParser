using System.Collections.Generic;


namespace GedcomParser.Entities.Internal
{
    /// <summary>
    /// Keeps track of the current level hierarchy.
    /// Just for temporary usage during parsing.
    /// </summary>
    internal class GedcomChunkLevels
    {
        private readonly Dictionary<int, GedcomChunk> currentLevelChunks = new Dictionary<int, GedcomChunk>();

        internal void Set(GedcomChunk gedcomChunk)
        {
            if (currentLevelChunks.ContainsKey(gedcomChunk.Level))
            {
                currentLevelChunks.Remove(gedcomChunk.Level);
            }
            currentLevelChunks.Add(gedcomChunk.Level, gedcomChunk);
        }

        internal GedcomChunk GetParentChunk(GedcomChunk gedcomChunk)
        {
            return currentLevelChunks[gedcomChunk.Level - 1];
        }
    }
}
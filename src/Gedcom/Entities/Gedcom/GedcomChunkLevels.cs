using System.Collections.Generic;

namespace Gedcom.Entities.Gedcom
{
    /// <summary>
    /// Keeps track of the current level hierarchy.
    /// Just for temporary usage during parsing.
    /// </summary>
    public class GedcomChunkLevels
    {
        private readonly Dictionary<int, GedcomChunk> _currentLevelChunks = new Dictionary<int, GedcomChunk>();

        public void Set(GedcomChunk gedcomChunk)
        {
            if (_currentLevelChunks.ContainsKey(gedcomChunk.Level))
            {
                _currentLevelChunks.Remove(gedcomChunk.Level);
            }
            _currentLevelChunks.Add(gedcomChunk.Level, gedcomChunk);
        }

        public GedcomChunk GetParentChunk(GedcomChunk gedcomChunk)
        {
            return _currentLevelChunks[gedcomChunk.Level - 1];
        }
    }
}
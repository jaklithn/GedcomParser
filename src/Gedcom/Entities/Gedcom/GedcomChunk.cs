using System.Collections.Generic;

namespace Gedcom.Entities.Gedcom
{
    /// <summary>
    /// This represents a line from the GEDCOM file AND all its related sublines in a structured hierarchy.
    /// </summary>
    internal class GedcomChunk : GedcomLine
    {
        internal List<GedcomChunk> SubChunks { get; }

        internal GedcomChunk(GedcomLine gedcomLine) : base(gedcomLine.Level, gedcomLine.Id, gedcomLine.Type, gedcomLine.Data, gedcomLine.Reference)
        {
            SubChunks = new List<GedcomChunk>();
        }
    }
}
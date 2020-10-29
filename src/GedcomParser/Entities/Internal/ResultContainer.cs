using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GedcomParser.Extensions;


namespace GedcomParser.Entities.Internal
{
    /// <summary>
    /// Collects the result AND keeps track of Id chunks used for cross reference.
    /// </summary>
    internal class ResultContainer : Result
    {
        private readonly ConcurrentDictionary<string, GedcomChunk> idChunks;

        internal ResultContainer()
        {
            idChunks = new ConcurrentDictionary<string, GedcomChunk>();
        }

        internal void AddIdChunk(GedcomChunk chunk)
        {
            if (chunk.Id.IsSpecified())
            {
                idChunks.AddOrUpdate(chunk.Id, chunk, (key, oldValue) => chunk);
            }
            else
            {
                Errors.Add($"{chunk.Type} had no Id");
            }
        }

        internal GedcomChunk GetIdChunk(string id)
        {
            if (idChunks.TryGetValue(id, out var chunk))
            {
                return chunk;
            }
            Errors.Add($"Id={id} was not found");
            return null;
        }

        public Result AsResult()
        {
            return new Result
            {
                Persons = Persons.OrderBy(p => p.Id).ToList(),
                ChildRelations = ChildRelations, 
                SpouseRelations = SpouseRelations,
                SiblingRelations = SiblingRelations, 
                Errors = new HashSet<string>(Errors.OrderBy(e => e)),
                Warnings = new HashSet<string>(Warnings.OrderBy(e => e))
            };
        }
    }
}
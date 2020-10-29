using System;
using GedcomParser.Extensions;


namespace GedcomParser.Entities.Internal
{
    /// <summary>
    /// Represents a line from the GEDCOM file parsed into its properties.
    /// </summary>
    internal class GedcomLine
    {
        public int Level { get; }
        public string Id { get; }
        public string Type { get; }
        public string Data { get; }
        public string Reference { get; }


        internal GedcomLine(int level, string id, string type, string data, string reference)
        {
            Level = level;
            Id = id;
            Type = type.ToUpper();
            Data = data;
            Reference = reference;
        }

        internal static GedcomLine Parse(string line)
        {
            line = line.Trim();
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            var sections = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (sections.Length < 2)
            {
                throw new ArgumentException("Line must have at least Level and Type");
            }
            if (!int.TryParse(sections[0], out var level))
            {
                throw new ArgumentException($"'{sections[0]}' can not be interpreted as valid Level integer");
            }

            // Normal case is no Id
            var id = (string)null;
            var type = sections[1];
            var data = sections.Length > 2 ? line.Substring(line.IndexOf(type) + type.Length).Trim() : null;
            var idReference = (string)null;

            // If there is an Id we take another approach
            if (sections[1].StartsWith("@") && sections[1].EndsWith("@"))
            {
                if (sections.Length < 3)
                {
                    throw new ArgumentException("Line with Id must have at least Level, Id and Type");
                }
                id = sections[1];
                type = sections[2];
                data = sections.Length > 3 ? line.Substring(line.IndexOf(type) + type.Length).Trim() : null;
            }

            if (data.IsSpecified() && data.StartsWith("@") && data.EndsWith("@"))
            {
                idReference = data;
                data = null;
            }

            return new GedcomLine(level, id, type, data, idReference);
        }

    }
}

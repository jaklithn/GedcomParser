using System.Collections.Generic;

namespace GedcomParser.Entities
{
    public class Source
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Abbreviation { get; set; }
        public string Publication { get; set; }
        public string Text { get; set; }
        public string Reference { get; set; }
        public string RepositoryId { get; set; }
        public string RecordId { get; set; }
        public List<Citation> Citations { get; set; } = new List<Citation>();
    }
}
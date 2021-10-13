using System.Collections.Generic;

namespace GedcomParser.Entities
{
    public abstract class Relation
    {
        public string FamilyId { get; set; }
        public Person From { get; set; }
        public Person To { get; set; }
    }

    public class ChildRelation : Relation
    {
        public string Pedigree { get; set; }
        public string Validity { get; set; }
        public string Adoption { get; set; }
    }

    public class SpouseRelation : Relation
    {
        public List<DatePlace> Engagement           { get; set; } = new List<DatePlace>();
        public List<DatePlace> Marriage             { get; set; } = new List<DatePlace>();
        public List<DatePlace> MarriageContract     { get; set; } = new List<DatePlace>();
        public List<DatePlace> MarriageSettlement   { get; set; } = new List<DatePlace>();
        public List<DatePlace> Divorce              { get; set; } = new List<DatePlace>();
        public List<DatePlace> DivorceFiled         { get; set; } = new List<DatePlace>();
        public List<DatePlace> Annulment            { get; set; } = new List<DatePlace>();
        public List<DatePlace> MarriageBann         { get; set; } = new List<DatePlace>();
        public List<DatePlace> MarriageLicense      { get; set; } = new List<DatePlace>();
        public string Relation { get; set; }
        public string Note { get; set; }
    }

    public class SiblingRelation : Relation
    {
    }
}
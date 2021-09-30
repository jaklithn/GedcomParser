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
        public DatePlace Engagement { get; set; }
        public DatePlace Marriage { get; set; }
        public DatePlace MarriageContract { get; set; }
        public DatePlace Divorce { get; set; }
        public DatePlace DivorceFiled { get; set; }
        public DatePlace Annulment { get; set; }
        public string Relation { get; set; }
        public string Note { get; set; }
    }

    public class SiblingRelation : Relation
    {
    }
}
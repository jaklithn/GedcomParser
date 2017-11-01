namespace GedcomParser.Entities
{
    public abstract class Relation
    {
        public string Type { get; }
        public string FamilyId { get; set; }
        public Person From { get; set; }
        public Person To { get; set; }

        public Relation(string type)
        {
            Type = type;
        }
    }

    public class ChildRelation : Relation
    {
        public string Pedigree { get; set; }
        public string Validity { get; set; }
        public string Adoption { get; set; }

        public ChildRelation() : base("Child_To")
        {
        }
    }

    public class SpouseRelation : Relation
    {
        public DatePlace Marriage { get; set; }
        public DatePlace Divorce { get; set; }
        public string Relation { get; set; }
        public string Note { get; set; }

        public SpouseRelation() : base("Spouse_To")
        {
        }
    }

    public class SiblingRelation : Relation
    {
        public SiblingRelation() : base("Sibling_To")
        {
        }
    }
}
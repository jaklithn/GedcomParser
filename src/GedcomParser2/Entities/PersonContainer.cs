using System.Collections.Generic;


namespace GedcomParser.Entities
{
    public class PersonContainer
    {
        public List<Person> Persons { get; }
        public List<ChildRelation> ChildRelations { get; }
        public List<SpouseRelation> SpouseRelations { get; set; }
        public List<SiblingRelation> SiblingRelations { get; set; }

        public PersonContainer()
        {
            Persons = new List<Person>();
            ChildRelations = new List<ChildRelation>();
            SpouseRelations = new List<SpouseRelation>();
            SiblingRelations = new List<SiblingRelation>();
        }
    }
}
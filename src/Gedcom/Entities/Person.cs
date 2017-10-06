namespace Gedcom.Entities
{
    public class Person
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DatePlace Birth { get; set; }
        public DatePlace Death { get; set; }
        public DatePlace Buried { get; set; }
        public DatePlace Baptized { get; set; }
        public string Education { get; set; }
        public string Religion { get; set; }
        public string Note { get; set; }
        public string Changed { get; set; }
        public string Occupation { get; set; }
        public string Health { get; set; }
        public string Title { get; set; }
    }
}
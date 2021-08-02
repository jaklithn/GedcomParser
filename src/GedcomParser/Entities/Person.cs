using System.Collections.Generic;

namespace GedcomParser.Entities
{
    public class Person
    {
        public string Id { get; set; }
        public string Uid { get; set; }
        public string IdNumber { get; set; }
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
        public Address Address { get; set; }
        public Adoption Adoption { get; set; }
        public DatePlace Residence { get; set; }
        public List<DatePlace> Emigrated { get; set; } = new List<DatePlace>();
        public List<DatePlace> Immigrated { get; set; } = new List<DatePlace>();
        public DatePlace BecomingCitizen { get; set; }
        public DatePlace Graduation { get; set; }
    }
}
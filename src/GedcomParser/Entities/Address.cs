using System.Collections.Generic;


namespace GedcomParser.Entities
{
    public class Address
    {
        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Country { get; set; }

        public List<string> Phone { get; set; }

        public List<string> Fax { get; set; }

        public List<string> Email { get; set; }

        public List<string> Web { get; set; }

        public Address()
        {
            Phone = new List<string>();
            Fax = new List<string>();
            Email = new List<string>();
            Web = new List<string>();
        }

    }
}

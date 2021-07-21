using System;
using GedcomParser.Entities;
using GedcomParser.Entities.Internal;


namespace GedcomParser.Parsers
{
    public static class AddressParser
    {
        internal static Address ParseAddress(this ResultContainer resultContainer, GedcomChunk addressChunk)
        {
            // Top level node can also contain a full address or first part of it ...
            var address = new Address {Street = addressChunk.Data};

            foreach (var chunk in addressChunk.SubChunks)
            {
                switch (chunk.Type)
                {
                    case "CONT":
                    case "ADR1":
                    case "ADR2":
                    case "ADR3":
                        address.Street += Environment.NewLine + chunk.Data;
                        break;

                    case "CITY":
                        address.City += chunk.Data;
                        break;

                    case "STAE":
                        address.State += chunk.Data;
                        break;

                    case "POST":
                        address.ZipCode += chunk.Data;
                        break;

                    case "CTRY":
                        address.Country += chunk.Data;
                        break;

                    case "PHON":
                        address.Phone.Add(chunk.Data);
                        break;

                    case "EMAIL":
                        address.Email.Add(chunk.Data);
                        break;

                    case "FAX":
                        address.Fax.Add(chunk.Data);
                        break;

                    case "WWW":
                        address.Web.Add(chunk.Data);
                        break;

                    default:
                        resultContainer.Errors.Add($"Failed to handle Address Type='{chunk.Type}'");
                        break;
                }
            }

            return address;
        }
    }
}
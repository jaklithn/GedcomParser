using System.Collections.Generic;
using System.Reflection;
using GedcomParser.Test.Extensions;


namespace GedcomParser.Test.Services
{
    public static class ResourceHelper
    {
        public static IEnumerable<string> GetLines(string fileName)
        {
            return Assembly.GetExecutingAssembly().GetResourceAsLines($"Resources.{fileName}");
        }
    }
}
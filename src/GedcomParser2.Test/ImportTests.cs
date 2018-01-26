using System.Diagnostics;
using System.IO;
using GedcomParser.Entities.Internal;
using GedcomParser.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace GedcomParser.Test
{
    [TestClass]
    public class ImportTests
    {
        [TestMethod]
        public void CanParse()
        {
            // Arrange
            var currentDir = Directory.GetCurrentDirectory();
            var baseDir = currentDir.Substring(0, currentDir.IndexOf(@"\bin\Debug"));
            var filePath = Path.Combine(baseDir, "Resources", "StefanFamily.ged");
            //var filePath = Path.Combine(baseDir, "Resources", "StefanFamilyAll.ged");
            //var filePath = Path.Combine(baseDir, "Resources", "Windsor.ged");
            //var filePath = Path.Combine(baseDir, "Resources", "Kennedy.ged");
            //var filePath = Path.Combine(baseDir, "Resources", "TGC551.ged");
            var fileParser = new FileParser();

            // Act
            fileParser.Parse(filePath);

            // Assert
            var relationCount = fileParser.PersonContainer.ChildRelations.Count + fileParser.PersonContainer.SpouseRelations.Count + fileParser.PersonContainer.SiblingRelations.Count;
            Assert.IsTrue(fileParser.PersonContainer.Persons.Count > 0);
            Assert.IsTrue(relationCount > 0);
            //var childStatusChildren = fileParser.Relations.OfType<ChildRelation>().Where(c => !string.IsNullOrEmpty(c.Pedigree)).ToList();
            //Assert.IsTrue(childStatusChildren.Count > 0);
            Debug.WriteLine("======================================================================");
            Debug.WriteLine($"GEDCOM file processed: '{filePath}'");
            Debug.WriteLine($"{fileParser.PersonContainer.Persons.Count} persons successfully parsed with {relationCount} relations detected");
            Debug.WriteLine("======================================================================");
        }

        [TestMethod]
        public void CanCollectTypes()
        {
            // Arrange
            var currentDir = Directory.GetCurrentDirectory();
            var baseDir = currentDir.Substring(0, currentDir.IndexOf(@"\bin\Debug"));
            var filePath = Path.Combine(baseDir, "Resources", "Windsor.ged");
            var fileParser = new FileParser();

            // Act
            var gedcomTopChunks = fileParser.GenerateChunks(filePath);
            DebugPrinter.PrintTypes(gedcomTopChunks);
        }

        [TestMethod]
        public void CanPrintChunks()
        {
            // Arrange
            var currentDir = Directory.GetCurrentDirectory();
            var baseDir = currentDir.Substring(0, currentDir.IndexOf(@"\bin\Debug"));
            var filePath = Path.Combine(baseDir, "Resources", "Windsor.ged");
            var fileParser = new FileParser();

            // Act
            var gedcomTopChunks = fileParser.GenerateChunks(filePath);
            DebugPrinter.PrintChunks(gedcomTopChunks);
        }

        [TestMethod]
        public void CanParseLine()
        {
            // Arrange
            const string line = "3 ADDR 7108 South Pine Cone Street";

            // Act
            var gedcomLine = GedcomLine.Parse(line);

            // Assert
            Assert.AreEqual(3, gedcomLine.Level);
            Assert.AreEqual("ADDR", gedcomLine.Type);
            Assert.AreEqual("7108 South Pine Cone Street", gedcomLine.Data);
        }

        [TestMethod]
        public void CanParseShortLine()
        {
            // Arrange
            const string line = "3 ADDR";

            // Act
            var gedcomLine = GedcomLine.Parse(line);

            // Assert
            Assert.AreEqual(3, gedcomLine.Level);
            Assert.AreEqual("ADDR", gedcomLine.Type);
            Assert.AreEqual(null, gedcomLine.Data);
        }
    }
}

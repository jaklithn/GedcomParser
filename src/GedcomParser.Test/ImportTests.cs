using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using GedcomParser.Entities.Internal;
using GedcomParser.Services;
using Xunit;
using FluentAssertions;


namespace GedcomParser.Test
{
    public class ImportTests
    {
        [Fact]
        public void CanParse()
        {
            // Arrange
            var filePath = Path.Combine(GetResourcePath(), "StefanFamily.ged");
            //var filePath = Path.Combine(baseDir, "Resources", "StefanFamilyAll.ged");
            //var filePath = Path.Combine(baseDir, "Resources", "Windsor.ged");
            //var filePath = Path.Combine(baseDir, "Resources", "Kennedy.ged");
            //var filePath = Path.Combine(baseDir, "Resources", "TGC551.ged");
            var fileParser = new FileParser();

            // Act
            fileParser.Parse(filePath);

            // Assert
            var relationCount = fileParser.PersonContainer.ChildRelations.Count + fileParser.PersonContainer.SpouseRelations.Count + fileParser.PersonContainer.SiblingRelations.Count;
            fileParser.PersonContainer.Persons.Count.Should().BeGreaterThan(0);
            relationCount.Should().BeGreaterThan(0);
            //var childStatusChildren = fileParser.Relations.OfType<ChildRelation>().Where(c => !string.IsNullOrEmpty(c.Pedigree)).ToList();
            //Assert.IsTrue(childStatusChildren.Count > 0);
            Debug.WriteLine("======================================================================");
            Debug.WriteLine($"GEDCOM file processed: '{filePath}'");
            Debug.WriteLine($"{fileParser.PersonContainer.Persons.Count} persons successfully parsed with {relationCount} relations detected");
            Debug.WriteLine("======================================================================");
        }

        [Fact]
        public void CanCollectTypes()
        {
            // Arrange
            var filePath = Path.Combine(GetResourcePath(), "Windsor.ged");
            var fileParser = new FileParser();

            // Act
            var gedcomTopChunks = fileParser.GenerateChunks(filePath);
            DebugPrinter.PrintTypes(gedcomTopChunks);
        }

        [Fact]
        public void CanPrintChunks()
        {
            // Arrange
            var filePath = Path.Combine(GetResourcePath(), "Windsor.ged");
            var fileParser = new FileParser();

            // Act
            var gedcomTopChunks = fileParser.GenerateChunks(filePath);
            DebugPrinter.PrintChunks(gedcomTopChunks);
        }

        [Fact]
        public void CanParseLine()
        {
            // Arrange
            const string line = "3 ADDR 7108 South Pine Cone Street";

            // Act
            var gedcomLine = GedcomLine.Parse(line);

            // Assert
            gedcomLine.Level.Should().Be(3);
            gedcomLine.Type.Should().Be("ADDR");
            gedcomLine.Data.Should().Be("7108 South Pine Cone Street");
        }

        [Fact]
        public void CanParseShortLine()
        {
            // Arrange
            const string line = "3 ADDR";

            // Act
            var gedcomLine = GedcomLine.Parse(line);

            // Assert
            gedcomLine.Level.Should().Be(3);
            gedcomLine.Type.Should().Be("ADDR");
            gedcomLine.Data.Should().BeNull();
        }
        
        private static string GetResourcePath()
        {
            var sep = Path.DirectorySeparatorChar;
            var currentDir = Directory.GetCurrentDirectory();
            var baseDir = currentDir.Substring(0, currentDir.IndexOf($"{sep}bin{sep}Debug"));
            return Path.Combine(baseDir, "Resources");
        }

    }
}

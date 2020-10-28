using System.IO;
using GedcomParser.Entities;
using GedcomParser.Services;
using GedcomParser.Test.Services;
using Xunit;
using FluentAssertions;


namespace GedcomParser.Test
{
    public class ExportTests
    {
        [Fact]
        public void CanExport()
        {
            // Arrange
            //const string gedcomFileName = "Kennedy.ged";
            //const string gedcomFileName = "Windsor.ged";
            const string gedcomFileName = "StefanFamily.ged";
            const string zipFileName = "PersonContainer.zip";
            const string jsonFileName = "Persons.json";
            var gedFilePath = Path.Combine(GetResourcePath(), gedcomFileName);
            var fileParser = new FileParser();
            fileParser.Parse(gedFilePath);

            // Act
            var zipFilePath = Path.Combine(GetResourcePath(), zipFileName);
            ZipHandler.SaveToZipFile(fileParser.PersonContainer, zipFilePath, jsonFileName);

            // Assert
            var personContainer = ZipHandler.ParseFromZipFile<PersonContainer>(zipFilePath, jsonFileName);
            personContainer.Persons.Count.Should().Be(fileParser.PersonContainer.Persons.Count);
            personContainer.ChildRelations.Count.Should().Be(fileParser.PersonContainer.ChildRelations.Count);
            personContainer.SpouseRelations.Count.Should().Be(fileParser.PersonContainer.SpouseRelations.Count);
            personContainer.SiblingRelations.Count.Should().Be(fileParser.PersonContainer.SiblingRelations.Count);
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
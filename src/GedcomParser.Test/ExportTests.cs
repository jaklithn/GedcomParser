using System.IO;
using GedcomParser.Entities;
using GedcomParser.Services;
using GedcomParser.Test.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace GedcomParser.Test
{
    [TestClass]
    public class ExportTests
    {
        [TestMethod]
        public void CanExport()
        {
            // Arrange
            //const string gedcomFileName = "Kennedy.ged";
            //const string gedcomFileName = "Windsor.ged";
            const string gedcomFileName = "StefanFamily.ged";
            const string zipFileName = "PersonContainer.zip";
            const string jsonFileName = "Persons.json";
            var currentDir = Directory.GetCurrentDirectory();
            var baseDir = currentDir.Substring(0, currentDir.IndexOf(@"\bin\Debug"));
            var gedFilePath = Path.Combine(baseDir, "Resources", gedcomFileName);
            var fileParser = new FileParser();
            fileParser.Parse(gedFilePath);

            // Act
            var zipFilePath = Path.Combine(currentDir, zipFileName);
            ZipHandler.SaveToZipFile(fileParser.PersonContainer, zipFilePath, jsonFileName);

            // Assert
            var personContainer = ZipHandler.ParseFromZipFile<PersonContainer>(zipFilePath, jsonFileName);
            Assert.AreEqual(fileParser.PersonContainer.Persons.Count, personContainer.Persons.Count);
            Assert.AreEqual(fileParser.PersonContainer.ChildRelations.Count, personContainer.ChildRelations.Count);
            Assert.AreEqual(fileParser.PersonContainer.SpouseRelations.Count, personContainer.SpouseRelations.Count);
            Assert.AreEqual(fileParser.PersonContainer.SiblingRelations.Count, personContainer.SiblingRelations.Count);
        }
    }
}
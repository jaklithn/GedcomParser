using System;
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
            var currentDir = Directory.GetCurrentDirectory();
            var baseDir = currentDir.Substring(0, currentDir.IndexOf(@"\bin\Debug"));
            var gedFilePath = Path.Combine(baseDir, "Resources", "StefanFamily.ged");
            var fileParser = new FileParser();
            fileParser.Parse(gedFilePath);

            // Act
            const string jsonFileName = "Persons.json";
            var directory = new DirectoryInfo(Environment.CurrentDirectory).Parent?.Parent?.Parent?.Parent?.Parent?.FullName ?? "";
            var subPath = @"GraphDatabaseDemo\src\Neo.Genealogy\Resources\PersonContainer.zip";
            var zipFilePath = Path.Combine(directory, subPath);
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
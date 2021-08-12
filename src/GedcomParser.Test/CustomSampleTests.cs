using System;
using System.Linq;
using GedcomParser.Services;
using GedcomParser.Test.Extensions;
using GedcomParser.Test.Services;
using Shouldly;
using Xunit;


namespace GedcomParser.Test
{
    public class CustomSampleTests
    {
        [Fact]
        public void CanParseKennedyFamily()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.Kennedy.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            // result.Warnings.ShouldBeEmptyWithFeedback();
            result.Warnings.Count.ShouldBe(3);
            result.Warnings.ShouldContain("Skipped Person Type='FAMC'");
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
            result.Warnings.ShouldContain("Skipped Person Type='HIST'");
        }

        [Fact]
        public void CanParseStefanFamily()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.Stefan.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            // result.Errors.ShouldBeEmptyWithFeedback();
            result.Errors.Count.ShouldBe(2);
            result.Errors.ShouldContain("Failed to handle top level Type='_GRP'");
            result.Errors.ShouldContain("Failed to handle top level Type='_PLC'");
            // result.Warnings.ShouldBeEmptyWithFeedback();
            result.Warnings.Count.ShouldBe(18);
        }

        [Fact]
        public void CanParseWindsorFamily()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.Windsor.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            // result.Warnings.ShouldBeEmptyWithFeedback();
            result.Warnings.Count.ShouldBe(1);
            result.Warnings.ShouldContain("Skipped Person Type='OBJE'");
        }

        [Fact]
        public void CanParseMultipleImmigrationEventsFamily()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleImmigrationEvents.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
            Assert.Collection(result.Persons, person => { Assert.Equal("Travis", person.FirstName.Trim()); Assert.Equal(2, person.Immigrated.Count); },
                                              person => { Assert.Equal("Niles", person.FirstName.Trim()); Assert.Empty(person.Immigrated); });            
        }

        [Fact]
        public void CanParseMultipleEmigrationEventsFamily()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleEmigrationEvents.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
            Assert.Collection(result.Persons, person => { Assert.Equal("Travis", person.FirstName.Trim()); Assert.Equal(2, person.Emigrated.Count); },
                                              person => { Assert.Equal("Niles", person.FirstName.Trim()); Assert.Empty(person.Emigrated); });
        }
    }
}
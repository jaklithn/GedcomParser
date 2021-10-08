using System;
using GedcomParser.Entities;
using GedcomParser.Services;
using GedcomParser.Test.Extensions;
using GedcomParser.Test.Services;
using Shouldly;
using Xunit;


namespace GedcomParser.Test
{
    /// <summary>
    /// Parsing test files provided by <see href="https://www.gedcom.org/samples.html">Gedcom organisation</see>
    /// </summary>
    public class GedcomStandardTests
    {
        [Fact]
        public void CanParseMinimal555()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomStandard.MINIMAL555.GED");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.Warnings.ShouldBeEmptyWithFeedback();
        }

        [Fact]
        public void CanParse555Sample()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomStandard.555SAMPLE.GED");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            // result.Warnings.ShouldBeEmptyWithFeedback();
            result.Warnings.Count.ShouldBe(3);
            result.Warnings.ShouldContain("Skipped Adoption Type='DATE'");
            result.Warnings.ShouldContain("Skipped Person Type='FAMC'");
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
        }

        [Fact]
        public void CanParse555Sample16BigEndian()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomStandard.555SAMPLE16BE.GED");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            // result.Warnings.ShouldBeEmptyWithFeedback();
            result.Warnings.Count.ShouldBe(3);
            result.Warnings.ShouldContain("Skipped Adoption Type='DATE'");
            result.Warnings.ShouldContain("Skipped Person Type='FAMC'");
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
        }

        [Fact]
        public void CanParse555Sample16LittleEndian()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomStandard.555SAMPLE16LE.GED");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            // result.Warnings.ShouldBeEmptyWithFeedback();
            result.Warnings.Count.ShouldBe(3);
            result.Warnings.ShouldContain("Skipped Adoption Type='DATE'");
            result.Warnings.ShouldContain("Skipped Person Type='FAMC'");
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
        }

        [Fact]
        public void CanParseReMarriage()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomStandard.REMARR.GED");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            // result.Warnings.ShouldBeEmptyWithFeedback();
            result.Warnings.Count.ShouldBe(1);
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
        }

        [Fact]
        public void CanParseReEngagement()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomStandard.REENGA.GED");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.Warnings.Count.ShouldBe(2);
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
            result.Warnings.ShouldContain("Skipped Person Type='FAMC'");
        }

        [Fact]
        public void CanParseDivorceFiled()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomStandard.DIVF.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.Warnings.Count.ShouldBe(2);
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
            result.Warnings.ShouldContain("Skipped Person Type='FAMC'");
        }

        [Fact]
        public void CanParseAnnulmentFiled()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomStandard.ANNUL.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.Warnings.Count.ShouldBe(2);
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
            result.Warnings.ShouldContain("Skipped Person Type='FAMC'");
        }
        [Fact]
        public void CanParseSameSexMarriage()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomStandard.SSMARR.GED");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            // result.Warnings.ShouldBeEmptyWithFeedback();
            result.Warnings.Count.ShouldBe(1);
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
        }

        [Fact]
        public void CanParseMarriageContract()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomStandard.MARC.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.Warnings.Count.ShouldBe(1);
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
        }

        [Fact]
        public void CanParseMarriageSettlement()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomStandard.MARS.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.Warnings.Count.ShouldBe(1);
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
        }
    }
}
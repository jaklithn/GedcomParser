using System;
using GedcomParser.Services;
using GedcomParser.Test.Services;
using Shouldly;
using Xunit;


namespace GedcomParser.Test
{
    /// <summary>
    /// 
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
            string.Join(Environment.NewLine, result.Errors).ShouldBeEmpty();
            string.Join(Environment.NewLine, result.Warnings).ShouldBeEmpty();
        }

        [Fact]
        public void CanParse555Sample()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomStandard.555SAMPLE.GED");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            string.Join(Environment.NewLine, result.Errors).ShouldBeEmpty();
            string.Join(Environment.NewLine, result.Warnings).ShouldBeEmpty();
        }

        [Fact]
        public void CanParse555Sample16BigEndian()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomStandard.555SAMPLE16BE.GED");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            string.Join(Environment.NewLine, result.Errors).ShouldBeEmpty();
            string.Join(Environment.NewLine, result.Warnings).ShouldBeEmpty();
        }

        [Fact]
        public void CanParse555Sample16LittleEndian()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomStandard.555SAMPLE16LE.GED");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            string.Join(Environment.NewLine, result.Errors).ShouldBeEmpty();
            string.Join(Environment.NewLine, result.Warnings).ShouldBeEmpty();
        }

        [Fact]
        public void CanParseReMarriage()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomStandard.REMARR.GED");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            string.Join(Environment.NewLine, result.Errors).ShouldBeEmpty();
            string.Join(Environment.NewLine, result.Warnings).ShouldBeEmpty();
        }

        [Fact]
        public void CanParseSameSexMarriage()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomStandard.SSMARR.GED");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            string.Join(Environment.NewLine, result.Errors).ShouldBeEmpty();
            string.Join(Environment.NewLine, result.Warnings).ShouldBeEmpty();
        }

    }
}
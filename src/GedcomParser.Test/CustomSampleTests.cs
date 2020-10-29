using System;
using GedcomParser.Services;
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
            string.Join(Environment.NewLine, result.Errors).ShouldBeEmpty();
            string.Join(Environment.NewLine, result.Warnings).ShouldBeEmpty();
        }

        [Fact]
        public void CanParseStefanFamily()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.Stefan.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            string.Join(Environment.NewLine, result.Errors).ShouldBeEmpty();
            string.Join(Environment.NewLine, result.Warnings).ShouldBeEmpty();
        }

        [Fact]
        public void CanParseWindsorFamily()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.Windsor.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            string.Join(Environment.NewLine, result.Errors).ShouldBeEmpty();
            string.Join(Environment.NewLine, result.Warnings).ShouldBeEmpty();
        }
    }
}
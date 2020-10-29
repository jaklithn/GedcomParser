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
    public class GedcomTortureTests
    {
        [Fact]
        public void CanParseTgc551()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomTorture.TGC551.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            string.Join(Environment.NewLine, result.Errors).ShouldBeEmpty();
        }

        [Fact]
        public void CanParseTgc551Lf()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomTorture.TGC551LF.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            string.Join(Environment.NewLine, result.Errors).ShouldBeEmpty();
        }

        [Fact]
        public void CanParseTgc55C()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomTorture.TGC55C.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            string.Join(Environment.NewLine, result.Errors).ShouldBeEmpty();
        }

        [Fact]
        public void CanParseTgc55Clf()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomTorture.TGC55CLF.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            string.Join(Environment.NewLine, result.Errors).ShouldBeEmpty();
        }
    }
}
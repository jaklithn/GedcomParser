using GedcomParser.Services;
using GedcomParser.Test.Extensions;
using GedcomParser.Test.Services;
using Xunit;


namespace GedcomParser.Test
{
    /// <summary>
    /// Parsing test files found on <see href="http://www.geditcom.com/gedcom.html">Geditcom</see>
    /// </summary>
    public class GedcomTortureTests
    {
        [Fact(Skip = "Torture tests skipped for now")]
        public void CanParseTgc551()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomTorture.TGC551.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.Warnings.ShouldBeEmptyWithFeedback();
        }

        [Fact(Skip = "Torture tests skipped for now")]
        public void CanParseTgc551Lf()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomTorture.TGC551LF.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.Warnings.ShouldBeEmptyWithFeedback();
        }

        [Fact(Skip = "Torture tests skipped for now")]
        public void CanParseTgc55C()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomTorture.TGC55C.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.Warnings.ShouldBeEmptyWithFeedback();
        }

        [Fact(Skip = "Torture tests skipped for now")]
        public void CanParseTgc55Clf()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("GedcomTorture.TGC55CLF.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.Warnings.ShouldBeEmptyWithFeedback();
        }
    }
}
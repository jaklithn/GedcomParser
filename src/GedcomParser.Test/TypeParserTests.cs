using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApprovalTests;
using ApprovalTests.Reporters;
using GedcomParser.Services;
using GedcomParser.Test.Services;
using Shouldly;
using Xunit;


namespace GedcomParser.Test
{
    [UseReporter(typeof(DiffReporter))]
    public class TypeParserTests
    {
        [Fact]
        public void CanParseKennedyFamily()
        {
            // Arrange
            const string fileName = "CustomSample.Kennedy.ged";
            var lines = ResourceHelper.GetLines(fileName);

            // Act
            var result = TypeParser.ParseTypeStructure(lines);

            // Assert
            result.Keys.Count.ShouldBe(36);
            result.Values.Sum(v => v.Count).ShouldBe(45);
            var output = GetAllTypes(fileName, result);
            Approvals.Verify(output);
        }

        [Fact]
        public void CanParseWindsorFamily()
        {
            // Arrange
            const string fileName = "CustomSample.Windsor.ged";
            var lines = ResourceHelper.GetLines(fileName);

            // Act
            var result = TypeParser.ParseTypeStructure(lines);

            // Assert
            result.Keys.Count.ShouldBe(17);
            result.Values.Sum(v => v.Count).ShouldBe(17);
            var output = GetAllTypes(fileName, result);
            Approvals.Verify(output);
        }

        [Fact]
        public void CanParseStefanFamily()
        {
            // Arrange
            const string fileName = "CustomSample.Stefan.ged";
            var lines = ResourceHelper.GetLines(fileName);

            // Act
            var result = TypeParser.ParseTypeStructure(lines);

            // Assert
            result.Keys.Count.ShouldBe(78);
            result.Values.Sum(v => v.Count).ShouldBe(176);
            var output = GetAllTypes(fileName, result);
            Approvals.Verify(output);
        }

        [Fact]
        public void CanParseGedcomStandardSample()
        {
            // Arrange
            const string fileName = "GedcomStandard.555SAMPLE.GED";
            var lines = ResourceHelper.GetLines(fileName);

            // Act
            var result = TypeParser.ParseTypeStructure(lines);

            // Assert
            result.Keys.Count.ShouldBe(48);
            result.Values.Sum(v => v.Count).ShouldBe(62);
            var output = GetAllTypes(fileName, result);
            Approvals.Verify(output);
        }

        [Fact]
        public void CanParseGedcomTortureSample()
        {
            // Arrange
            const string fileName = "GedcomTorture.TGC551.ged";
            var lines = ResourceHelper.GetLines(fileName);

            // Act
            var result = TypeParser.ParseTypeStructure(lines);

            // Assert
            result.Keys.Count.ShouldBe(129);
            result.Values.Sum(v => v.Count).ShouldBe(489);
            var output = GetAllTypes(fileName, result);
            Approvals.Verify(output);
        }

        private static string GetAllTypes(string fileName, IDictionary<string, List<string>> types)
        {
            var sb = new StringBuilder();
            sb.AppendLine("======================================================================");
            sb.AppendLine($"  ALL types detected in the file : '{fileName}'");
            sb.AppendLine("======================================================================");

            foreach (var type in types.Keys)
            {
                var subTypes = types[type];
                sb.AppendLine(type);
                foreach (var subType in subTypes)
                {
                    var indicator = types[subType].Any() ? "+" : "";
                    sb.AppendLine($"    {subType}{indicator}");
                }
            }

            return sb.ToString();
        }

    }
}
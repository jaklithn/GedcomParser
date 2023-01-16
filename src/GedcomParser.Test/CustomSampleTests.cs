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
            result.Errors.Count.ShouldBe(2);
            result.Errors.ShouldContain("Failed to handle top level Type='_GRP'");
            result.Errors.ShouldContain("Failed to handle top level Type='_PLC'");
            result.Warnings.Count.ShouldBe(17);
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
            result.Warnings.Count.ShouldBe(1);
            result.Warnings.ShouldContain("Skipped Person Type='OBJE'");
        }

        [Fact]
        public void CanParseMultipleImmigrationEventsFamily()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleEvents.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            var travis = result.Persons.Single(p => p.FirstName == "Travis");
            var niles = result.Persons.Single(p => p.FirstName == "Niles");
            travis.Immigrated.Count.ShouldBe(2);
            niles.Immigrated.ShouldBeEmpty();

        }

        [Fact]
        public void CanParseMultipleEmigrationEventsFamily()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleEvents.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            var travis = result.Persons.Single(p => p.FirstName == "Travis");
            var niles = result.Persons.Single(p => p.FirstName == "Niles");
            travis.Emigrated.Count.ShouldBe(2);
            niles.Emigrated.ShouldBeEmpty();
        }

        [Fact]
        public void CanParseMultipleResidenceEventsFamily()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleEvents.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            var travis = result.Persons.Single(p => p.FirstName == "Travis");
            var niles = result.Persons.Single(p => p.FirstName == "Niles");
            travis.Residence.Count.ShouldBe(3);
            niles.Residence.ShouldBeEmpty();
        }

        [Fact]
        public void CanParseMultipleMigrationEvents()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleEvents.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            var travis = result.Persons.Single(p => p.FirstName == "Travis");
            var travisArrivals = travis.Events["arrival"];
            var travisDepartures = travis.Events["departure"];
            travisArrivals.Count.ShouldBe(3);
            travisDepartures.Count.ShouldBe(2);

            var travisFirstArrival = travisArrivals.First();
            travisFirstArrival.Date.ShouldBe("20 Aug 2000");
            travisFirstArrival.Place.ShouldBe("Nevada City, Merced, California, USA");

            var travisFirstDeparture = travis.Events["departure"].First();
            travisFirstDeparture.Date.ShouldBe("12 Jul 2005");
            travisFirstDeparture.Place.ShouldBe("California Hot Springs, Tulare, California, USA");

            var niles = result.Persons.Single(p => p.FirstName == "Niles");
            niles.Events.ShouldBeEmpty();
        }

        [Fact]
        public void CanParseMultipleNaturalizationEventsFamily()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleEvents.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            var travis = result.Persons.Single(p => p.FirstName == "Travis");
            var niles = result.Persons.Single(p => p.FirstName == "Niles");
            travis.BecomingCitizen.Count.ShouldBe(2);
            niles.BecomingCitizen.Count.ShouldBe(2);
        }

        [Fact]
        public void CanParseMultipleCensusEventsFamily()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleEvents.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            var travis = result.Persons.Single(p => p.FirstName == "Travis");
            var niles = result.Persons.Single(p => p.FirstName == "Niles");
            travis.Census.Count.ShouldBe(2);
            niles.Census.ShouldBeEmpty();
        }

        [Fact]
        public void CanParseMultipleDestinationEventsFamily()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleEvents.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            var travis = result.Persons.Single(p => p.FirstName == "Travis");
            var niles = result.Persons.Single(p => p.FirstName == "Niles");
            travis.Destination.Count.ShouldBe(2);
            niles.Destination.Count.ShouldBe(2);
            niles.Census.ShouldBeEmpty();
        }

        [Fact]
        public void CanParseDescriptionOfEvents()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleEvents.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            var firstPersonInTree = result.Persons.FirstOrDefault(p => p.Id.Trim().Equals("@I272301164062@"));

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();

            firstPersonInTree.Immigrated.Count.ShouldBe(2);
            firstPersonInTree.Immigrated[0].Description.ShouldBe("Shifted to USA");
            firstPersonInTree.Immigrated[1].Description.ShouldBe("Shifted to England");

            firstPersonInTree.Emigrated.Count.ShouldBe(2);
            firstPersonInTree.Emigrated[0].Description.ShouldBe("Emigrated from Leeds");
            firstPersonInTree.Emigrated[1].Description.ShouldBe("Emigrated from Utah");

            firstPersonInTree.Residence.Count.ShouldBe(3);
            firstPersonInTree.Residence[0].Description.ShouldBe("Resident of Nevada");
            firstPersonInTree.Residence[1].Description.ShouldBe("Resident of Utah");
            firstPersonInTree.Residence[2].Description.ShouldBe("Resident of Leeds");

            firstPersonInTree.BecomingCitizen.Count.ShouldBe(2);
            firstPersonInTree.BecomingCitizen[0].Description.ShouldBe("Citizen of Manchester");
            firstPersonInTree.BecomingCitizen[1].Description.ShouldBe("Citizen of Leeds");

            var arrivalEvents = firstPersonInTree.Events["arrival"];
            arrivalEvents.Count.ShouldBe(3);
            arrivalEvents[0].Description.ShouldBe("Arrival to Nevada");
            arrivalEvents[1].Description.ShouldBe("Arrival to New York");
            arrivalEvents[2].Description.ShouldBe("Arrival to Italy");

            var departureEvents = firstPersonInTree.Events["departure"];
            departureEvents.Count.ShouldBe(2);
            departureEvents[0].Description.ShouldBe("Departure to California");
            departureEvents[1].Description.ShouldBe("Departure from Utah");

            firstPersonInTree.Destination.Count.ShouldBe(2);
            firstPersonInTree.Destination[0].Description.ShouldBe("California Mesa, Delta");
            firstPersonInTree.Destination[1].Description.ShouldBe("Washington County, Alabama");
        }

        [Fact]
        public void CanParseDescriptionOfSpousalEvents()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleSpousalEventsOfSameType.ged");

            // Act
            var result = FileParser.ParseLines(lines);
            var firstPersonInTree = result.SpouseRelations.FirstOrDefault(p => p.From.Id.Trim().Equals("@I252326392761@"));

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();

            firstPersonInTree.Engagement.Count.ShouldBe(2);
            firstPersonInTree.Engagement[0].Description.ShouldBe("Engagement event");
            firstPersonInTree.Engagement[1].Description.ShouldBe("This is a second engagement event");

            firstPersonInTree.Marriage.Count.ShouldBe(2);
            firstPersonInTree.Marriage[0].Description.ShouldBe("This is a marriage event");
            firstPersonInTree.Marriage[1].Description.ShouldBe("This is a second marriage event.");

            firstPersonInTree.Divorce.Count.ShouldBe(1);
            firstPersonInTree.Divorce[0].Description.ShouldBe("This is divorce event");
        }

        [Fact]
        public void CanParseMarriageContractEvents()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleEvents.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.SpouseRelations.Count.ShouldBe(2);
            var travisRelation = result.SpouseRelations.Single(r => r.From.FirstName == "Travis");
            var nilesRelation = result.SpouseRelations.Single(r => r.From.FirstName == "Niles");
            travisRelation.MarriageContract.ShouldNotBeNull();
            nilesRelation.MarriageContract.ShouldNotBeNull();
        }

        [Fact]
        public void CanParseMarriageBannEvents()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleEvents.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.SpouseRelations.Count.ShouldBe(2);
            var travisRelation = result.SpouseRelations.Single(r => r.From.FirstName == "Travis");
            var nilesRelation = result.SpouseRelations.Single(r => r.From.FirstName == "Niles");
            travisRelation.MarriageBann.ShouldNotBeNull();
            nilesRelation.MarriageBann.ShouldNotBeNull();
        }

        [Fact]
        public void CanParseMarriageLicenseEvents()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleEvents.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.SpouseRelations.Count.ShouldBe(2);
            var travisRelation = result.SpouseRelations.Single(r => r.From.FirstName == "Travis");
            var nilesRelation = result.SpouseRelations.Single(r => r.From.FirstName == "Niles");
            travisRelation.MarriageLicense.ShouldNotBeNull();
            nilesRelation.MarriageLicense.ShouldNotBeNull();
        }

        [Fact]
        public void CanParseMarriageSettlementEvents()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleEvents.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.SpouseRelations.Count.ShouldBe(2);
            var travisRelation = result.SpouseRelations.Single(r => r.From.FirstName == "Travis");
            var nilesRelation = result.SpouseRelations.Single(r => r.From.FirstName == "Niles");
            travisRelation.MarriageSettlement.ShouldNotBeNull();
            nilesRelation.MarriageSettlement.ShouldNotBeNull();
        }

        [Fact]
        public void CanParseMultipleSpousalEventsOfSameType()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleSpousalEventsOfSameType.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.SpouseRelations.Count.ShouldBe(2);
            var firstRelation = result.SpouseRelations.First();
            firstRelation.From.FirstName.ShouldBe("Charlie");
            firstRelation.To.FirstName.ShouldBe("Lily");
            firstRelation.Engagement.Count.ShouldBe(2);
            firstRelation.Marriage.Count.ShouldBe(2);
        }

        [Fact]
        public void CanParseSeparationEvents()
        {
            // Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleEvents.ged");

            // Act
            var result = FileParser.ParseLines(lines);

            // Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.SpouseRelations.Count.ShouldBe(2);
            result.SpouseRelations[0].From.FirstName.ShouldBe("Travis");
            result.SpouseRelations[1].Separation.ShouldNotBeNull();
        }
    }
}
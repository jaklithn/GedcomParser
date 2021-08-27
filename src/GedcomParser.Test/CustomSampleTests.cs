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
            // result.Warnings.ShouldBeEmptyWithFeedback();
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
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
            Assert.Collection(result.Persons, person => { Assert.Equal("Travis", person.FirstName.Trim()); Assert.Equal(2, person.Immigrated.Count); },
                                              person => { Assert.Equal("Niles", person.FirstName.Trim()); Assert.Empty(person.Immigrated); });            
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
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
            Assert.Collection(result.Persons, person => { Assert.Equal("Travis", person.FirstName.Trim()); Assert.Equal(2, person.Emigrated.Count); },
                                              person => { Assert.Equal("Niles", person.FirstName.Trim()); Assert.Empty(person.Emigrated); });
        }

        [Fact]
        public void CanParseMultipleResidenceEventsFamily()
        {
            //Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleEvents.ged");

            //Act
            var result = FileParser.ParseLines(lines);

            //Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
            Assert.Collection(result.Persons, person => { Assert.Equal("Travis", person.FirstName.Trim()); Assert.Equal(3, person.Residence.Count); },
                person => { Assert.Equal("Niles", person.FirstName.Trim()); Assert.Empty(person.Residence); });

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
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
            Assert.Collection(result.Persons, person => { Assert.Equal("Travis", person.FirstName.Trim()); Assert.Equal(3, person.Events["arrival"].Count);
                                                          Assert.Equal(2, person.Events["departure"].Count); },
                                              person => { Assert.Equal("Niles", person.FirstName.Trim()); Assert.Empty(person.Events); });
            Assert.Collection(result.Persons, person => { Assert.Equal("20 Aug 2000", person.Events["arrival"].FirstOrDefault().Date);
                                                          Assert.Equal("Nevada City, Merced, California, USA", person.Events["arrival"].FirstOrDefault().Place);
                                                          Assert.Equal("12 Jul 2005", person.Events["departure"].FirstOrDefault().Date);
                                                          Assert.Equal("California Hot Springs, Tulare, California, USA", person.Events["departure"].FirstOrDefault().Place);
                                                        },
                                              person => { Assert.Empty(person.Events); });
        }

        [Fact]
        public void CanParseMultipleNaturalizationEventsFamily()
        {
            //Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleEvents.ged");

            //Act
            var result = FileParser.ParseLines(lines);

            //Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
            Assert.Collection(result.Persons, person => { Assert.Equal("Travis", person.FirstName.Trim()); Assert.Equal(2, person.BecomingCitizen.Count); },
                person => { Assert.Equal("Niles", person.FirstName.Trim()); Assert.Equal(2, person.BecomingCitizen.Count); });
        }

        [Fact]
        public void CanParseMultipleCensusEventsFamily()
        {
            //Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleEvents.ged");

            //Act
            var result = FileParser.ParseLines(lines);

            //Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");
            Assert.Collection(result.Persons, person => { Assert.Equal("Travis", person.FirstName.Trim()); Assert.Equal(2, person.Census.Count); },
                person => { Assert.Equal("Niles", person.FirstName.Trim()); Assert.Empty(person.Census); });

        }

        [Fact]
        public void CanParseDescriptionOfEvents()
        {
            //Arrange
            var lines = ResourceHelper.GetLines("CustomSample.MultipleEvents.ged");

            //Act
            var result = FileParser.ParseLines(lines);

            var firstPersonInTree = result.Persons.FirstOrDefault(p => p.Id.Trim().Equals("@I272301164062@"));
            
            var immigrationEvents       = firstPersonInTree.Immigrated;
            var emigrationEvents        = firstPersonInTree.Emigrated;
            var residenceEvents         = firstPersonInTree.Residence;
            var naturalizationEvents    = firstPersonInTree.BecomingCitizen;
            var migrationEvents         = firstPersonInTree.Events;

            //Assert
            result.Errors.ShouldBeEmptyWithFeedback();
            result.Warnings.ShouldContain("Skipped Person Type='FAMS'");

            Assert.Equal(2, immigrationEvents.Count);
            Assert.Equal(2, emigrationEvents.Count);
            Assert.Equal(3, residenceEvents.Count);
            Assert.Equal(2, naturalizationEvents.Count);
            Assert.Equal(3, migrationEvents["arrival"].Count);
            Assert.Equal(2, migrationEvents["departure"].Count);

            Assert.Collection(immigrationEvents, evt => { Assert.Equal("Shifted to USA", evt.Description); },
                                                 evt => { Assert.Equal("Shifted to England", evt.Description); });

            Assert.Collection(emigrationEvents, evt => { Assert.Equal("Emigrated from Leeds", evt.Description); },
                                               evt => { Assert.Equal("Emigrated from Utah", evt.Description); });

            Assert.Collection(residenceEvents, evt => { Assert.Equal("Resident of Nevada", evt.Description); },
                                               evt => { Assert.Equal("Resident of Utah", evt.Description); },
                                               evt => { Assert.Equal("Resident of Leeds", evt.Description); });

            Assert.Collection(naturalizationEvents, evt => { Assert.Equal("Citizen of Manchester", evt.Description); },
                                                    evt => { Assert.Equal("Citizen of Leeds", evt.Description); });

            Assert.Collection(migrationEvents["arrival"], evt => { Assert.Equal("Arrival to Nevada", evt.Description); },
                                                          evt => { Assert.Equal("Arrival to New York", evt.Description); },
                                                          evt => { Assert.Equal("Arrival to Italy", evt.Description); });

            Assert.Collection(migrationEvents["departure"], evt => { Assert.Equal("Departure to California", evt.Description); },
                                                            evt => { Assert.Equal("Departure from Utah", evt.Description); });

        }
    }
}
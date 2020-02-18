namespace Dfe.Spi.Common.Extensions.UnitTests
{
    using Dfe.Spi.Common.Extensions;
    using Dfe.Spi.Models;
    using Dfe.Spi.Models.Entities;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ObjectExtensionsTests
    {
        [Test]
        public void SomethingSomething()
        {
            // Arrange
            LearningProvider learningProvider = new LearningProvider()
            {
                Name = "SomeCorp Inc. Acadamies, Ltd",
                Address = new Address()
                {
                    AddressLine1 = "SomeCorp House",
                    AddressLine2 = "123 Example Way",
                    County = "Leicestershire",
                    Town = "Enderby",
                },
                DfeNumber = "ABC/DEF",
                Ukprn = 123,
                Uprn = "4567890",
                Urn = 7262827,
                OpenDate = new DateTime(2010, 8, 2),
            };

            string[] propertiesToInclude = new string[]
            {
                null,
                nameof(LearningProvider.Address),
                nameof(LearningProvider.Urn),
                nameof(LearningProvider.OpenDate),
                string.Empty,
                "a field that does not exist",
                nameof(LearningProvider.Name),
            };

            LearningProvider pruned = null;

            // Act
            pruned = learningProvider.PruneModel(propertiesToInclude);

            // Assert
            Assert.IsNotNull(pruned.Name);
            Assert.IsNotNull(pruned.Address);
            Assert.IsNull(pruned.DfeNumber);
            Assert.IsNull(pruned.Ukprn);
            Assert.IsNull(pruned.Uprn);
            Assert.IsNotNull(pruned.Urn);
            Assert.IsNotNull(pruned.OpenDate);
        }
    }
}
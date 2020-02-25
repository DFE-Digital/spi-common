using System;
using AutoFixture.NUnit3;
using Dfe.Spi.Common.UnitTesting.Fixtures;
using Dfe.Spi.Models.Entities;
using Dfe.Spi.Models.Extensions;
using NUnit.Framework;

namespace Dfe.Spi.Models.Extensions.UnitTests
{
    public class WhenSettingLineageForRequestedFieldsForEntity
    {
        [Test]
        public void ThenItShouldIncludeLineageForAllPropertiesThatAreNotNull()
        {
            var entity = new UnitTestEntity
            {
                StringField = "hello",
                IntField = 1,
                LongField = 99,
            };

            entity.SetLineageForRequestedFields();
            
            Assert.IsNotNull(entity._Lineage);
            Assert.AreEqual(3, entity._Lineage.Count);
            Assert.IsTrue(entity._Lineage.ContainsKey("StringField"));
            Assert.IsTrue(entity._Lineage.ContainsKey("IntField"));
            Assert.IsTrue(entity._Lineage.ContainsKey("LongField"));
        }
        
        [Test, NonRecursiveAutoData]
        public void ThenItSetReadDateOfLineageEntriesToSpecifiedDate(UnitTestEntity entity, DateTime readDate)
        {
            entity.SetLineageForRequestedFields(readDate);

            foreach (var key in entity._Lineage.Keys)
            {
                var actual = entity._Lineage[key];
                Assert.AreEqual(readDate, actual.ReadDate,
                    $"Expected ReadDate of {readDate} for {key} but received {actual.ReadDate}");
            }
        }
        
        [Test, NonRecursiveAutoData]
        public void ThenItSetReadDateOfLineageEntriesToUtcNowIfNoDateSpecified(UnitTestEntity entity)
        {
            entity.SetLineageForRequestedFields();

            var expected = DateTime.UtcNow;
            foreach (var key in entity._Lineage.Keys)
            {
                var actual = entity._Lineage[key];
                Assert.AreEqual(expected.Ticks, actual.ReadDate.Value.Ticks, 1000,
                    $"Expected ReadDate of {expected} +/- 1000 ticks for {key} but received {actual.ReadDate}");
            }
        }
        
        [Test]
        public void ThenItShouldNotIncludeLineageForPropertiesOfTheyEntityBase()
        {
            var entity = new UnitTestEntity
            {
                StringField = "hello",
                IntField = 1,
                LongField = 99,
                SubEntity = new UnitTestSubEntity(),
            };
            
            entity.SetLineageForRequestedFields();

            Assert.IsFalse(entity._Lineage.ContainsKey("SubEntity"));
        }
    }

    public class UnitTestEntity : EntityBase
    {
        public string StringField { get; set; }
        public int IntField { get; set; }
        public int LongField { get; set; }
        public UnitTestSubEntity SubEntity { get; set; }
    }

    public class UnitTestSubEntity : EntityBase
    {
        
    }
}
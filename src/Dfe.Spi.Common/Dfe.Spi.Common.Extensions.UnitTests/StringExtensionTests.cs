namespace Dfe.Spi.Common.Extensions.UnitTests
{
    using System;
    using NUnit.Framework;
    
    public class StringExtensionTests
    {
        [TestCase("2020-03-11", 2020, 3, 11, 0, 0, 0)]
        [TestCase("2020/03/11", 2020, 3, 11, 0, 0, 0)]
        [TestCase("2020.03.11", 2020, 3, 11, 0, 0, 0)]
        [TestCase("2020-03-11 10:23:38", 2020, 3, 11, 10, 23, 38)]
        [TestCase("2020/03/11 10:23:38", 2020, 3, 11, 10, 23, 38)]
        [TestCase("2020.03.11 10:23:38", 2020, 3, 11, 10, 23, 38)]
        [TestCase("11-03-2020", 2020, 3, 11, 0, 0, 0)]
        [TestCase("11/03/2020", 2020, 3, 11, 0, 0, 0)]
        [TestCase("11.03.2020", 2020, 3, 11, 0, 0, 0)]
        [TestCase("11-03-2020 10:23:38", 2020, 3, 11, 10, 23, 38)]
        [TestCase("11/03/2020 10:23:38", 2020, 3, 11, 10, 23, 38)]
        [TestCase("11.03.2020 10:23:38", 2020, 3, 11, 10, 23, 38)]
        [TestCase("2020-03-11T10:23:38Z", 2020, 3, 11, 10, 23, 38)]
        [TestCase("2020-03-11T10:23:38+00:00", 2020, 3, 11, 10, 23, 38)]
        [TestCase("2020-03-11T11:23:38+01:00", 2020, 3, 11, 10, 23, 38)]
        public void ValidFormatStringsShouldBeConvertedToDates(string value, int year, int month, int day, int hour, int minute, int second)
        {
            var actual = value.ToDateTime();
            
            Assert.AreEqual(year, actual.Year);
            Assert.AreEqual(month, actual.Month);
            Assert.AreEqual(day, actual.Day);
            Assert.AreEqual(hour, actual.Hour);
            Assert.AreEqual(minute, actual.Minute);
            Assert.AreEqual(second, actual.Second);
        }

        [TestCase("not-a-date")]
        [TestCase("2020-20-03")]
        public void InvalidFormatStringsShouldThrowInvalidDateTimeFormatException(string value)
        {
            Assert.Throws<InvalidDateTimeFormatException>(() => value.ToDateTime());
        }
    }
}
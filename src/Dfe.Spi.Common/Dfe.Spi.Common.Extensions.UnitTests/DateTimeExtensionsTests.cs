namespace Dfe.Spi.Common.Extensions.UnitTests
{
    using System;
    using NUnit.Framework;
    
    public class DateTimeExtensionsTests
    {
        [TestCaseSource(nameof(DateTimeToStringTestCases))]
        public void DateTimesShouldBeConvertedToSpiStrings(DateTime dateTime, string expected)
        {
            var actual = dateTime.ToSpiString();
            
            Assert.AreEqual(expected, actual);
        }

        public static object[] DateTimeToStringTestCases = new[]
        {
            new object[] {DateTime.SpecifyKind(new DateTime(2020, 3, 11, 10, 25, 28), DateTimeKind.Utc), "2020-03-11T10:25:28.0000000Z"}
        };
    }
}
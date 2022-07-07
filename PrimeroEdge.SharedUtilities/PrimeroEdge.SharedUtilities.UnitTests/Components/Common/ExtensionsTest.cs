namespace PrimeroEdge.SharedUtilities.UnitTests.Components.Common
{
    using System;
    using NUnit.Framework;
    using PrimeroEdge.SharedUtilities.Components.Common;

    public class ExtensionsTest
    {
        [Test]
        public void ToUtcDateTime_ShouldConvertToUtc_FromDate()
        {
            var dtDateTime = new DateTime(2022, 07, 05, 12, 0, 0);
            var fromDateTime = dtDateTime.ToUtcDateTime();
            Assert.IsNotNull(fromDateTime);
            Assert.IsNotEmpty(fromDateTime);
            var expectedFromDateTime = "2022-07-05T05:00:00";
            Assert.AreEqual(expectedFromDateTime, fromDateTime);
        }

        [Test]
        public void ToUtcDateTime_ShouldConvertToUtc_ToDate()
        {
            var dtDateTime = new DateTime(2022, 07, 05, 12, 0, 0);
            var toDateTime = dtDateTime.ToUtcDateTime(true);
            Assert.IsNotNull(toDateTime);
            Assert.IsNotEmpty(toDateTime);
            var expectedToDateTime = "2022-07-06T04:59:59";
            Assert.AreEqual(expectedToDateTime, toDateTime);
        }
    }
}
namespace Dfe.Spi.Common.Http.Server.UnitTests
{
    using Dfe.Spi.Common.Context.Models;
    using Dfe.Spi.Common.Http.Server.Definitions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using Microsoft.Net.Http.Headers;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class HttpSpiExecutionContextManagerTests
    {
        private HttpSpiExecutionContextManager httpSpiExecutionContextManager;

        [SetUp]
        public void Arrange()
        {
            this.httpSpiExecutionContextManager =
                new HttpSpiExecutionContextManager();
        }

        [Test]
        public void SetContext_HeaderValuesSetInContext_ValuesAsExpected()
        {
            // Arrange
            Guid? expectedInternalRequestId = new Guid("36b3d65b-02af-407d-acd6-d47587f938fe");
            Guid? actualInternalRequestId = null;

            string expectedExternalRequestId = "ed0509b1-16ea-4e1e-88c0-770be6b17081";
            string actualExternalRequestId = null;

            string expectedIdentityToken = "abcOneTwo3";
            string actualIdentityToken = null;

            Dictionary<string, StringValues> store =
                new Dictionary<string, StringValues>()
                {
                    {
                        SpiHeaderNames.InternalRequestIdHeaderName,
                        new StringValues(expectedInternalRequestId.ToString())
                    },
                    {
                        SpiHeaderNames.ExternalRequestIdHeaderName,
                        new StringValues(expectedExternalRequestId)
                    },
                    {
                        HeaderNames.Authorization,
                        new StringValues($"BeaRER {expectedIdentityToken}")
                    }
                };

            HeaderDictionary headerDictionary = new HeaderDictionary(
                store);

            SpiExecutionContext spiExecutionContext = null;

            // Act
            this.httpSpiExecutionContextManager.SetContext(headerDictionary);

            spiExecutionContext =
                this.httpSpiExecutionContextManager.SpiExecutionContext;

            // Assert
            actualInternalRequestId = spiExecutionContext.InternalRequestId;

            Assert.AreEqual(expectedInternalRequestId, actualInternalRequestId);

            actualExternalRequestId = spiExecutionContext.ExternalRequestId;

            Assert.AreEqual(actualExternalRequestId, expectedExternalRequestId);

            actualIdentityToken = spiExecutionContext.IdentityToken;

            Assert.AreEqual(expectedIdentityToken, actualIdentityToken);
        }
    }
}
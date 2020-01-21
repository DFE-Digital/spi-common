namespace Dfe.Spi.Common.Http.Server.UnitTests
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using NJsonSchema;
    using NUnit.Framework;

    [TestFixture]
    public class TypeExtensionsTests
    {
        [Test]
        public void GetFunctionJsonSchemaAsync_NoSchemaExists_ThrowsFileNotFoundException()
        {
            // Arrange
            Type type = typeof(SomeMethodWithoutSchema);
            AsyncTestDelegate asyncTestDelegate = null;

            asyncTestDelegate =
                async () =>
                {
                    // Act
                    await type.GetFunctionJsonSchemaAsync();
                };

            // Assert
            Assert.ThrowsAsync<FileNotFoundException>(asyncTestDelegate);
        }

        [Test]
        public void GetFunctionJsonSchemaAsync_CorruptSchemaResourceExists_ThrowsSchemaParsingException()
        {
            // Arrange
            Type type = typeof(SomeCorruptSchemaMethod);
            AsyncTestDelegate asyncTestDelegate = null;

            asyncTestDelegate =
                async () =>
                {
                    // Act
                    await type.GetFunctionJsonSchemaAsync();
                };

            // Assert
            Assert.ThrowsAsync<SchemaParsingException>(asyncTestDelegate);
        }

        [Test]
        public async Task GetFunctionJsonSchemaAsync_WellFormedSchemaResourceExists_ReturnsJsonSchema()
        {
            // Arrange
            Type type = typeof(SomeExampleMethod);
            JsonSchema jsonSchema = null;

            // Act
            jsonSchema = await type.GetFunctionJsonSchemaAsync();

            // Assert
            Assert.IsNotNull(jsonSchema);
        }

        public class SomeExampleMethod
        {
            // Nothing - just declaring this type, so it can be used in a test.
        }

        public class SomeCorruptSchemaMethod
        {
            // Nothing - just declaring this type, so it can be used in a test.
        }

        public class SomeMethodWithoutSchema
        {
            // Nothing - just declaring this type, so it can be used in a test.
        }
    }
}
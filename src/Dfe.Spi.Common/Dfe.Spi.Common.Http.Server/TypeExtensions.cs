namespace Dfe.Spi.Common.Http.Server
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Dfe.Spi.Common.Extensions;
    using Newtonsoft.Json;
    using NJsonSchema;

    /// <summary>
    /// Contains extension methods for the <see cref="Type" /> class.
    /// </summary>
    public static class TypeExtensions
    {
        private const string SchemaFilenameFormat = "{0}-body.json";

        /// <summary>
        /// Loads, from the <paramref name="type" />'s <see cref="Assembly" />,
        /// a <see cref="JsonSchema" />, using the embedded resources present
        /// in the <paramref name="type" />'s <see cref="Assembly" />.
        /// </summary>
        /// <param name="type">
        /// An instance of <see cref="Type" />.
        /// </param>
        /// <returns>
        /// A populated <see cref="JsonSchema" />.
        /// </returns>
        public static async Task<JsonSchema> GetFunctionJsonSchemaAsync(
            this Type type)
        {
            JsonSchema toReturn = null;

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            string name = type.Name;

            // Convert ThisTypeName to this-type-name.
            name = name.PascalToKebabCase();
            name = string.Format(
                CultureInfo.InvariantCulture,
                SchemaFilenameFormat,
                name);

            Assembly assembly = type.Assembly;

            string[] embeddedResources =
                assembly.GetManifestResourceNames();

            string fullPath = embeddedResources
                .SingleOrDefault(x => x.EndsWith(
                    name,
                    StringComparison.InvariantCulture));

            if (string.IsNullOrEmpty(fullPath))
            {
                throw new FileNotFoundException(
                    $"Could not find JSON schema as an embedded resource " +
                    $"with name \"{name}\". Ensure that the file exists and " +
                    $"that it's build action is set to \"Embedded\".");
            }

            string dataStr = null;
            using (Stream stream = assembly.GetManifestResourceStream(fullPath))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    dataStr = await streamReader.ReadToEndAsync()
                        .ConfigureAwait(false);
                }
            }

            try
            {
                // Then load it.
                toReturn = await JsonSchema.FromJsonAsync(dataStr)
                    .ConfigureAwait(false);
            }
            catch (JsonReaderException jsonReaderException)
            {
                throw new SchemaParsingException(
                    fullPath,
                    jsonReaderException);
            }

            return toReturn;
        }
    }
}
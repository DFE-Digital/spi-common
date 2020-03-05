namespace Dfe.Spi.Common.Models
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Represents a generic filter for data operations.
    /// </summary>
    public class DataFilter : ModelsBase
    {
        /// <summary>
        /// Gets or sets the field name.
        /// </summary>
        public string Field
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="DataOperator" />.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public DataOperator Operator
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the field value, as a <see cref="string" /> value.
        /// </summary>
        public string Value
        {
            get;
            set;
        }
    }
}
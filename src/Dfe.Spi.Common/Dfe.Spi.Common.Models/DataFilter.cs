namespace Dfe.Spi.Common.Models
{
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
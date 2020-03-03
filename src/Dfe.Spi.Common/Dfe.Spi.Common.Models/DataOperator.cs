namespace Dfe.Spi.Common.Models
{
    /// <summary>
    /// An enumeration of data operations.
    /// </summary>
    public enum DataOperator
    {
        /// <summary>
        /// An equals operation.
        /// </summary>
        Equals,

        /// <summary>
        /// A contains operation.
        /// </summary>
        Contains,

        /// <summary>
        /// A greater-than operation.
        /// </summary>
        GreaterThan,

        /// <summary>
        /// A greater-than or equals to operation.
        /// </summary>
        GreaterThanOrEqualTo,

        /// <summary>
        /// A less-than operation.
        /// </summary>
        LessThan,

        /// <summary>
        /// A less-than or equals to operation.
        /// </summary>
        LessThanOrEqualTo,

        /// <summary>
        /// An "is-in" operation.
        /// </summary>
        In,

        /// <summary>
        /// An is-null operation.
        /// </summary>
        IsNull,

        /// <summary>
        /// An is-not-null operation.
        /// </summary>
        IsNotNull,

        /// <summary>
        /// A "between" operation.
        /// </summary>
        Between,
    }
}
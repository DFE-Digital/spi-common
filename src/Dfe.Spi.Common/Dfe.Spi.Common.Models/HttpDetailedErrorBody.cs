namespace Dfe.Spi.Common.Models
{
    /// <summary>
    /// Common error body model, including array of error details
    /// </summary>
    public class HttpDetailedErrorBody : HttpErrorBody
    {
        /// <summary>
        /// Array of error details
        /// </summary>
        public string[] Details { get; set; }
    }
}
namespace Dfe.Spi.Common.Http.Server.Definitions
{
    using System.Net;

    /// <summary>
    /// Describes the operations of the <see cref="HttpErrorBodyResult" />
    /// provider.
    /// </summary>
    public interface IHttpErrorBodyResultProvider
    {
        /// <summary>
        /// Creates an instance of <see cref="HttpErrorBodyResult" />, using
        /// detail stored in the host's resources file.
        /// </summary>
        /// <param name="httpStatusCode">
        /// A <see cref="HttpStatusCode" /> value.
        /// </param>
        /// <param name="errorIdentifierInt">
        /// The id of the error message resource as stored in the host's
        /// resources file, minus the underscore.
        /// </param>
        /// <param name="messageArguments">
        /// Any arguments to populate the error message pulled from the host's
        /// resources file.
        /// </param>
        /// <returns>
        /// An instance of <see cref="HttpErrorBodyResult" />.
        /// </returns>
        HttpErrorBodyResult GetHttpErrorBodyResult(
            HttpStatusCode httpStatusCode,
            int errorIdentifierInt,
            params string[] messageArguments);
    }
}
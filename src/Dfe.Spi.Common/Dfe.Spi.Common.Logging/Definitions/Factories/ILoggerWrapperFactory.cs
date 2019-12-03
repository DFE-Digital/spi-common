namespace Dfe.Spi.Common.Logging.Definitions.Factories
{
    using Dfe.Spi.Common.Models;

    /// <summary>
    /// Describes the operations of the <see cref="ILoggerWrapper" /> factory.
    /// </summary>
    public interface ILoggerWrapperFactory
    {
        /// <summary>
        /// Creates an instance of type <see cref="ILoggerWrapper" />.
        /// </summary>
        /// <param name="requestResponseBase">
        /// An instance of <see cref="RequestResponseBase" />.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </returns>
        ILoggerWrapper Create(RequestResponseBase requestResponseBase);
    }
}
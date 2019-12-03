namespace Dfe.Spi.Common.Logging.Factories
{
    using Dfe.Spi.Common.Logging.Definitions;
    using Dfe.Spi.Common.Logging.Definitions.Factories;
    using Dfe.Spi.Common.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Implements <see cref="ILoggerWrapperFactory" />.
    /// </summary>
    public class LoggerWrapperFactory : ILoggerWrapperFactory
    {
        /// <inheritdoc />
        public ILoggerWrapper Create(
            ILogger logger,
            RequestResponseBase requestResponseBase)
        {
            LoggerWrapper toReturn = new LoggerWrapper(
                logger,
                requestResponseBase);

            return toReturn;
        }
    }
}
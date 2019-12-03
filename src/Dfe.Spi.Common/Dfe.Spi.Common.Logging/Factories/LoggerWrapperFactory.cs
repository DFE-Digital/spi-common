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
        private readonly ILogger logger;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="LoggerWrapperFactory" /> class.
        /// </summary>
        /// <param name="logger">
        /// An instance of type <see cref="ILogger" />.
        /// </param>
        public LoggerWrapperFactory(ILogger logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public ILoggerWrapper Create(RequestResponseBase requestResponseBase)
        {
            LoggerWrapper toReturn = new LoggerWrapper(
                this.logger,
                requestResponseBase);

            return toReturn;
        }
    }
}
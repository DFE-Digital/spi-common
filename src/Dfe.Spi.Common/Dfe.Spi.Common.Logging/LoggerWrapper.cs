﻿namespace Dfe.Spi.Common.Logging
{
    using System;
    using Dfe.Spi.Common.Logging.Definitions;
    using Dfe.Spi.Common.Logging.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Implements <see cref="ILoggerWrapper" />.
    /// </summary>
    public class LoggerWrapper : ILoggerWrapper
    {
        private const string InternalRequestIdHeaderName = "X-Internal-Request-Id";
        private const string ExternalRequestIdHeaderName = "X-External-Request-Id";

        private readonly ILogger logger;

        private RequestContext requestContext;

        /// <summary>
        /// Initialises a new instance of the <see cref="LoggerWrapper" />
        /// class.
        /// </summary>
        /// <param name="logger">
        /// An instance of type <see cref="ILogger" />.
        /// </param>
        public LoggerWrapper(ILogger logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public void SetContext(IHeaderDictionary headerDictionary)
        {
            if (headerDictionary == null)
            {
                throw new ArgumentNullException(nameof(headerDictionary));
            }

            string internalRequestIdStr =
                headerDictionary[InternalRequestIdHeaderName];
            Guid internalRequestId = Guid.Parse(internalRequestIdStr);

            string externalRequestIdStr =
                headerDictionary[ExternalRequestIdHeaderName];

            RequestContext requestContext = new RequestContext()
            {
                InternalRequestId = internalRequestId,
                ExternalRequestId = externalRequestIdStr,
            };

            this.requestContext = requestContext;
        }

        /// <inheritdoc />
        public void Debug(string message)
        {
            this.LogMessage(LogLevel.Debug, message);
        }

        /// <inheritdoc />
        public void Error(string message, Exception exception)
        {
            this.LogMessage(LogLevel.Error, message, exception);
        }

        /// <inheritdoc />
        public void Info(string message)
        {
            this.LogMessage(LogLevel.Information, message);
        }

        /// <inheritdoc />
        public void Warning(string message)
        {
            this.LogMessage(LogLevel.Warning, message);
        }

        /// <inheritdoc />
        public void Warning(string message, Exception exception)
        {
            this.LogMessage(LogLevel.Warning, message, exception);
        }

        private void LogMessage(
            LogLevel logLevel,
            string message,
            Exception exception = null)
        {
            message =
                $"{message} " +
                $"({nameof(RequestContext.InternalRequestId)}: {{{nameof(RequestContext.InternalRequestId)}}}, " +
                $"{nameof(RequestContext.ExternalRequestId)}: {{{nameof(RequestContext.ExternalRequestId)}}})";

            this.logger.Log(
                logLevel,
                exception,
                message,
                this.requestContext.InternalRequestId,
                this.requestContext.ExternalRequestId);
        }
    }
}
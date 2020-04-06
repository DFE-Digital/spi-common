namespace Dfe.Spi.Common.Http.Server
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.Spi.Common.Http.Server.Definitions;
    using Dfe.Spi.Common.Logging.Definitions;
    using Dfe.Spi.Common.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using NJsonSchema;
    using NJsonSchema.Validation;

    /// <summary>
    /// Abstract base class for SPI-related functions, automatically handling
    /// the validation of an embedded schema and returning the error
    /// information.
    /// </summary>
    /// <typeparam name="TRequest">
    /// A request type, deriving from <see cref="RequestResponseBase" />.
    /// </typeparam>
    public abstract class FunctionsBase<TRequest>
        where TRequest : RequestResponseBase
    {
        private readonly IHttpSpiExecutionContextManager httpSpiExecutionContextManager;
        private readonly ILoggerWrapper loggerWrapper;

        private JsonSchema jsonSchema;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="FunctionsBase{TRequest}" /> class.
        /// </summary>
        /// <param name="httpSpiExecutionContextManager">
        /// An instance of type <see cref="IHttpSpiExecutionContextManager" />.
        /// </param>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        protected FunctionsBase(
            IHttpSpiExecutionContextManager httpSpiExecutionContextManager,
            ILoggerWrapper loggerWrapper)
        {
            this.httpSpiExecutionContextManager = httpSpiExecutionContextManager;
            this.loggerWrapper = loggerWrapper;
        }

        /// <summary>
        /// A base implementation of the function's entry point method. This
        /// should be over-ridden and the base implementation called from
        /// within the over-ridden implementation.
        /// </summary>
        /// <param name="httpRequest">
        /// An instance of <see cref="HttpRequest" />.
        /// </param>
        /// <param name="runContext">
        /// An instance of <see cref="FunctionRunContext" />.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="IActionResult" />.
        /// </returns>
        public async Task<IActionResult> ValidateAndRunAsync(
            HttpRequest httpRequest,
            FunctionRunContext runContext,
            CancellationToken cancellationToken)
        {
            IActionResult toReturn = null;

            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            this.httpSpiExecutionContextManager.SetContext(
                httpRequest.Headers);

            TRequest request = null;
            try
            {
                request =
                    await this.ParseAndValidateRequestAsync(
                        httpRequest)
                        .ConfigureAwait(false);
            }
            catch (JsonReaderException jsonReaderException)
            {
                this.loggerWrapper.Info(
                    $"A {nameof(JsonReaderException)} was thrown during the " +
                    $"parsing of the body of the request.",
                    jsonReaderException);

                toReturn = this.GetMalformedErrorResponse(runContext);
            }
            catch (JsonSchemaValidationException jsonSchemaValidationException)
            {
                this.loggerWrapper.Info(
                    $"A {nameof(JsonSchemaValidationException)} was thrown " +
                    $"during the parsing of the body of the request.",
                    jsonSchemaValidationException);

                string message = jsonSchemaValidationException.Message;

                toReturn = this.GetSchemaValidationResponse(message, runContext);
            }

            if (request != null)
            {
                // The JSON is valid and not null, but at this point, it's
                // unknown if its valid according to the *schema*.
                toReturn = await this.ProcessWellFormedRequestAsync(
                    request,
                    runContext,
                    cancellationToken)
                    .ConfigureAwait(false);
            }

            if (toReturn is HttpErrorBodyResult)
            {
                HttpErrorBodyResult httpErrorBodyResult =
                    (HttpErrorBodyResult)toReturn;

                object value = httpErrorBodyResult.Value;

                this.loggerWrapper.Info(
                    $"This HTTP request failed. Returning: {value}.");
            }

            return toReturn;
        }

        /// <summary>
        /// Gets an instance of <see cref="HttpErrorBodyResult" /> to return
        /// when the user provides either an empty or malformed request.
        /// </summary>
        /// <param name="runContext">
        /// An instance of <see cref="FunctionRunContext" />.
        /// </param>
        /// <returns>
        /// An instance of <see cref="HttpErrorBodyResult" />.
        /// </returns>
        protected abstract HttpErrorBodyResult GetMalformedErrorResponse(FunctionRunContext runContext);

        /// <summary>
        /// Gets an instance of <see cref="HttpErrorBodyResult" /> to return
        /// when the user provides a well-formed request, but it does not pass
        /// the schema validation.
        /// </summary>
        /// <param name="message">
        /// Details on why the schema validation failed.
        /// </param>
        /// <param name="runContext">
        /// An instance of <see cref="FunctionRunContext" />.
        /// </param>
        /// <returns>
        /// An instance of <see cref="HttpErrorBodyResult" />.
        /// </returns>
        protected abstract HttpErrorBodyResult GetSchemaValidationResponse(
            string message,
            FunctionRunContext runContext);

        /// <summary>
        /// Executes the function's processor and handles the output, specific
        /// to this function implementation.
        /// </summary>
        /// <param name="request">
        /// An instance of type <typeparamref name="TRequest" />, well-formed
        /// and validated.
        /// </param>
        /// <param name="runContext">
        /// An instance of <see cref="FunctionRunContext" />.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="IActionResult" />.
        /// </returns>
        protected abstract Task<IActionResult> ProcessWellFormedRequestAsync(
            TRequest request,
            FunctionRunContext runContext,
            CancellationToken cancellationToken);

        private async Task<TRequest> ParseAndValidateRequestAsync(
            HttpRequest httpRequest)
        {
            TRequest toReturn = null;

            // Read the body as a string...
            string requestStr = null;
            using (StreamReader streamReader = new StreamReader(httpRequest.Body))
            {
                requestStr = streamReader.ReadToEnd();
            }

            this.loggerWrapper.Debug(
                $"Body of request read, as a string value: \"{requestStr}\".");

            // Validate against the schema.
            JsonSchema jsonSchema = await this.LoadJsonSchema()
                .ConfigureAwait(false);

            this.loggerWrapper.Debug(
                $"Performing validation of body against " +
                $"{nameof(JsonSchema)}...");

            ICollection<ValidationError> validationErrors =
                jsonSchema.Validate(requestStr);

            if (validationErrors.Count > 0)
            {
                throw new JsonSchemaValidationException(validationErrors);
            }

            this.loggerWrapper.Debug(
                $"Deserialising body into a {nameof(TRequest)} instance...");

            toReturn = JsonConvert.DeserializeObject<TRequest>(requestStr);

            return toReturn;
        }

        private async Task<JsonSchema> LoadJsonSchema()
        {
            JsonSchema toReturn = null;

            if (this.jsonSchema == null)
            {
                this.loggerWrapper.Debug(
                    $"The {nameof(JsonSchema)} for this function hasn't " +
                    $"been loaded yet...");

                Type type = this.GetType();

                this.loggerWrapper.Debug($"{nameof(type)} = {type}");

                this.jsonSchema = await type.GetFunctionJsonSchemaAsync()
                    .ConfigureAwait(false);

                this.loggerWrapper.Info($"{nameof(JsonSchema)} loaded.");
            }

            toReturn = this.jsonSchema;

            this.loggerWrapper.Info(
                $"Returning {nameof(JsonSchema)} stored in memory: " +
                $"{toReturn}.");

            return toReturn;
        }
    }
}
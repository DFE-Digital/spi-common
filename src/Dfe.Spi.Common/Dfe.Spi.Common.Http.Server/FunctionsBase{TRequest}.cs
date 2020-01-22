namespace Dfe.Spi.Common.Http.Server
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
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
        private readonly ILoggerWrapper loggerWrapper;

        private JsonSchema jsonSchema;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="FunctionsBase{TRequest}" /> class.
        /// </summary>
        /// <param name="loggerWrapper">
        /// An instance of type <see cref="ILoggerWrapper" />.
        /// </param>
        protected FunctionsBase(
            ILoggerWrapper loggerWrapper)
        {
            this.loggerWrapper = loggerWrapper;
        }

        /// <summary>
        /// Gets an instance of <see cref="HttpErrorBodyResult" /> to return
        /// when the user provides either an empty or malformed request.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="HttpErrorBodyResult" />.
        /// </returns>
        protected abstract HttpErrorBodyResult GetMalformedErrorResponse();

        /// <summary>
        /// Gets an instance of <see cref="HttpErrorBodyResult" /> to return
        /// when the user provides a well-formed request, but it does not pass
        /// the schema validation.
        /// </summary>
        /// <param name="message">
        /// Details on why the schema validation failed.
        /// </param>
        /// <returns>
        /// An instance of <see cref="HttpErrorBodyResult" />.
        /// </returns>
        protected abstract HttpErrorBodyResult GetSchemaValidationResponse(
            string message);

        /// <summary>
        /// Executes the function's processor and handles the output, specific
        /// to this function implementation.
        /// </summary>
        /// <param name="request">
        /// An instance of type <typeparamref name="TRequest" />, well-formed
        /// and validated.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="IActionResult" />.
        /// </returns>
        protected abstract Task<IActionResult> ProcessWellFormedRequestAsync(
            TRequest request);

        /// <summary>
        /// A base implementation of the function's entry point method. This
        /// should be over-ridden and the base implementation called from
        /// within the over-ridden implementation.
        /// </summary>
        /// <param name="httpRequest">
        /// An instance of <see cref="HttpRequest" />.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="IActionResult" />.
        /// </returns>
        protected async virtual Task<IActionResult> RunAsync(
            HttpRequest httpRequest)
        {
            IActionResult toReturn = null;

            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            this.loggerWrapper.SetContext(httpRequest.Headers);

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

                toReturn = this.GetMalformedErrorResponse();
            }
            catch (JsonSchemaValidationException jsonSchemaValidationException)
            {
                this.loggerWrapper.Info(
                    $"A {nameof(JsonSchemaValidationException)} was thrown " +
                    $"during the parsing of the body of the request.",
                    jsonSchemaValidationException);

                string message = jsonSchemaValidationException.Message;

                toReturn = this.GetSchemaValidationResponse(message);
            }

            if (request != null)
            {
                // The JSON is valid and not null, but at this point, it's
                // unknown if its valid according to the *schema*.
                toReturn = await this.ProcessWellFormedRequestAsync(request)
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

                Type type = typeof(TRequest);

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
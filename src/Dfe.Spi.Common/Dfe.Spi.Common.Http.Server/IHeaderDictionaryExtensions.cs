using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Dfe.Spi.Common.Http.Server
{
    public static class IHeaderDictionaryExtensions
    {
        public static string GetHeaderValue(this IHeaderDictionary headers, string headerName)
        {
            var values = GetHeaderValues(headers, headerName);
            return values.FirstOrDefault();
        }
        
        public static StringValues GetHeaderValues(this IHeaderDictionary headers, string headerName)
        {
            var casedHeaderName = headers.Keys.SingleOrDefault(k => k.Equals(headerName, StringComparison.InvariantCultureIgnoreCase));
            if (string.IsNullOrEmpty(casedHeaderName))
            {
                return default;
            }

            return headers[casedHeaderName];
        }
    }
}
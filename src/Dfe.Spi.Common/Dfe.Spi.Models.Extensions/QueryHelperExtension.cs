using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dfe.Spi.Common.Extensions;
using Dfe.Spi.Models.Entities;

namespace Dfe.Spi.Models.Extensions
{
    public static class QueryHelperExtension
    {
        private static readonly Dictionary<Type, PropertyInfo[]> PropertyCache = new Dictionary<Type, PropertyInfo[]>();
        
        public static T Pick<T>(this T source, string fields) where T : EntityBase
        {
            // Then we need to limit the fields we send back...
            var requestedFields = fields
                .Split(',')
                .Select(x => x.ToUpperInvariant())
                .ToArray();

            var pruned = source.PruneModel(requestedFields);

            // If lineage was requested then...
            if (pruned._Lineage != null)
            {
                // ... prune the lineage too.
                pruned._Lineage = pruned
                    ._Lineage
                    .Where(x => requestedFields.Contains(x.Key.ToUpperInvariant()))
                    .ToDictionary(x => x.Key, x => x.Value);
            }

            return pruned;
        }

        public static void SetLineageForRequestedFields<T>(this T source) where T : EntityBase
        {
            SetLineageForRequestedFields(source, DateTime.UtcNow);
        }
        public static void SetLineageForRequestedFields<T>(this T source, DateTime? readDate) where T : EntityBase
        {
            var type = typeof(T);
            var lineage = GetPropertiesOfType(type)
                .Where(x => !x.Name.StartsWith("_") && (x.GetValue(source) != null))
                .ToDictionary(
                    x => x.Name,
                    x => new LineageEntry()
                    {
                        ReadDate = readDate,
                    });
            source._Lineage = lineage;
        }


        private static PropertyInfo[] GetPropertiesOfType(Type type)
        {
            if (PropertyCache.ContainsKey(type))
            {
                return PropertyCache[type];
            }

            var properties = type.GetProperties(BindingFlags.Public);
            PropertyCache.Add(type, properties);
            return properties;
        }
    }
}
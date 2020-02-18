namespace Dfe.Spi.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Contains extension methods for the <see cref="object" /> class.
    /// </summary>
    public static class ObjectExtensions
    {
        private static Dictionary<Type, PropertyInfo[]> typeProperties =
            new Dictionary<Type, PropertyInfo[]>();

        /// <summary>
        /// Prunes an existing input <paramref name="model" />, based on the
        /// supplied <paramref name="propertiesToInclude" />.
        /// </summary>
        /// <typeparam name="TModel">
        /// A model type.
        /// </typeparam>
        /// <param name="model">
        /// An instance of type <typeparamref name="TModel" />.
        /// </param>
        /// <param name="propertiesToInclude">
        /// An array of <see cref="string" /> values, describing the properties
        /// to include on the output <typeparamref name="TModel" />.
        /// </param>
        /// <returns>
        /// The pruned <typeparamref name="TModel" />.
        /// </returns>
        public static TModel PruneModel<TModel>(
            this TModel model,
            string[] propertiesToInclude)
            where TModel : class
        {
            TModel toReturn = null;

            // 1) Get back all the properties for the input model type.
            Type modelType = typeof(TModel);

            PropertyInfo[] propertyInfos = null;
            if (typeProperties.ContainsKey(modelType))
            {
                propertyInfos = typeProperties[modelType];
            }
            else
            {
                propertyInfos = modelType.GetProperties();

                typeProperties.Add(modelType, propertyInfos);
            }

            // 2) Create a new empty instance of type TModel.
            toReturn = Activator.CreateInstance<TModel>();

            string[] fieldsUpper = propertiesToInclude
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => x.ToUpperInvariant())
                .ToArray();

            // 3) Cycle through and add set fields via reflection specified in
            // fields.
            PropertyInfo propertyInfo = null;
            foreach (string field in fieldsUpper)
            {
                propertyInfo = propertyInfos
                    .SingleOrDefault(x => x.Name.ToUpperInvariant() == field);

                if (propertyInfo != null)
                {
                    // Get the value and...
                    object currentValue = propertyInfo.GetValue(model);

                    // Copy it.
                    propertyInfo.SetValue(toReturn, currentValue);
                }
            }

            // 4) Return.
            return toReturn;
        }
    }
}
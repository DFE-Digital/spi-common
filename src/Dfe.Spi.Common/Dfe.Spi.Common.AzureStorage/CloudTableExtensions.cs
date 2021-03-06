﻿namespace Dfe.Spi.Common.AzureStorage
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Contains extension methods for the <see cref="CloudTable" /> class.
    /// </summary>
    public static class CloudTableExtensions
    {
        /// <summary>
        /// Modified version of the solution from
        /// <see href="https://stackoverflow.com/questions/24234350/how-to-execute-an-azure-table-storage-query-async-client-version-4-0-1" />.
        /// Creates an effective <c>ExecuteQueryAsync</c> method from the
        /// <see cref="CloudTable.ExecuteQuerySegmentedAsync(TableQuery, TableContinuationToken)" />
        /// method.
        /// </summary>
        /// <typeparam name="TTableEntity">
        /// A type deriving from <see cref="ITableEntity" />.
        /// </typeparam>
        /// <param name="cloudTable">
        /// An instance of <see cref="CloudTable" />.
        /// </param>
        /// <param name="tableQuery">
        /// An instance of <see cref="TableQuery{TTableEntity}" />.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="IList{TTableEntity}" />.
        /// </returns>
        public static async Task<IList<TTableEntity>> ExecuteQueryAsync<TTableEntity>(
            this CloudTable cloudTable,
            TableQuery<TTableEntity> tableQuery,
            CancellationToken cancellationToken)
            where TTableEntity : ITableEntity, new()
        {
            List<TTableEntity> toReturn = new List<TTableEntity>();

            if (cloudTable == null)
            {
                throw new ArgumentNullException(nameof(cloudTable));
            }

            if (tableQuery == null)
            {
                throw new ArgumentNullException(nameof(tableQuery));
            }

            TableQuery<TTableEntity> runningQuery =
                new TableQuery<TTableEntity>()
                {
                    FilterString = tableQuery.FilterString,
                    SelectColumns = tableQuery.SelectColumns,
                };

            TableContinuationToken token = null;
            do
            {
                runningQuery.TakeCount = tableQuery.TakeCount - toReturn.Count;

                TableQuerySegment<TTableEntity> tableQuerySegment = null;

                tableQuerySegment =
                    await cloudTable.ExecuteQuerySegmentedAsync(
                        runningQuery,
                        token)
                        .ConfigureAwait(false);

                token = tableQuerySegment.ContinuationToken;
                toReturn.AddRange(tableQuerySegment);
            }
            while ((token != null) && (!cancellationToken.IsCancellationRequested) && (tableQuery.TakeCount == null || toReturn.Count < tableQuery.TakeCount.Value));

            return toReturn;
        }

        /// <summary>
        /// Modified version of the solution from
        /// <see href="https://stackoverflow.com/questions/24234350/how-to-execute-an-azure-table-storage-query-async-client-version-4-0-1" />.
        /// Creates an effective <c>ExecuteQueryAsync</c> method from the
        /// <see cref="CloudTable.ExecuteQuerySegmentedAsync(TableQuery, TableContinuationToken)" />
        /// method.
        /// </summary>
        /// <typeparam name="TTableEntity">
        /// A type deriving from <see cref="ITableEntity" />.
        /// </typeparam>
        /// <param name="cloudTable">
        /// An instance of <see cref="CloudTable" />.
        /// </param>
        /// <param name="tableQuery">
        /// An instance of <see cref="TableQuery{TTableEntity}" />.
        /// </param>
        /// <param name="entityResolver">
        /// An instance of <see cref="EntityResolver{TTableEntity}" />.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="IList{TTableEntity}" />.
        /// </returns>
        public static async Task<IList<TTableEntity>> ExecuteQueryAsync<TTableEntity>(
            this CloudTable cloudTable,
            TableQuery tableQuery,
            EntityResolver<TTableEntity> entityResolver,
            CancellationToken cancellationToken)
            where TTableEntity : ITableEntity
        {
            List<TTableEntity> toReturn = new List<TTableEntity>();

            if (cloudTable == null)
            {
                throw new ArgumentNullException(nameof(cloudTable));
            }

            if (tableQuery == null)
            {
                throw new ArgumentNullException(nameof(tableQuery));
            }

            TableQuery runningQuery =
                new TableQuery()
                {
                    FilterString = tableQuery.FilterString,
                    SelectColumns = tableQuery.SelectColumns,
                };

            TableContinuationToken token = null;
            do
            {
                runningQuery.TakeCount = tableQuery.TakeCount - toReturn.Count;

                TableQuerySegment<TTableEntity> tableQuerySegment = null;

                tableQuerySegment =
                    await cloudTable.ExecuteQuerySegmentedAsync(
                        runningQuery,
                        entityResolver,
                        token)
                        .ConfigureAwait(false);

                token = tableQuerySegment.ContinuationToken;
                toReturn.AddRange(tableQuerySegment);
            }
            while ((token != null) && (!cancellationToken.IsCancellationRequested) && (tableQuery.TakeCount == null || toReturn.Count < tableQuery.TakeCount.Value));

            return toReturn;
        }
    }
}
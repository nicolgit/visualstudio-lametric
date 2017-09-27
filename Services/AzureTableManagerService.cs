using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using nicold.visualstudio.to.lametric.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nicold.visualstudio.to.lametric.Services
{
    public class AzureTableManagerService: IAzureTableManager
    {
        private ISettings _settings;
        public AzureTableManagerService(ISettings settings)
        {
            _settings = settings;
        }
        
        async Task<bool> IAzureTableManager.UpdateRow(LametricEntity row)
        {
            var table = await _getTableObject();

            TableOperation insertOperation = TableOperation.InsertOrReplace(row);
            await table.ExecuteAsync(insertOperation);

            return true;
        }

        async Task<LametricEntity> IAzureTableManager.GetRow(string email)
        {
            var table = await _getTableObject();

            TableQuery<LametricEntity> query = new TableQuery<LametricEntity>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, email),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, email)));

            TableContinuationToken token = null;
            TableQuerySegment<LametricEntity> seg = await table.ExecuteQuerySegmentedAsync<LametricEntity>(query, token);

            return seg.FirstOrDefault();
        }

        public async Task<bool> UpdateUrl(string email, string url)
        {
            var table = await _getTableObject();

            var entity = new DynamicTableEntity(email, email);
            entity.ETag = "*";
            entity.Properties.Add("VSO_Url", new EntityProperty(url));
            var mergeOperation = TableOperation.Merge(entity);

            try
            {
                await table.ExecuteAsync(mergeOperation);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<CloudTable> _getTableObject()
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(_settings.BlobStorageConnectionString);
            var tableClient = cloudStorageAccount.CreateCloudTableClient();

            var table = tableClient.GetTableReference("connections");
            await table.CreateIfNotExistsAsync();

            return table;
        }

        
    }
}

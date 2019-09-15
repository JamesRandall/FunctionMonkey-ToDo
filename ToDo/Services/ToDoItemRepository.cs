using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using ToDo.Models;

namespace ToDo.Services
{
    internal class ToDoItemRepository : IToDoItemRepository
    {
        private readonly CosmosClient _cosmosClient;
        private readonly string _databaseName;
        private readonly string _containerName;
        
        public ToDoItemRepository(ApplicationSettings settings)
        {
            CosmosClientOptions options = new CosmosClientOptions()
            {
                SerializerOptions = new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            };
            _cosmosClient = new CosmosClient(settings.CosmosConnectionString, options);
            _databaseName = settings.CosmosDatabaseName;
            _containerName = settings.CosmosContainerName;
        }
        
        public async Task Upsert(ToDoItem toDoItem)
        {
            Container container = _cosmosClient.GetContainer(_databaseName, _containerName);
            await container.UpsertItemAsync(toDoItem, new PartitionKey(toDoItem.Id));
        }

        public async Task<IReadOnlyCollection<ToDoItem>> Get(string userId)
        {
            string query = "SELECT * FROM c WHERE c.createdByUserId = @userId";
            Container container = _cosmosClient.GetContainer(_databaseName, _containerName);
            QueryDefinition queryDefinition = new QueryDefinition(query)
                .WithParameter("@userId", userId);
            FeedIterator<ToDoItem> queryResultSetIterator = container.GetItemQueryIterator<ToDoItem>(queryDefinition);

            List<ToDoItem> items = new List<ToDoItem>();
            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<ToDoItem> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                items.AddRange(currentResultSet);
            }

            return items;
        }

        public async Task<ToDoItem> GetSingleItem(string itemId)
        {
            Container container = _cosmosClient.GetContainer(_databaseName, _containerName);
            return await container.ReadItemAsync<ToDoItem>(itemId, new PartitionKey(itemId));
        }
    }
}
using CosmosDbRepository.Core.Entities.Base;
using CosmosDbRepository.Core.Interfaces;
using CosmosDbRepository.Infrastracture.Interfaces;
using CosmosDbRepository.Infrastracture.Models;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbRepository.Infrastracture.Repository
{
    public abstract class CosmosDbRepository<T> : IRepository<T> , IContainerContext<T> where T : BaseEntity
    {
        private readonly ICosmosDbContainerFactory cosmosDbContainerFactory;
        private readonly Container container;


        public CosmosDbRepository(ICosmosDbContainerFactory cosmosDbContainerFactory)
        {
            this.cosmosDbContainerFactory = cosmosDbContainerFactory ?? throw new ArgumentNullException(nameof(cosmosDbContainerFactory));
            this.container = this.cosmosDbContainerFactory.GetContainer(ContainerName).Container;
        }

        public abstract string ContainerName { get;}
        public abstract string GenerateId(T entity);
        public abstract string ResolvePartitionKey(T entity); 
        

        public virtual async Task DeleteItem(string id, PartitionKey partitionKey)
        {
           await container.DeleteItemAsync<T>(id, partitionKey);
        }
        public virtual async Task AddItem(T item)
        {
            item.Id = GenerateId(item);
            string pKey = ResolvePartitionKey(item);
            await container.CreateItemAsync<T>(item, new PartitionKey(pKey));
        }
        public virtual async Task AddItemsAsync(IEnumerable<T> items)
        {
            List<Task> concurrentOps = new();
            foreach (var item in items)
            {
                string pKey = ResolvePartitionKey(item);
                concurrentOps.Add(container.CreateItemAsync(item, new PartitionKey(pKey)));
            }

            await Task.WhenAll(concurrentOps);
        }
        public virtual async Task<IEnumerable<T>> GetItems(QueryDefinition query)
        {
            FeedIterator<T> resultsIterator = container.GetItemQueryIterator<T>(query);
            List<T> results = new List<T>();
            while (resultsIterator.HasMoreResults)
            {
                FeedResponse<T> res = await resultsIterator.ReadNextAsync();
                results.AddRange(res.ToList());
            }
            return results;
        }
        public virtual async Task UpdateItem(string id, T item)
        {
            string pKey = ResolvePartitionKey(item);
            await this.container.UpsertItemAsync<T>(item, new PartitionKey(pKey));
        }
    }
}

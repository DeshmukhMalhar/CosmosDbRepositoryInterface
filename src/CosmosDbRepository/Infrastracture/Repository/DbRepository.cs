using CosmosDbRepository.Core.Entities.Base;
using CosmosDbRepository.Core.Interfaces;
using CosmosDbRepository.Infrastracture.Interfaces;
using CosmosDbRepository.Infrastracture.Models;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbRepository.Infrastracture.Repository
{
    /// <summary>
    /// Implement the startup as
    /// public static class DatabaseConfig
    ///    {
    ///        public static void SetupCosmosDb(this IServiceCollection services, IConfiguration configuration)
    ///    {
    ///        CosmosDbSettings cosmosDbSettings = configuration.GetSection("ConnectionStrings:CosmosDbConnectionString").Get<CosmosDbSettings>();
    ///        services.AddCosmosDb(
    ///            cosmosDbSettings.EndpointUrl,
    ///            cosmosDbSettings.PrimaryKey,
    ///            cosmosDbSettings.DatabaseName,
    ///            cosmosDbSettings.Containers
    ///            );    
    ///        services.AddScoped<IToDoItemRepository, ToDoItemRepository>();    
    ///    }
    ///}
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>


public abstract class DbRepository<T> : IDbRepository<T>, IContainerContext<T> where T : BaseEntity
    {
        private readonly Container partitionKeys;
        private readonly IPartitionKeyStoreRepository partitionKeyStoreRepository;
        private readonly Container container;
        public DbRepository(ICosmosDbContainerFactory cosmosDbContainerFactory )
        {
            this.container = cosmosDbContainerFactory.GetContainer(ContainerName).Container;
            this.partitionKeys = cosmosDbContainerFactory.GetContainer("PartitionKeyStore").Container; //do we need this container here ?
            this.partitionKeyStoreRepository = new PartitionKeyStoreRepository(cosmosDbContainerFactory);

        }
        public abstract string ContainerName { get; }
        public abstract string GenerateId(T entity);
        public abstract string ResolvePartitionKey(T entity);

        public virtual async Task AddItem(T item)
        {
            string id = GenerateId(item);
            string pKey = ResolvePartitionKey(item);     
            
            PartitionKeyStore keyStore = new(ContainerName, id, pKey);
            await partitionKeyStoreRepository.AddItem(keyStore);
            await container.CreateItemAsync(item, new PartitionKey(pKey));
        }
        public virtual async Task DeleteItem(string id, PartitionKey partitionKey)
        {
            await partitionKeyStoreRepository.DeleteItem(
                (await partitionKeyStoreRepository.GetKeyStoresByEntityId(id)).FirstOrDefault().Id,
                    new PartitionKey(ContainerName))
                ;
           await container.DeleteItemAsync<T>(id, partitionKey);
        }

        public virtual async Task DeleteItem(string Id)
        {
            var partitionKeyStore = (await partitionKeyStoreRepository.GetKeyStoresByEntityId(Id)).FirstOrDefault();

            await partitionKeyStoreRepository.DeleteItem(partitionKeyStore.Id, new PartitionKey(ContainerName));
            await container.DeleteItemAsync<T>(Id, new PartitionKey(partitionKeyStore.EntityPartitionKey));
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
            await container.UpsertItemAsync(item);
        }
    }
}

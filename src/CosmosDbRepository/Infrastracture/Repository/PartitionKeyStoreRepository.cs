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
    public class PartitionKeyStoreRepository : CosmosDbRepository<Models.PartitionKeyStore>, Interfaces.IPartitionKeyStoreRepository
    {

        public PartitionKeyStoreRepository(ICosmosDbContainerFactory cosmosDbContainerFactory) : base(cosmosDbContainerFactory)
        {
        }

        public override string ContainerName { get; } = "PartitionKeyStore";

        public async Task<PartitionKey> GetPartitionKeyByEntityId(string entityId)
        {
            var  res = (await GetKeyStoresByEntityId(entityId)).FirstOrDefault().EntityPartitionKey;
            return new PartitionKey(res);
        }

        public async Task<IEnumerable<PartitionKeyStore>> GetKeyStoresByEntityId(string entityId)
        {
            string queryString = $@"select * from c where c.entityId = @entityId order by c._ts desc";
            QueryDefinition query = new QueryDefinition(queryString).WithParameter("@entityId", entityId);
            var res = (await GetItems(query));
            return res;
        }

        public override string GenerateId(PartitionKeyStore entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            return entity.Id;
        }

        public override string ResolvePartitionKey(PartitionKeyStore keyItem)
        {
            return keyItem.EntityCollectionName;
        }
    }
}

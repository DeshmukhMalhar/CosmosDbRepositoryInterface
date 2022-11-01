using CosmosDbRepository.Infrastracture.Models;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbRepository.Infrastracture.Interfaces
{
    public interface IPartitionKeyStoreRepository : Core.Interfaces.IRepository<Models.PartitionKeyStore>
    {
        Task<IEnumerable<PartitionKeyStore>> GetKeyStoresByEntityId(string entityId);
        Task<PartitionKey> GetPartitionKeyByEntityId(string entityId);
    }
}

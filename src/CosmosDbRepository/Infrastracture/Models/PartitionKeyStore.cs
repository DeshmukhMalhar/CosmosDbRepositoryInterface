using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbRepository.Infrastracture.Models
{
    public class PartitionKeyStore : Core.Entities.Base.BaseEntity
    {
        public PartitionKeyStore(string entityCollectionName, string entityId, string entityPartitionKey)
        {
            Id = Guid.NewGuid().ToString();
            EntityCollectionName = entityCollectionName;
            EntityId = entityId;
            EntityPartitionKey = entityPartitionKey;
        }

        /// <summary>
        /// Use this prop as the partitionkey, 
        /// so, the values are partitioned by container/model
        /// </summary>
        
        [JsonProperty(PropertyName = "entityCollectionName")]
        public string EntityCollectionName { get; set; }
        
        [JsonProperty(PropertyName = "entityId")]
        public string EntityId { get; set; }

        [JsonProperty(PropertyName = "entityPartitionKey ")]
        public string EntityPartitionKey { get; set; }
    }    
}

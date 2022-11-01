using CosmosDbRepository.Infrastracture.Interfaces;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbRepository.Infrastracture
{
    public class CosmosDbContainer : ICosmosDbContainer
    {        
        public Container Container { get; }

        public CosmosDbContainer(
            CosmosClient cosmosClient,
            string databaseName,
            string containerName
            )
        {
            this.Container = cosmosClient.GetContainer(databaseName, containerName);
        }        
    }
}

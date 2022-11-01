using CosmosDbRepository.Infrastracture.AppSettings;
using CosmosDbRepository.Infrastracture.Interfaces;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbRepository.Infrastracture
{
    public class CosmosDbContainerFactory : ICosmosDbContainerFactory
    {
        private readonly CosmosClient cosmosClient;
        private readonly string databaseName;
        private readonly List<ContainerInfo> containers;


        public CosmosDbContainerFactory(CosmosClient cosmosClient, 
            string databaseName, 
            List<ContainerInfo> containers)
        {
            this.cosmosClient = cosmosClient ?? throw new ArgumentNullException(nameof(cosmosClient)) ;
            this.databaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            this.containers = containers ?? throw new ArgumentNullException(nameof(containers));
        }
    
        public Task EnsureDbSetupAsync()
        {
            throw new NotImplementedException();
        }

        public ICosmosDbContainer GetContainer(string containerName)
        {
            if(containers.Where(x=>x.Name == containerName) == null)
            {
                throw new ArgumentException($"Unable to Find Container{containerName}");
            }
            return new CosmosDbContainer(cosmosClient, databaseName, containerName);
        }
    }
}

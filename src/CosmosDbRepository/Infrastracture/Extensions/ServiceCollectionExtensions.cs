using CosmosDbRepository.Infrastracture.AppSettings;
using CosmosDbRepository.Infrastracture.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbRepository.Infrastracture.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCosmosDb(this IServiceCollection services,
            string endpointUrl,
            string primaryKey,
            string databaseName,
            List<ContainerInfo> containers
            )
        {
            CosmosClientOptions options = new CosmosClientOptions() { AllowBulkExecution = true };
            CosmosClient client = new(endpointUrl, primaryKey, options);

            CosmosDbContainerFactory cosmosDbContainerFactory = new CosmosDbContainerFactory(client, databaseName, containers);

            services.AddSingleton<ICosmosDbContainerFactory>(cosmosDbContainerFactory);
            return services;
        }
    }
}

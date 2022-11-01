

using Microsoft.Azure.Cosmos;

namespace CosmosDbRepository.Infrastracture.Interfaces
{
    public interface ICosmosDbContainer
    {
        Container Container { get; }
    }
}

using CosmosDbRepository.Core.Entities.Base;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbRepository.Core.Interfaces
{
    public interface IRepository<T> where  T:BaseEntity 
    {
        Task AddItem(T item); //C
        Task<IEnumerable<T>> GetItems(QueryDefinition query); //R
        Task UpdateItem(string id, T item); //U
        Task DeleteItem(string id, PartitionKey partitionKey); //D
    }
}

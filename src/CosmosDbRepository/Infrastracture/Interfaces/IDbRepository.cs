using CosmosDbRepository.Core.Entities.Base;
using CosmosDbRepository.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbRepository.Infrastracture.Interfaces
{
    public interface IDbRepository<T> :IRepository<T> where T : BaseEntity
    {
        Task DeleteItem(string Id);

    }
}

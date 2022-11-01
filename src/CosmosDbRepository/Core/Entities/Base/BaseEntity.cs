using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CosmosDbRepository.Core.Entities.Base
{
    public abstract class BaseEntity
    {
        [JsonProperty(PropertyName ="id")]
        public virtual string Id { get; set; }
    }
}

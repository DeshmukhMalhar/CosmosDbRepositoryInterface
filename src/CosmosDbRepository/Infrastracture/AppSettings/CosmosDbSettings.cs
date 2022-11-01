using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbRepository.Infrastracture.AppSettings
{
    public class CosmosDbSettings
    {
        public string EndpointUrl { get; set; }
        public string PrimaryKey { get; set; }
        public string DatabaseName { get; set; }

        public List<ContainerInfo> Containers { get; set; }
    }

    public class ContainerInfo
    {
        public string Name { get; set; }
        public string PartitionKey { get; set; }
    }
}

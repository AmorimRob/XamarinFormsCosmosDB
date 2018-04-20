using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleCosmosDb.Models
{
    public abstract class CosmosDbModel<T> where T : CosmosDbModel<T>
    {
        public string CollectionId => typeof(T).Name;
        public string DatabaseId { get; private set; }

        public string TypeName => typeof(T).Name;

        [JsonProperty("id")]
        public string Id { get; set; }

        public CosmosDbModel()
        {
            DatabaseId = "SampleCosmos";
        }
        
    }
}

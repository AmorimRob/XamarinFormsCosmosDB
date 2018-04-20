using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleCosmosDb.Models
{
    public class Clubs : CosmosDbModel<Clubs>
    {
        public string Name { get; set; }
        public string Country { get; set; }

        public static explicit operator Clubs(Document doc)
        {
            return new Clubs
            {
                Name = doc.GetPropertyValue<string>(nameof(Name)),
                Country = doc.GetPropertyValue<string>(nameof(Country)),
                Id = doc.GetPropertyValue<string>(nameof(Id).ToLower())
            };
        }

    }
}

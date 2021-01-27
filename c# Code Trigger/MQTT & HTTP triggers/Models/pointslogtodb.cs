using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_MQTT.Models
{
    public class pointslogtodb
    {
        public string name { get; set; }
        public int points { get; set; }
        
    }

    public class pointslogtodbEntity : TableEntity
    {
        public pointslogtodbEntity()
        {

        }

        public pointslogtodbEntity(string name, string id)
        {
            PartitionKey = name;
            RowKey = id;
        }

        public int points { get; set; }
     
    }
}

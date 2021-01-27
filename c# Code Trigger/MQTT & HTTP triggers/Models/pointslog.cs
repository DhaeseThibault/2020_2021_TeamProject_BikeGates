using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_MQTT.Models
{
    public class pointslog
    {
        public string name { get; set; }
        public DateTimeOffset timestamp { get; set; }
        public int points { get; set; }
        
    }

    public class pointslogEntity : TableEntity
    {
        public pointslogEntity()
        {

        }

        public pointslogEntity(string name, string id)
        {
            PartitionKey = name;
            RowKey = id;
        }

        public DateTimeOffset timestamp { get; set; }
        public int points { get; set; }
     
    }
}

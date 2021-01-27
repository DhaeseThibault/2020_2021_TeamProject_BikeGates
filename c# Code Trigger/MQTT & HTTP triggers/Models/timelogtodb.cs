using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_MQTT.Models
{
    public class timelogtodb
    {
        public string name { get; set; }
        public int isFinished { get; set; }
        public int totalTime { get; set; }


    }

    public class timelogtodbEntity : TableEntity
    {
        public timelogtodbEntity()
        {

        }

        public timelogtodbEntity(string name, string id)
        {
            PartitionKey = name;
            RowKey = id;
        }


        public int isFinished { get; set; }
        public int totalTime { get; set; }

    }
}

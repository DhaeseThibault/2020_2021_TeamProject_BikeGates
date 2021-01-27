using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_MQTT.Models
{
    public class timelog
    {
        public string name { get; set; }
        public DateTimeOffset timestamp { get; set; }
        public int isFinished { get; set; }
        public int totalTime { get; set; }


    }

    public class timelogEntity : TableEntity
    {
        public timelogEntity()
        {

        }

        public timelogEntity(string name, string id)
        {
            PartitionKey = name;
            RowKey = id;
        }

        public DateTimeOffset timestamp { get; set; }
        public int isFinished { get; set; }
        public int totalTime { get; set; }

    }
}

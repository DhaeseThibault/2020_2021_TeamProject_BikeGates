using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CaseOnline.Azure.WebJobs.Extensions.Mqtt;
using CaseOnline.Azure.WebJobs.Extensions.Mqtt.Messaging;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Table;
using Demo_MQTT.Models;

namespace Demo_MQTT
{
    public class ReceiveMessageFunction
    {
        
        [FunctionName("ProcessPoints")]
        public async Task ProcessPoints([MqttTrigger("ESP/Parkour/ToDB", ConnectionString = "MqttConnection")] IMqttMessage message, ILogger logger)
        {
            try
            {
                var body = message.GetMessage();
                var bodyString = Encoding.UTF8.GetString(body);
                var records = JsonConvert.DeserializeObject<pointslogtodb>(bodyString);

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("tablestorageconnection"));
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new Microsoft.Azure.Cosmos.Table.TableClientConfiguration());
                CloudTable table = tableClient.GetTableReference("LeaderboardParkour");

                TableQuery<pointslogtodbEntity> getquery = new TableQuery<pointslogtodbEntity>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, records.name));

                var getQueryResult = await table.ExecuteQuerySegmentedAsync<pointslogtodbEntity>(getquery, null);
                pointslogtodbEntity entity = new pointslogtodbEntity();

                if (getQueryResult.Results.Count == 0)
                {
                    entity.PartitionKey = records.name;
                    entity.RowKey = Guid.NewGuid().ToString();
                    entity.points = records.points;
                }
                else
                {
                    entity.PartitionKey = records.name;
                    entity.RowKey = getQueryResult.Results[0].RowKey;
                    entity.points = getQueryResult.Results[0].points + records.points;
                }

                TableOperation insertOrMergeOperation = Microsoft.Azure.Cosmos.Table.TableOperation.InsertOrMerge(entity);
                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [FunctionName("ProcessTimeRace")]
        public async Task ProcessTimeRace([MqttTrigger("ESP/TimeRace/ToDB", ConnectionString = "MqttConnection2")] IMqttMessage message, ILogger logger)
        {
            try
            {
                var body = message.GetMessage();
                var bodyString = Encoding.UTF8.GetString(body);
                var records = JsonConvert.DeserializeObject<timelogtodb>(bodyString);

                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("tablestorageconnection"));
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new Microsoft.Azure.Cosmos.Table.TableClientConfiguration());
                CloudTable table = tableClient.GetTableReference("LeaderboardTimeRace");

                TableQuery<timelogtodbEntity> getquery = new TableQuery<timelogtodbEntity>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, records.name));

                var getQueryResult = await table.ExecuteQuerySegmentedAsync<timelogtodbEntity>(getquery, null);
                timelogtodbEntity entity = new timelogtodbEntity();

                if (getQueryResult.Results.Count == 0)
                {
                    entity.PartitionKey = records.name;
                    entity.RowKey = Guid.NewGuid().ToString();
                    entity.isFinished = records.isFinished;
                    entity.totalTime = records.totalTime;
                }
                else
                {
                    entity.PartitionKey = records.name;
                    entity.RowKey = getQueryResult.Results[0].RowKey;
                    if (records.totalTime == 0)
                    {
                        entity.isFinished = records.isFinished;
                        entity.totalTime = getQueryResult.Results[0].totalTime;
                    }
                    else
                    {
                        entity.isFinished = records.isFinished;
                        entity.totalTime = records.totalTime;
                    }
                }

                TableOperation insertOrMergeOperation = Microsoft.Azure.Cosmos.Table.TableOperation.InsertOrMerge(entity);
                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;
using Demo_MQTT.Models;
using System.Collections.Generic;
using CaseOnline.Azure.WebJobs.Extensions.Mqtt.Messaging;
using System.Text;
using CaseOnline.Azure.WebJobs.Extensions.Mqtt;

namespace Demo_MQTT
{
    public static class HTTPtriggerFunction
    {
        [FunctionName("GetParkour")]
        public static async Task<IActionResult> GetParkour(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "LeaderboardParkour")] HttpRequest req,
            ILogger log)
        {
            try
            {
                string connectionstring = Environment.GetEnvironmentVariable("tablestorageconnection");
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionstring);
                CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
                CloudTable table = cloudTableClient.GetTableReference("LeaderboardParkour");

                TableQuery<pointslogEntity> rangeQuery = new TableQuery<pointslogEntity>();

                var queryResult = await table.ExecuteQuerySegmentedAsync<pointslogEntity>(rangeQuery, null);
                List<pointslog> points = new List<pointslog>();

                foreach (var reg in queryResult.Results)
                {
                    points.Add(new pointslog()
                    {
                        name = reg.PartitionKey,
                        timestamp = reg.Timestamp,
                        points = reg.points
                    });
                }

                return new OkObjectResult(points);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(500);
            }
        }

        [FunctionName("GetParkourByName")]
        public static async Task<IActionResult> GetParkourByName(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "LeaderboardParkour/{name}")] HttpRequest req, string name,
            ILogger log)
        {
            try
            {
                string connectionstring = Environment.GetEnvironmentVariable("tablestorageconnection");
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionstring);
                CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
                CloudTable table = cloudTableClient.GetTableReference("LeaderboardParkour");

                TableQuery<pointslogEntity> rangeQuery = new TableQuery<pointslogEntity>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, name));

                var queryResult = await table.ExecuteQuerySegmentedAsync<pointslogEntity>(rangeQuery, null);
                List<pointslog> points = new List<pointslog>();

                foreach (var reg in queryResult.Results)
                {
                    points.Add(new pointslog()
                    {
                        name = reg.PartitionKey,
                        points = reg.points
                    });
                }

                return new OkObjectResult(points);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(500);
            }
        }

        [FunctionName("GetTimeRace")]
        public static async Task<IActionResult> GetTimeRace(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "LeaderboardTimeRace")] HttpRequest req,
            ILogger log)
        {
            try
            {
                string connectionstring = Environment.GetEnvironmentVariable("tablestorageconnection");
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionstring);
                CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
                CloudTable table = cloudTableClient.GetTableReference("LeaderboardTimeRace");

                TableQuery<timelogEntity> rangeQuery = new TableQuery<timelogEntity>();

                var queryResult = await table.ExecuteQuerySegmentedAsync<timelogEntity>(rangeQuery, null);
                List<timelog> points = new List<timelog>();

                foreach (var reg in queryResult.Results)
                {
                    points.Add(new timelog()
                    {
                        name = reg.PartitionKey,
                        timestamp = reg.Timestamp,
                        isFinished = reg.isFinished,
                        totalTime = reg.totalTime

                    });
                }

                return new OkObjectResult(points);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(500);
            }
        }

        [FunctionName("GetTimeRaceByName")]
        public static async Task<IActionResult> GetTimeRaceByName(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "LeaderboardTimeRace/{name}")] HttpRequest req, string name,
            ILogger log)
        {
            try
            {
                string connectionstring = Environment.GetEnvironmentVariable("tablestorageconnection");
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionstring);
                CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
                CloudTable table = cloudTableClient.GetTableReference("LeaderboardTimeRace");

                TableQuery<timelogEntity> rangeQuery = new TableQuery<timelogEntity>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, name));

                var queryResult = await table.ExecuteQuerySegmentedAsync<timelogEntity>(rangeQuery, null);
                List<timelog> points = new List<timelog>();

                foreach (var reg in queryResult.Results)
                {
                    points.Add(new timelog()
                    {
                        name = reg.PartitionKey,
                        isFinished = reg.isFinished,
                        totalTime = reg.totalTime

                    });
                }

                return new OkObjectResult(points);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return new StatusCodeResult(500);
            }
        }

        [FunctionName("startParkour")]
        public static ActionResult sendMessageTest(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "StartParkour")] HttpRequest req,
            [Mqtt] out IMqttMessage outMessage,
            ILogger log)
        {
            try
            {
                string requestBody = new StreamReader(req.Body).ReadToEnd();

                outMessage = new MqttMessage("ESP/Start/Parkour2", Encoding.ASCII.GetBytes(requestBody), MqttQualityOfServiceLevel.AtMostOnce, false);

                return new StatusCodeResult(200);
            }
            catch (Exception ex)
            { 
                outMessage = null;
                return new StatusCodeResult(500);
            }
        }

        [FunctionName("startTimeRace")]
        public static ActionResult startTimeRace(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "StartTimeRace")] HttpRequest req,
            [Mqtt] out IMqttMessage outMessage,
            ILogger log)
        {
            try
            {
                string requestBody = new StreamReader(req.Body).ReadToEnd();

                outMessage = new MqttMessage("ESP/Start/TimeRace", Encoding.ASCII.GetBytes(requestBody), MqttQualityOfServiceLevel.AtMostOnce, false);

                return new StatusCodeResult(200);
            }
            catch (Exception ex)
            {
                outMessage = null;
                return new StatusCodeResult(500);
            }
        }

        [FunctionName("StopGamemode")]
        public static ActionResult StopGamemode(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "GameMode/Stop")] HttpRequest req,
            [Mqtt] out IMqttMessage outMessage,
            ILogger log)
        {
            try
            {
                string requestBody = new StreamReader(req.Body).ReadToEnd();

                outMessage = new MqttMessage("ESP/GameMode/Stop", Encoding.ASCII.GetBytes(requestBody), MqttQualityOfServiceLevel.AtMostOnce, false);

                return new StatusCodeResult(200);
            }
            catch (Exception ex)
            {
                outMessage = null;
                return new StatusCodeResult(500);
            }
        }

        [FunctionName("AddTimeRaceTotalTime")]
        public static ActionResult AddTimeRaceTotalTime(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "TimeRace/Time")] HttpRequest req,
           [Mqtt] out IMqttMessage outMessage,
           ILogger log)
        {
            try
            {
                string requestBody = new StreamReader(req.Body).ReadToEnd();

                outMessage = new MqttMessage("ESP/TimeRace/ToDB", Encoding.ASCII.GetBytes(requestBody), MqttQualityOfServiceLevel.AtMostOnce, false);

                return new StatusCodeResult(200);
            }
            catch (Exception ex)
            {
                outMessage = null;
                return new StatusCodeResult(500);
            }
        }
    }
}

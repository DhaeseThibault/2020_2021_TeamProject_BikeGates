using BikeGates.Models;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BikeGates.Repositories
{
    public class BikeGatesRepository
    {
        private const string _BASEURL = "https://bikegatestrigger.azurewebsites.net/api";
        private const string _PARKOUR = "LeaderboardParkour";
        private const string _TIMERACE = "LeaderboardTimeRace";
        private const string _SURVIVAL = "LeaderboardSurvival";
        private static HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("accept", "application/json");
            return client;
        }

        public static async Task<List<Parkour>> GetParkour()
        {
            string url = $"{_BASEURL}/{_PARKOUR}";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    string json = await client.GetStringAsync(url);
                    List<Parkour> list = JsonConvert.DeserializeObject<List<Parkour>>(json);

                    return list;
                }
                catch (Exception ex)
                {

                    throw ex; // hier altijd een breakpoint zetten
                    // je applicatie gaat niet stoppen op je foutmelding in xamarin
                }
            }
        }

        public static async Task<List<Parkour>> GetPointsParkour()
        {
            string url = $"{_BASEURL}/{_PARKOUR}";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    string json = await client.GetStringAsync(url);
                    List<Parkour> list = JsonConvert.DeserializeObject<List<Parkour>>(json);

                    return list;
                }
                catch (Exception ex)
                {

                    throw ex; // hier altijd een breakpoint zetten
                    // je applicatie gaat niet stoppen op je foutmelding in xamarin
                }
            }
        }


        public static async Task<List<TimeRace>> GetTimeRace()
        {
            string url = $"{_BASEURL}/{_TIMERACE}";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    string json = await client.GetStringAsync(url);
                    List<TimeRace> list = JsonConvert.DeserializeObject<List<TimeRace>>(json);

                    return list;
                }
                catch (Exception ex)
                {

                    throw ex; // hier altijd een breakpoint zetten
                    // je applicatie gaat niet stoppen op je foutmelding in xamarin
                }
            }
        }

        public static async Task<List<Parkour>> GetParkourByName(string name)
        {
            string url = $"{_BASEURL}/{_PARKOUR}/{name}";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    string json = await client.GetStringAsync(url);
                    List<Parkour> list = JsonConvert.DeserializeObject<List<Parkour>>(json);

                    return list;
                }
                catch (Exception ex)
                {

                    throw ex; // hier altijd een breakpoint zetten
                    // je applicatie gaat niet stoppen op je foutmelding in xamarin
                }
            }
        }
        public static async Task<List<TimeRace>> GetTimeRaceByName(string name)
        {
            string url = $"{_BASEURL}/{_TIMERACE}/{name}";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    string json = await client.GetStringAsync(url);
                    List<TimeRace> list = JsonConvert.DeserializeObject<List<TimeRace>>(json);

                    return list;
                }
                catch (Exception ex)
                {

                    throw ex; // hier altijd een breakpoint zetten
                    // je applicatie gaat niet stoppen op je foutmelding in xamarin
                }
            }
        }

        public static async void StartParkour(string playerName)
        {
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    var stringContent = new StringContent(playerName, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("https://bikegatestrigger.azurewebsites.net/api/StartParkour", stringContent);
                }
                catch (Exception ex)
                {

                    throw ex; // hier altijd een breakpoint zetten
                    // je applicatie gaat niet stoppen op je foutmelding in xamarin
                }
            }
        }

        public static async void StopParkour(string playerName)
        {
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    var stringContent = new StringContent(playerName, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("https://bikegatestrigger.azurewebsites.net/api/GameMode/Stop", stringContent);
                }
                catch (Exception ex)
                {

                    throw ex; // hier altijd een breakpoint zetten
                    // je applicatie gaat niet stoppen op je foutmelding in xamarin
                }
            }
        }

        public static async void StartTimeRace(string playerName)
        {
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    var stringContent = new StringContent(playerName, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("https://bikegatestrigger.azurewebsites.net/api/StartTimeRace", stringContent);
                }
                catch (Exception ex)
                {

                    throw ex; // hier altijd een breakpoint zetten
                    // je applicatie gaat niet stoppen op je foutmelding in xamarin
                }
            }
        }

        

        public static async void SetTimeRaceTime(string Time)
        {
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    var stringContent = new StringContent(Time, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("https://bikegatestrigger.azurewebsites.net/api/TimeRace/Time", stringContent);
                }
                catch (Exception ex)
                {

                    throw ex; // hier altijd een breakpoint zetten
                    // je applicatie gaat niet stoppen op je foutmelding in xamarin
                }
            }
        }
    }
}

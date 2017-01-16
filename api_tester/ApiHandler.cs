using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace api_tester
{
    public class ApiHandler
    {
        private readonly HttpClient _client;
        private readonly Dictionary<Endpoints, string> _apiEndpointsDictionary;
        private readonly string _baseUrl;

        public ApiHandler(string baseUrl)
        {
            _baseUrl = String.Format("{0}/EasyAutomation/services/api.php?",baseUrl);
            _client = new HttpClient();
            _apiEndpointsDictionary = new Dictionary<Endpoints, string>();
            InsertEndPointsToDictionary();
        }

        public async Task<List<Sensor>> GetSensorList()
        {
            HttpResponseMessage responeMessage = await _client.GetAsync(_apiEndpointsDictionary[Endpoints.ListSensors]);
            string response = await responeMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Sensor>>(response);
        }

        public async Task<JsonAnswer> CreateSensor(Sensor sensor)
        {
            string json = CreateJsonDataForServer(sensor, Endpoints.CreateSensor);
            var dataContent = new StringContent(json,Encoding.UTF8,"application/json");
            HttpResponseMessage responseMessage =
                await _client.PostAsync(_baseUrl,dataContent);
            string response = await responseMessage.Content.ReadAsStringAsync();
            return new JsonAnswer(response);
        }

        public async Task<JsonAnswer> DeleteSensor(Sensor sensor)
        {
            string json = CreateJsonDataForServer(sensor, Endpoints.DeleteSensor);
            var dataContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage =
                await _client.PostAsync(_baseUrl, dataContent);
            string response = await responseMessage.Content.ReadAsStringAsync();
            return new JsonAnswer(response);
        }

        public async Task<List<SensorData>> GetLatestSensorData(int sensorId)
        {
            string json = CreateJsonDataForServer(sensorId, Endpoints.GetlatestSensorData);
            var dataContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage =
                await _client.PostAsync(_baseUrl, dataContent);
            string response = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<SensorData>>(response);
        }

        public async Task<JsonAnswer> ChangeSensorValue(int sensorId, SensorData sensorData)
        {
            string json = CreateJsonDataForServer(sensorData, Endpoints.ChangesensorValue);
            var dataContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage =
                await _client.PostAsync(_baseUrl, dataContent);
            string response = await responseMessage.Content.ReadAsStringAsync();
            return new JsonAnswer(response);
        }

        #region Private methods

        private void InsertEndPointsToDictionary()
        {
            foreach (Endpoints endpoint in Enum.GetValues(typeof(Endpoints)))
            {
                if (endpoint != Endpoints.ListSensors)
                    _apiEndpointsDictionary.Add(endpoint, String.Format("method={0}",endpoint.ToString().ToLower()));
                else
                {
                    _apiEndpointsDictionary.Add(endpoint,String.Format("{0}method={1}",_baseUrl,endpoint));
                }
            }

        }

        private string CreateJsonDataForServer(Object objectToSerialize, Endpoints endpoint)
        {
            JObject jObject = new JObject();
            if (objectToSerialize is int)
            {
                jObject.Add("id", (int) objectToSerialize);
            }
            else
            {
                jObject = JObject.FromObject(objectToSerialize);
            }
            jObject.Add("method", endpoint.ToString().ToLower());
            return jObject.ToString();
        }

        #endregion
    }
}

using Newtonsoft.Json;

namespace api_tester
{
    public class SensorData
    {
        [JsonProperty(PropertyName = "time")]
        public string Time { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "unit")]
        public string Unit { get; set; }

        [JsonProperty(PropertyName = "sensorid")]
        public int SensorId { get; set; }

        public SensorData(string time, string value, string unit,int sensorid)
        {
            Time = time;
            Value = value;
            Unit = unit;
            SensorId = sensorid;
        }
    }
}
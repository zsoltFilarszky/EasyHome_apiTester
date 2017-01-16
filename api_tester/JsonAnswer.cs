using Newtonsoft.Json;

namespace api_tester
{
    public class JsonAnswer
    {
        public JsonAnswer(string answer)
        {
            Answer = answer;
        }
        public string Answer { get; set; }
    }
}
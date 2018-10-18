using Newtonsoft.Json;

namespace StatSnapShotter.Entities
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class StatElement
    {
        [JsonProperty(PropertyName = "name")]
        public string name { get; set; }

        [JsonProperty(PropertyName = "query")]
        public string query { get; set; }
        public int count { get; set; }
    }
}

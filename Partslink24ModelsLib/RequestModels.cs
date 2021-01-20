using Newtonsoft.Json;

namespace PartslinkModels
{
    public partial class Pathparams
    {
        [JsonProperty("ticket")]
        public string Ticket { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
    public class FirstPageAnswer
    {
        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("jsonUrl")]
        public string JsonUrl { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}

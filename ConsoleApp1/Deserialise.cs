using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ParsingLib;

namespace ConsoleParsingPartsLinks24
{
    public partial class Answers
    {
        [JsonProperty("tcLinks")]
        public List<Answer> ContentList { get; set; }
        public static Answers FromJson(string json) => JsonConvert.DeserializeObject<Answers>(json);
    }

    public class Answer
    {
        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("jsonUrl")]
        public string JsonUrl { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }


    public partial class DetailAnswers
    {
        [JsonProperty("positions")]
        public List<Position> Positions { get; set; }
        public static DetailAnswers FromJson(string json) => JsonConvert.DeserializeObject<DetailAnswers>(json);
    }

    public partial class Position
    {
        [JsonProperty("quantity")]
        public List<string> Quantity { get; set; }

        [JsonProperty("originalPartNumber")]
        public List<string> OriginalPartNumber { get; set; }

        [JsonProperty("link")]
        public object Link { get; set; }

        [JsonProperty("description")]
        public List<string> Description { get; set; }

        [JsonProperty("remark")]
        public List<string> Remark { get; set; }

        [JsonProperty("hotspotIds")]
        public List<int> HotspotIds { get; set; }

        [JsonProperty("vehicleDataAvailable")]
        public bool VehicleDataAvailable { get; set; }

        [JsonProperty("positionId")]
        public long PositionId { get; set; }

        [JsonProperty("expandedPartNumber")]
        public string ExpandedPartNumber { get; set; }

        [JsonProperty("modelInfo")]
        public List<string> ModelInfo { get; set; }

        [JsonProperty("header")]
        public bool Header { get; set; }

        [JsonProperty("partNumber")]
        public string PartNumber { get; set; }

        [JsonProperty("position")]
        public string PositionPosition { get; set; }

        [JsonProperty("idx")]
        public long Idx { get; set; }

        [JsonProperty("fiInvalid")]
        public bool FiInvalid { get; set; }

        [JsonProperty("infoButtonAvailable")]
        public bool InfoButtonAvailable { get; set; }

        [JsonProperty("theOneForHotspot")]
        public bool TheOneForHotspot { get; set; }

        [JsonProperty("deploymentTime")]
        public string DeploymentTime { get; set; }
    }

    // image 
    public partial class ImageParam
    {
        [JsonProperty("pathparams")]
        public Pathparams Pathparams { get; set; }
    }

    public partial class Pathparams
    {
        [JsonProperty("ticket")]
        public string Ticket { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
    public partial class ImageParam
    {
        public static ImageParam FromJson(string json) => JsonConvert.DeserializeObject<ImageParam>(json);
    }
    public partial class ImageInfo
    {
        [JsonProperty("imageWidth")]
        public long ImageWidth { get; set; }

        [JsonProperty("imageHeight")]
        public long ImageHeight { get; set; }

        [JsonProperty("imageFormat")]
        public string ImageFormat { get; set; }

        [JsonProperty("maxScaleFactor")]
        public long MaxScaleFactor { get; set; }

        [JsonProperty("hotspots")]
        public List<Hotspot> Hotspots { get; set; }
    }

    public partial class Hotspot
    {
        [JsonProperty("hsKey")]
        public string HsKey { get; set; }

        [JsonProperty("hsPartNo")]
        public string HsPartNo { get; set; }

        [JsonProperty("hsGroup")]
        public List<object> HsGroup { get; set; }

        [JsonProperty("hsX")]
        public long HsX { get; set; }

        [JsonProperty("hsY")]
        public long HsY { get; set; }

        [JsonProperty("hsWidth")]
        public long HsWidth { get; set; }

        [JsonProperty("hsHeight")]
        public long HsHeight { get; set; }
    }

    public partial class ImageInfo
    {
        public static ImageInfo FromJson(string json) => JsonConvert.DeserializeObject<ImageInfo>(json);
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;

namespace PartslinkModels
{
    public class ModelConfig
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Model { get; set; }
        public string FamilyKey { get; set; } // model key
        public string Year { get; set; } // year key
        public string Restrict1 { get; set; }
        public string RestrictKey { get; set; } // restrict key
        public string Restrict2 { get; set; }
        public string Restrict3 { get; set; }
        // relationship
        //public GroupConfig GroupConfig { get; set; } = new GroupConfig();
    }

    public class GroupConfig
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string MainGroup { get; set; } // GroupKey
        public string GLGR { get; set; }
        public string Ilustration { get; set; }
        public string IlustrationId { get; set; } // Illustrationkey
        public string Name { get; set; }
        public string Notice { get; set; }
        public string Inputs { get; set; }
        // relationship
        public ModelConfig ModelConfig { get; set; } = new ModelConfig();
    }

    public partial class DetailConfing
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        [JsonProperty("positions")]
        public List<Detail> Details { get; set; } = new List<Detail>();
        [JsonProperty("hotspots")]
        public ImageInfo ImageInfo { get; set; } = new ImageInfo();
        public GroupConfig GroupConfig { get; set; } = new GroupConfig();

    }
    public class Detail
    {
        [JsonProperty("quantity")]
        public List<int?> Quantity { get; set; }

        [JsonProperty("originalPartNumber")]
        public List<string> OriginalPartNumber { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("description")]
        public List<string> Description { get; set; }

        [JsonProperty("remark")]
        public List<string> Remark { get; set; }

        [JsonProperty("hotspotIds")]
        public List<int> HotspotIds { get; set; }

        [JsonProperty("vehicleDataAvailable")]
        public bool VehicleDataAvailable { get; set; }

        [JsonProperty("positionId")]
        public int PositionId { get; set; }

        [JsonProperty("expandedPartNumber")]
        public string ExpandedPartNumber { get; set; }

        [JsonProperty("modelInfo")]
        public List<string> ModelInfo { get; set; }

        [JsonProperty("header")]
        public bool Header { get; set; }

        [JsonProperty("partNumber")]
        public string PartNumber { get; set; }

        [JsonProperty("position")]
        public string PositionStr { get; set; }

        [JsonProperty("idx")]
        public int Idx { get; set; }

        [JsonProperty("fiInvalid")]
        public bool FiInvalid { get; set; }

        [JsonProperty("infoButtonAvailable")]
        public bool InfoButtonAvailable { get; set; }

        [JsonProperty("theOneForHotspot")]
        public bool TheOneForHotspot { get; set; }

        [JsonProperty("deploymentTime")]
        public string DeploymentTime { get; set; }
    }
    public partial class ImageInfo
    {
        public string ImageUrl { get; set; }
        [JsonProperty("imageWidth")]
        public int ImageWidth { get; set; }

        [JsonProperty("imageHeight")]
        public int ImageHeight { get; set; }

        [JsonProperty("imageFormat")]
        public string ImageFormat { get; set; }

        [JsonProperty("maxScaleFactor")]
        public int MaxScaleFactor { get; set; }

        [JsonProperty("hotspots")]
        public List<HotSpot> HotSpots { get; set; }
        public static ImageInfo FromJson(string json) => JsonConvert.DeserializeObject<ImageInfo>(json);
    }
    public class HotSpot
    {
        [JsonProperty("hsKey")]
        public string HsKey { get; set; }

        [JsonProperty("hsPartNo")]
        public string HsPartNo { get; set; }

        [JsonProperty("hsGroup")]
        public List<object> HsGroup { get; set; }

        [JsonProperty("hsX")]
        public int HsX { get; set; }

        [JsonProperty("hsY")]
        public int HsY { get; set; }

        [JsonProperty("hsWidth")]
        public int HsWidth { get; set; }

        [JsonProperty("hsHeight")]
        public int HsHeight { get; set; }
    }
}

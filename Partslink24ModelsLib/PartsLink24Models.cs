using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Partslink24Models
{
    public class ModelConfig
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Model { get; set; }
        public string FamilyKey { get; set; }
        public string Year { get; set; }
        public string Restrict1 { get; set; }
        public string RestrictKey { get; set; }
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
        public string IlustrationId { get; set; }
        public string Name { get; set; }
        public string Notice { get; set; }
        public string Inputs { get; set; }
        // relationship
        public ModelConfig ModelConfig { get; set; } = new ModelConfig();
        //public DetailConfing DetailPacks { get; set; } = new DetailConfing();
    }

    public class DetailConfing
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Image { get; set; }
        public string Position { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Notice { get; set; }
        public int? Count { get; set; }
        public string Input { get; set; }
        // relationship
        public GroupConfig GroupConfig { get; set; } = new GroupConfig();
    }
}

﻿using System;
using System.Collections.Generic;

namespace Partslink24ModelsLib
{
    public class ModelConfig
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Restrict1 { get; set; }
        public string Restrict2 { get; set; }
        public string Restrict3 { get; set; }
        // relationship
        public List<GroupConfig> Groups = new List<GroupConfig>();
    }

    public class GroupConfig
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string GLGR { get; set; }
        public string Ilustration { get; set; }
        public string Name { get; set; }
        public string Notice { get; set; }
        public string Inputs { get; set; }
        // relationship
        public int ModelConfigId { get; set; }
        public ModelConfig ModelConfig = new ModelConfig();
        public List<DetailConfing> DetailPacks { get; set; } = new List<DetailConfing>();
    }

    public class DetailConfing
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Position { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Notice { get; set; }
        public int? Count { get; set; }
        public string Input { get; set; }
        // relationship
        public int GroupConfigId { get; set; }
        public GroupConfig GroupConfig { get; set; } = new GroupConfig();
    }
    public class Detail
    {
        public string Position { get; set; }
        public string PartNo { get; set; }
        public string OriginalPartno { get; set; }
        public string Description { get; set; }
        public string ModelDescription { get; set; }
        public int? Quantity { get; set; }
        public string Input { get; set; }
        public string DeploymentTime { get; set; }
        public string NotAvailable { get; set; }
    }
}

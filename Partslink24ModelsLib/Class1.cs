using System;
using System.Collections.Generic;

namespace Partslink24ModelsLib
{
    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Year
    {
        public int Id { get; set; }
        public int Value { get; set; }
    }
    public class restrictI
    {
        public int Id { get; set; }
        public int Value { get; set; }
    }
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Part
    {
        public int Id { get; set; }
        public string GLGR { get; set; }
        public string Ilustration { get; set; }
        public string Name { get; set; }
        public string Notice { get; set; }
        public string Inputs { get; set; }
        public List<DetailPack> DetailPacks { get; set; } = new List<DetailPack>();
    }
    public class DetailPack
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public int PartId { get; set; }
        public Part Part = new Part();
        public List<Detail> Details { get; set; } = new List<Detail>();
    }
    public class Detail
    {
        public int Id { get; set; }
        public int DetailPackId { get; set; }
        public DetailPack DetailPack = new DetailPack();
        public string Position { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Notice { get; set; }
        public int? Count { get; set; }
        public string Input { get; set; }
    }
}

using ParsingLib;
using PartslinkModels;
using System.Collections.Generic;

namespace WebApiPartsLink24.Services.AudiServices
{
    public interface IVehicleService
    {
        public List<Answer> GetModels();
        public List<Answer> GetYears(ModelConfig config);
        public List<Answer> GetRestrict1(ModelConfig config);
        public List<Answer> GetGroups(ModelConfig config);
        public List<GroupConfig> GetParts(GroupConfig config);
        public List<DetailConfing> GetDetails(GroupConfig config);
    }
}

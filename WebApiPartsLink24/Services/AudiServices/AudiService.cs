using ParsingLib;
using PartslinkModels;
using System.Collections.Generic;
using static ConsoleParsingPartsLinks24.ParsingProgram;

namespace WebApiPartsLink24.Services.AudiServices
{
    public class AudiService : IVehicleService
    {
        private static string path = "https://www.partslink24.com/vwag/audi_parts";

        public List<Answer> GetModels()
        {
            string action = "/vehicle.action?";
            string audiUrl = path + action + "lang=ru&localMarketOnly=true&modelYear=1988&restriction1=92389&startup=false&mode=K00U0DEXX&upds=1381";

            ParsingModels newParsingModel = new ParsingModels(audiUrl);
            return newParsingModel.GetAllModels();
        }

        public List<Answer> GetYears(ModelConfig config)
        {
            string action = "/json-model-years.action?";
            string constParams = "lang=ru&localMarketOnly=true&ordinalNumber=2&startup=false&mode=K00U0DEXX&upds=1381";
            string yearsUrl = string.Format(path + action + constParams + "&familyKey={0}", config.FamilyKey);

            ParsingYears newParsingYears = new ParsingYears(yearsUrl);
            return newParsingYears.GetAllModels();
        }

        public List<Answer> GetRestrict1(ModelConfig config)
        {
            string action = "/json-vehicle-restriction1.action?";
            string constParams = "lang=ru&localMarketOnly=true&ordinalNumber=2&startup=false&mode=K00U0DEXX&upds=1381";
            string restrict1Url = string.Format(path + action + constParams + "&familyKey={0}&modelYear={1}", config.FamilyKey, config.Year);

            ParsingRestrictI newParsingRestrict1 = new ParsingRestrictI(restrict1Url);
            return newParsingRestrict1.GetAllModels();
        }

        public List<Answer> GetGroups(ModelConfig config)
        {
            string action = "/group.action?";
            string constParams = "catalogMarket=RDW&episType=152&lang=ru&localMarketOnly=true&ordinalNumber=2&partDetailsMarket=RDW&startup=false&mode=K00U0DEXX&upds=1381";
            string groupsUrl = string.Format(path + action + constParams + "&familyKey={0}&modelYear={1}&restriction1={2}",
                               config.FamilyKey, config.Year, config.Restrict1);
            ParsingGroups newParsingGroups = new ParsingGroups(groupsUrl);
            return newParsingGroups.GetAllModels();
        }

        public List<GroupConfig> GetParts(GroupConfig config)
        {
            string action = "/group.action?";
            string constParams = "catalogMarket=RDW&episType=152&lang=ru&localMarketOnly=true&ordinalNumber=2&partDetailsMarket=RDW&startup=false&mode=K00U0RUXX&upds=1381";
            string partsUrl = string.Format(path + action + constParams + "&familyKey={0}&modelYear={1}&maingroup={2}&restriction1={3}",
                              config.ModelConfig.FamilyKey, config.ModelConfig.Year, config.MainGroup, config.ModelConfig.RestrictKey);
            ParsingPart newParsingGroups = new ParsingPart(partsUrl);
            return newParsingGroups.GetAllModels();
        }

        public List<DetailConfing> GetDetails(GroupConfig config)
        {
            string action = "/image-board.action?";
            string constParams = "catalogMarket=RDW&episType=152&lang=ru&localMarketOnly=true&ordinalNumber=2&partDetailsMarket=RDW&startup=false&mode=K00U0DEXX&upds=1381";
            string detailUrl = string.Format(path + action + constParams + "&familyKey={0}&modelYear={1}&maingroup={2}&restriction1={3}&illustrationId={4}",
                               config.ModelConfig.FamilyKey, config.ModelConfig.Year, config.MainGroup, config.ModelConfig.RestrictKey, config.IlustrationId);
            ParsingDetail newParsingGroups = new ParsingDetail(detailUrl);
            return newParsingGroups.GetAllModels();
        }
    }
}

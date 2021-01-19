using Microsoft.AspNetCore.Mvc;
using static ConsoleParsingPartsLinks24.ParsingProgram;
using PartslinkModels;

namespace WebApiPartsLink24
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartLinkController : ControllerBase
    {
        static string domain = "https://www.partslink24.com";
        static string path = domain + "/vwag/audi_parts";

        [HttpGet]
        public ActionResult GetAudiModels()
        {
            string action = "/vehicle.action?";
            string audiUrl = path + action + "lang=ru&localMarketOnly=true&modelYear=1988&restriction1=92389&startup=false&mode=K00U0DEXX&upds=1381";
            ParsingModels newParsingModel = new ParsingModels(new ModelConfig(), audiUrl);
            return Ok(newParsingModel.GetAllModels());
        }
        [HttpPost("years")]
        public ActionResult GetYears(ModelConfig config)
        {
            string action = "/json-model-years.action?";
            string constParams = "lang=ru&localMarketOnly=true&ordinalNumber=2&startup=false&mode=K00U0DEXX&upds=1381";
            string yearsUrl = string.Format(path + action + constParams + "&familyKey={0}", config.FamilyKey);
            ParsingYears newParsingYears = new ParsingYears(config, yearsUrl);
            return Ok(newParsingYears.GetAllModels());
        }
        [HttpPost("restricts1")]
        public ActionResult GetRestrictI(ModelConfig config)
        {
            string action = "/json-vehicle-restriction1.action?";
            string constParams = "lang=ru&localMarketOnly=true&ordinalNumber=2&startup=false&mode=K00U0DEXX&upds=1381";
            string restrict1Url = string.Format(path + action + constParams + "&familyKey={0}&modelYear={1}", config.FamilyKey, config.Year);
            ParsingRestrictI newParsingRestrict1 = new ParsingRestrictI(config, restrict1Url);
            return Ok(newParsingRestrict1.GetAllModels());
        }
        [HttpPost("groups")]
        public ActionResult GetGroups(ModelConfig config)
        {
            string action = "/group.action?";
            string constParams = "catalogMarket=RDW&episType=152&lang=ru&localMarketOnly=true&ordinalNumber=2&partDetailsMarket=RDW&startup=false&mode=K00U0DEXX&upds=1381";
            string groupsUrl = string.Format(path + action + constParams + "&familyKey={0}&modelYear={1}&restriction1={2}",
                               config.FamilyKey, config.Year, config.Restrict1);
            ParsingGroups newParsingGroups = new ParsingGroups(config, groupsUrl);
            return Ok(newParsingGroups.GetAllModels());
        }
        [HttpPost("parts")]
        public ActionResult GetParts(GroupConfig config)
        {
            string action = "/group.action?";
            string constParams = "catalogMarket=RDW&episType=152&lang=ru&localMarketOnly=true&ordinalNumber=2&partDetailsMarket=RDW&startup=false&mode=K00U0RUXX&upds=1381";
            string partsUrl = string.Format(path + action + constParams + "&familyKey={0}&modelYear={1}&maingroup={2}&restriction1={3}",
                              config.ModelConfig.FamilyKey, config.ModelConfig.Year, config.MainGroup, config.ModelConfig.RestrictKey);
            ParsingPart newParsingGroups = new ParsingPart(config, partsUrl);
            return Ok(newParsingGroups.GetAllModels());            
        }
        [HttpPost("details")]
        public ActionResult GetDetails(GroupConfig config)
        {
            string action = "/image-board.action?";
            string constParams = "catalogMarket=RDW&episType=152&lang=ru&localMarketOnly=true&ordinalNumber=2&partDetailsMarket=RDW&startup=false&mode=K00U0DEXX&upds=1381";
            string detailUrl = string.Format(path + action + constParams + "&familyKey={0}&modelYear={1}&maingroup={2}&restriction1={3}&illustrationId={4}",
                               config.ModelConfig.FamilyKey, config.ModelConfig.Year, config.MainGroup, config.ModelConfig.RestrictKey, config.IlustrationId);
            ParsingDetail newParsingGroups = new ParsingDetail(config, detailUrl);
            return Ok(newParsingGroups.GetAllModels());
        }
    }
}

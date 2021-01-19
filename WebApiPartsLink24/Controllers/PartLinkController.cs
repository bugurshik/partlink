using Microsoft.AspNetCore.Mvc;
using ConsoleParsingPartsLinks24;
using Partslink24Models;
using Microsoft.AspNetCore.Http;

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
            var newParsingModel = new ParsingProgram.ParsingModels(new ModelConfig(), audiUrl);
            return Ok(newParsingModel.GetAll());
        }
        [HttpPost("years")]
        public ActionResult GetYears(ModelConfig config)
        {
            string action = "/json-model-years.action?";
            string constParams = "lang=ru&localMarketOnly=true&ordinalNumber=2&startup=false&mode=K00U0DEXX&upds=1381";
            string yearsUrl = string.Format(path + action + constParams + "&familyKey={0}", config.FamilyKey);
            var newParsingYears = new ParsingProgram.Years(config, yearsUrl);
            return Ok(newParsingYears.GetAll());
        }
        [HttpPost("restricts1")]
        public ActionResult GetRestrictI(ModelConfig config)
        {
            string action = "/json-vehicle-restriction1.action?";
            string constParams = "lang=ru&localMarketOnly=true&ordinalNumber=2&startup=false&mode=K00U0DEXX&upds=1381";
            string restrict1Url = string.Format(path + action + constParams + "&familyKey={0}&modelYear={1}", config.FamilyKey, config.Year);
            var newParsingRestrict1 = new ParsingProgram.RestrictI(config, restrict1Url);
            return Ok(newParsingRestrict1.GetAll());
        }
        [HttpPost("groups")]
        public ActionResult GetGroups(ModelConfig config)
        {
            string action = "/group.action?";
            string constParams = "catalogMarket=RDW&episType=152&lang=ru&localMarketOnly=true&ordinalNumber=2&partDetailsMarket=RDW&startup=false&mode=K00U0DEXX&upds=1381";
            string groupsUrl = string.Format(path + action + constParams + "&familyKey={0}&modelYear={1}&restriction1={2}",
                               config.FamilyKey, config.Year, config.Restrict1);
            var newParsingGroups = new ParsingProgram.Groups(config, groupsUrl);
            return Ok(newParsingGroups.GetAll());
        }
        [HttpPost("parts")]
        public ActionResult GetParts(GroupConfig config)
        {
            string action = "/group.action?";
            string constParams = "catalogMarket=RDW&episType=152&lang=ru&localMarketOnly=true&ordinalNumber=2&partDetailsMarket=RDW&startup=false&mode=K00U0RUXX&upds=1381";
            string partsUrl = string.Format(path + action + constParams + "&familyKey={0}&modelYear={1}&maingroup={2}&restriction1={3}",
                              config.ModelConfig.FamilyKey, config.ModelConfig.Year, config.MainGroup, config.ModelConfig.RestrictKey);
            var newParsingGroups = new ParsingProgram.Part(config, partsUrl);
            return Ok(newParsingGroups.GetAll());
        }
        [HttpPost("details")]
        public ActionResult GetDetails(GroupConfig config)
        {
            string action = "/image-board.action?";
            string constParams = "catalogMarket=RDW&episType=152&lang=ru&localMarketOnly=true&ordinalNumber=2&partDetailsMarket=RDW&startup=false&mode=K00U0DEXX&upds=1381";
            string detailUrl = string.Format(path + action + constParams + "&familyKey={0}&modelYear={1}&maingroup={2}&restriction1={3}&illustrationId={4}",
                               config.ModelConfig.FamilyKey, config.ModelConfig.Year, config.MainGroup, config.ModelConfig.RestrictKey, config.IlustrationId);
            var newParsingGroups = new ParsingProgram.Detail(config, detailUrl);
            return Ok(newParsingGroups.GetAll());
        }
    }
}

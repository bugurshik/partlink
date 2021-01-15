using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleParsingPartsLinks24;
using Partslink24Models;

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
            var action = "/vehicle.action?";
            var audiUrl = path + action + "lang=ru&localMarketOnly=true&modelYear=1988&restriction1=92389&startup=false&mode=K00U0DEXX&upds=1381";
            var models = ParsingProgram.Models.GetAll(audiUrl);

            if (models != null)
                return Ok(models);
            return NotFound("models not found");
        }
        [HttpPost("years")]
        public ActionResult GetYears(ModelConfig config)
        {
            var action = "/json-model-years.action?";
            var yearsUrl = string.Format(path + action + "familyKey={0}&lang=ru&localMarketOnly=true&ordinalNumber=2&startup=false&mode=K00U0DEXX&upds=1381", config.FamilyKey);
            return Ok(ParsingProgram.Years.GetAll(config, yearsUrl));
        }
        [HttpPost("restricts1")]
        public ActionResult GetRestrictI(ModelConfig config)
        {
            var action = "/json-vehicle-restriction1.action?";
            var restrict1Url = string.Format(path + action + "familyKey={0}&lang=ru&localMarketOnly=true&modelYear={1}&ordinalNumber=2&startup=false&mode=K00U0DEXX&upds=1381", config.FamilyKey, config.Year);
            
            return Ok(ParsingProgram.RestrictI.GetAll(config, restrict1Url));
        }
        [HttpPost("groups")]
        public ActionResult GetGroups()
        {
            var action = "/group.action?";
            var groupsUrl = string.Format(path + action + "catalogMarket=RDW&episType=152&familyKey=91155&lang=ru&localMarketOnly=true&modelYear=1998&ordinalNumber=2&partDetailsMarket=RDW&restriction1=92362&startup=false&mode=K00U0DEXX&upds=1381");
            return Ok();
        }
    }
}

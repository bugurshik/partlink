using Microsoft.AspNetCore.Mvc;
using PartslinkModels;
using WebApiPartsLink24.Services.AudiServices;
using ParsingLib;

namespace WebApiPartsLink24
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartLinkController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        public PartLinkController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public ActionResult GetAudiModels()
        {
            var responce = _vehicleService.GetModels();
            if (responce == null)
                return NotFound();
            return Ok(responce);
        }

        [HttpPost("years")]
        public ActionResult GetYears(ModelConfig config)
        {
            var responce = _vehicleService.GetYears(config);
            if (responce.Count == 0)
                return NotFound();
            return Ok(responce);
        }

        [HttpPost("restricts1")]
        public ActionResult GetRestrictI(ModelConfig config)
        {
            var responce = _vehicleService.GetRestrict1(config);
            if (responce.Count == 0)
                return NotFound();
            return Ok(responce);
        }

        [HttpPost("group")]
        public ActionResult GetGroups(ModelConfig config)
        {
            var responce = _vehicleService.GetGroups(config);
            if (responce.Count == 0)
                return NotFound();
            return Ok(responce);
        }

        [HttpPost("parts")]
        public ActionResult GetParts(GroupConfig config)
        {
            var responce = _vehicleService.GetParts(config);
            if (responce.Count == 0)
                return NotFound();
            return Ok(responce);          
        }
        [HttpPost("details")]
        public ActionResult GetDetails(GroupConfig config)
        {
            var responce = _vehicleService.GetDetails(config);
            if (responce.Count == 0)
                return NotFound();
            return Ok(responce);
        }
    }
}

using login.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Platform.ReferencialData.Business.business_services.Authentification;
using Platform.ReferentialData.DataModel.Authentification;
using Platform.ReferentialData.DtoModel.Authentification;

namespace Platform.ReferencialData.WebAPI.Controllers.Authentification
{

    [ApiController]
    [Route("[controller]")]

    public class shopownerController : ControllerBase
    {
        private readonly UserManager<employer3> _usermanager;
        private readonly RoleManager<AspRoles> _RoleManager;
        private readonly IConfiguration _configuration;
        private readonly IshopownerService _shopownerservice;
        public shopownerController(UserManager<employer3> _usermanager, RoleManager<AspRoles> _RoleManager, IConfiguration _configuration, IshopownerService shopownerService)
        {
            this._usermanager = _usermanager;
            this._configuration = _configuration;
            this._RoleManager = _RoleManager;
            _shopownerservice = shopownerService;
        }

        [HttpPost]
        [Route("addDemandePayement")]
        public  Task addDemandePayement (demandePayementDto payement)
        {
            return  _shopownerservice.addDemandePayement(payement);
        }
        [HttpGet]
        [Route("getAllDemandePayement")]
        public async Task<List<Object>> getAllDemandePayement()
        {
            return await _shopownerservice.getAllDemandePayement();
        }

        [HttpGet]
        [Route("getDemandePayementByshopowner")]
        public List<demandePayement> getDemandePayementByshopowner(string id)
        {
            return _shopownerservice.getDemandePayementByshopowner(id);
        }


     
    }
}

using login.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Platform.ReferencialData.Business.business_services.Authentification;
using Platform.ReferencialData.Business.business_services_implementations.Authentification;
using Platform.ReferentialData.DataModel.Authentification;
using Platform.ReferentialData.DtoModel.Authentification;

namespace Platform.ReferencialData.WebAPI.Controllers.Authentification
{
    [ApiController]
    [Route("[controller]")]
    public class DemandePayementController : ControllerBase
    {

        private readonly UserManager<employer3> _usermanager;
        private readonly RoleManager<AspRoles> _RoleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeManagementService _IEmployeeManagementService;

        public DemandePayementController(UserManager<employer3> _usermanager, RoleManager<AspRoles> _RoleManager, IConfiguration _configuration, IEmployeeManagementService _IEmployeeManagementService)
        {
            this._usermanager = _usermanager;
            this._configuration = _configuration;
            this._RoleManager = _RoleManager;
            this._IEmployeeManagementService = _IEmployeeManagementService;
        }




        [HttpGet]
        [Route("getDemandePayementByShopowner")]
        public async Task<List<demandePayement>> getDemandePayementByShopowner(string Id)
        {
            return await _IEmployeeManagementService.getDemandePayementByShopowner(Id);
        }


        [HttpGet]
        [Route("getDemandePayementById")]
        public  List<demandePayement> getDemandePayementById(string Id)
        {
            return  _IEmployeeManagementService.getDemandePayementById(Id);
        }

        [HttpGet]
        [Route("getDemandesPayement")]
        public async Task<List<Object>> getDemandesPayement()
        {
            return await _IEmployeeManagementService.getDemandePayement();

        }
        [HttpPost]
        [Route("DemandeTransaction")]
        public async Task addDemandeTransaction(demandePayementDto demande)
        {
            await _IEmployeeManagementService.addDemandePayement(demande);
        }
       

        [HttpPut]
        [Route("UpdateDemandePayement")]
        public async Task<IActionResult> updateDemandePayement(string id, string value,string idval)
        {
            var result =await _IEmployeeManagementService.updateDemandePayement(id, value,idval);
            if (result)
                return Ok("demandeTransaction modifier");
            else
                return BadRequest("echec de la modification");
        }

    }
}

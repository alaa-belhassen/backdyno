using login.models;
using Microsoft.AspNetCore.Http;
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

    public class DemandeTransactionController : ControllerBase
    {
        private readonly UserManager<employer3> _usermanager;
        private readonly RoleManager<AspRoles> _RoleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeManagementService _IEmployeeManagementService;
        public DemandeTransactionController(UserManager<employer3> _usermanager, RoleManager<AspRoles> _RoleManager, IConfiguration _configuration, IEmployeeManagementService _IEmployeeManagementService)
        {
            this._usermanager = _usermanager;
            this._configuration = _configuration;
            this._RoleManager = _RoleManager;
            this._IEmployeeManagementService = _IEmployeeManagementService;
        }


        [HttpGet]
        [Route("getDemandeTransactionByEmployer")]
        public async Task<List<demandeTransaction>> getDemandeTransactionByEmployer(string Id)
        {
            return await _IEmployeeManagementService.getDemandeTransactionByEmployer(Id);
        }
        [HttpGet]
        [Route("getDemandesTransaction")]
        public async Task<List<Object>> getDemandeTransaction()
        {
            return await _IEmployeeManagementService.getDemandeTransaction();

        }
        [HttpPost]
        [Route("DemandeTransaction")]
        public async Task addDemandeTransaction(demandeTransactionDto demande)
        {
            await _IEmployeeManagementService.addDemandeTransaction(demande);
        }
        [HttpDelete]
        [Route("deleteDemandeTransaction")]
        public async Task<IActionResult> deleteDemandeTransaction(string id)
        {
            var result =  _IEmployeeManagementService.deleteDemandeTransaction(id);
            if (result)
                return Ok("demandeTransaction supprimer");
            else
                return BadRequest("echec de la suppression");
        }

        [HttpPut]
        [Route("UpdateDemandeTransaction")]
        public async Task<IActionResult> updateDemandeTransaction(string id,string value)
        {
            var result = _IEmployeeManagementService.updateDemandeTransaction(id,value);
            if (result)
                return Ok("demandeTransaction modifier");
            else
                return BadRequest("echec de la modification");
        }
    }
}

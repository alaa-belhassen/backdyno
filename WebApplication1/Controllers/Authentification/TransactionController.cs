using login.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Platform.ReferencialData.Business.business_services.Authentification;
using Platform.ReferentialData.DataModel.Authentification;
using Platform.ReferentialData.DtoModel.Authentification;

namespace Platform.ReferencialData.WebAPI.Controllers.Authentification
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {

        private readonly UserManager<employer3> _usermanager;
        private readonly RoleManager<AspRoles> _RoleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeManagementService _IEmployeeManagementService;
        public TransactionController(UserManager<employer3> _usermanager, RoleManager<AspRoles> _RoleManager, IConfiguration _configuration, IEmployeeManagementService _IEmployeeManagementService)
        {
            this._usermanager = _usermanager;
            this._configuration = _configuration;
            this._RoleManager = _RoleManager;
            this._IEmployeeManagementService = _IEmployeeManagementService;
        }
        
        [HttpPost]
        [Route("addTransactions")]
        public async Task<IActionResult> addTransactions(TransactionDto id)
        {
            var result = await _IEmployeeManagementService.addTransaction(id);
            if (result)
                return Ok("Transaction ajouter ");
            else
                return BadRequest("echec de l' ajout");
        }
    }
}

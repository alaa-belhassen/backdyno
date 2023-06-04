using login.models;
using Microsoft.AspNetCore.Mvc;
using Platform.ReferencialData.Business.business_services.Authentification;
using Microsoft.AspNetCore.Identity;
using Platform.ReferentialData.DataModel.Authentification;
using Platform.ReferentialData.DtoModel.Authentification;
using Platform.ReferencialData.Business.Helper;
using TunisieSMS;
namespace Platform.ReferencialData.WebAPI.Controllers.Authentification
{
    [ApiController]
    [Route("[controller]")]
    public class accountController : ControllerBase
    {
        private readonly UserManager<employer3> _usermanager;
        private readonly RoleManager<AspRoles> _RoleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeManagementService _employee;

        public accountController(UserManager<employer3> _usermanager, RoleManager<AspRoles> _RoleManager, IConfiguration _configuration, IEmployeeManagementService _employee)
        {
            this._usermanager = _usermanager;
            this._configuration = _configuration;
            this._RoleManager = _RoleManager;
            this._employee = _employee;
        }

        [HttpPost]
        [Route("addAccount")]
        public async Task<IActionResult> addAccount(string secret,string mail)
        {
            if ( secret == null || mail == null)
            {
                return BadRequest("valeur entrer inexistante");
            }
            var existinguser =await _usermanager.FindByEmailAsync(mail);
            if ( existinguser == null)
            {
                return BadRequest("iduser incorrect");
            }
            _employee.addaccount(secret, existinguser.Id);  
            return Ok("account added");
        }

        [HttpPost]
        [Route("GetAccountNames")]
        public async Task<List<employer3>> GetAccountNames(List<string> list)
        {
          return await  _employee.getaccountName(list);
        }


        [HttpGet]
        [Route("getAccountByID")]
        public async Task<IActionResult> getAccount(string id)
        {
            if (id == null )
            {
                return BadRequest("valeur entrer inexistante");
            }
            var existinguser = await _usermanager.FindByIdAsync(id);
            if (existinguser == null)
            {
                return BadRequest("iduser incorrect");
            }
            var account = _employee.getAccount(id);
            return Ok(account);
        }

        [HttpPost]
        [Route("sendMessage")]
        public async Task<IActionResult> sendMessage(sendMessageDto message)
        {
            if (message == null)
            {
                return BadRequest("valeur entrer inexistante");
            }
            TunisieSmsService smsService= new TunisieSmsService();
            return Ok(smsService.sendmessage(message.numero, message.Message));
            
        }



        

    }
}

using login.data.PostgresConn;
using login.models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.ReferencialData.Business.business_services.Authentification;
using Platform.ReferentialData.DataModel.Authentification;
using Platform.ReferentialData.DtoModel.Authentification;
using System.Security.Claims;

namespace Platform.ReferencialData.WebAPI.Controllers.Authentification
{
    [ApiController]
    [Route("[Controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly Postgres _context;
        private readonly UserManager<employer3> _usermanager;
        private readonly RoleManager<AspRoles> _RoleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeManagementService _IEmployeeManagementService;
        public EmployeeController(UserManager<employer3> _usermanager, Postgres context, RoleManager<AspRoles> _RoleManager, IConfiguration _configuration, IEmployeeManagementService _IEmployeeManagementService)
        {
            this._usermanager = _usermanager;
            this._configuration = _configuration;
            this._RoleManager = _RoleManager;
            _context = context;
            this._IEmployeeManagementService = _IEmployeeManagementService;
        }



        [HttpGet]
        [Route("GetRolesByUserId")]
        public async Task<IList<string>> GetRolesByUserId(string id)
        {
            var result = await _IEmployeeManagementService.getRolesByUserId(id);
        
            return result;
        }




       

        [HttpPost]
        [Route("CreateRole")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer")]
        public async Task<IActionResult> CreateRole(EmployeeRoleRequestDto role)
        {
            if (ModelState.IsValid)
            {
                var Role = new RoleRequestDto()
                {
                    RoleName = role.RoleName,
                    idEmployer = role.idEmployer
                };
                var employer_exist = await _context.Employer.FirstOrDefaultAsync(e => e.moreInfoId == role.idEmployer);
              
                if (employer_exist == null)
                {
                    return BadRequest("Employer n'existe pas exist");

                }
                var result = await _IEmployeeManagementService.AddRole(Role);
                if (!result)
                {
                    return BadRequest("echec de l'ajout");
                }
                var existing_Role = await _RoleManager.FindByNameAsync(role.RoleName);
                if (existing_Role == null)
                {
                    return BadRequest("echec de l'ajout");
                }
                var is_added = await _IEmployeeManagementService.addClaims(existing_Role,role.Permission);
                if (!is_added)
                {
                    return BadRequest("Permission are not added");
                }
                return Ok($"{role.RoleName} is added");
            }

            return BadRequest();
        }

       
        [HttpPut]
        [Route("updatePermission")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer")]
        public async Task<IActionResult> updatePermission(EmployeeRoleRequestDto role)
        {
            
            if (ModelState.IsValid)
            { 

                var existing_Role = await _RoleManager.FindByNameAsync(role.RoleName);
                if (existing_Role == null)
                {
                    return BadRequest("role does not exist");
                }

                var Claims = await _RoleManager.GetClaimsAsync(existing_Role);
                List<Claim> newList = new List<Claim>();

                foreach(var claim in Claims)
                {
                    await _RoleManager.RemoveClaimAsync(existing_Role,claim);
                }

                var is_added = await _IEmployeeManagementService.addClaims(existing_Role, role.Permission);
                if (!is_added)
                {
                    return BadRequest("Permission are not added");
                }
                return Ok($"{role.RoleName} is updated");
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("DeletePermission")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer")]
        public async Task<IActionResult> DeletePermission(string role)
        {

            if (ModelState.IsValid)
            {

                var existing_Role = await _RoleManager.FindByNameAsync(role);
                if (existing_Role == null)
                {
                    return BadRequest("role does not exist");
                }

                var is_deleted = await _RoleManager.DeleteAsync(existing_Role);
                if (is_deleted.Succeeded)
                {
                    return Ok("deleted succesfully");
                }
                else
                    return BadRequest("failed to delete");

              
            }

            return BadRequest();
        }


        [HttpGet]
        [Route("createManagerRH")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "createManagerRH")]
        public async Task<IActionResult> createManagerRH()
        {
            if (ModelState.IsValid)
            {

            }
            return Ok();
        }

        [HttpGet]
        [Route("createManagerFacturation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "createManagerFacturation")]
        public async Task<IActionResult> createManagerFacturation()
        {
            if (ModelState.IsValid)
            {

            }
            return Ok();

        }

        [HttpGet]
        [Route("createEmployee")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "createEmployee")]
        public async Task<IActionResult> createEmployee()
        {
            if (ModelState.IsValid)
            {

            }
            return Ok();
        }

        [HttpGet]
        [Route("refillAllWallet")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "refillAllWallet")]
        public async Task<IActionResult> refillAllWallet()
        {
            if (ModelState.IsValid)
            {

            }
            return Ok();
        }
        [HttpGet]
        [Route("refillWallet")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "refillWallet")]
        public async Task<IActionResult> refillWallet()
        {
            if (ModelState.IsValid)
            {

            }
            return Ok();
        }
        [HttpGet]
        [Route("accessHistorieFromEmployerToEmployee")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "accessHistorieFromEmployerToEmployee")]
        public async Task<IActionResult> accessHistorieFromEmployerToEmployee()
        {
            if (ModelState.IsValid)
            {

            }
            return Ok();
        }
        [HttpGet]
        [Route("accessHistorieFromDynoToEmployer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "accessHistorieFromDynoToEmployer")]
        public async Task<IActionResult> accessHistorieFromDynoToEmployer()
        {
            if (ModelState.IsValid)
            {

            }
            return Ok();
        }

        [HttpGet]
        [Route("refillEmployerWallet")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "refillEmployerWallet")]
        public async Task<IActionResult> refillEmployerWallet()
        {
            if (ModelState.IsValid)
            {

            }
            return Ok();
        }

    }
}

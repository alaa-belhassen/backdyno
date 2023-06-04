using login.data.PostgresConn;
using login.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.ReferencialData.Business.business_services.Authentification;
using Platform.ReferentialData.DataModel.Authentification;
using Platform.ReferentialData.DtoModel.Authentification;

namespace Platform.ReferencialData.WebAPI.Controllers.Authentification
{
    [ApiController]
    [Route("[controller]")]

    public class EmploymentController : ControllerBase
    {
        private readonly UserManager<employer3> _usermanager;
        private readonly RoleManager<AspRoles> _RoleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeManagementService _IEmployeeManagementService;
        private readonly Postgres _context;

        public EmploymentController(UserManager<employer3> _usermanager, Postgres _context, RoleManager<AspRoles> _RoleManager, IConfiguration _configuration, IEmployeeManagementService _IEmployeeManagementService)
        {
            this._usermanager = _usermanager;
            this._configuration = _configuration;
            this._RoleManager = _RoleManager;
            this._IEmployeeManagementService = _IEmployeeManagementService;
            this._context = _context;
        }

        [HttpPost]
        [Route("AddEmployment")]
        public async Task<IActionResult> AddEmployment(EmployDto employ)
        {
           
                var userexist = await _usermanager.FindByEmailAsync(employ.mailEmployee);
                if (userexist != null)
                {
                    var employee = await _context.Employee.FirstOrDefaultAsync(x => x.moreInfoId == userexist.Id);
                    if (employee != null) {

                    var exist = _context.Employement.FirstOrDefault(x => x.IdEmployer == employ.IdEmployer && x.IdEmployee == employee.moreInfoId && x.IdCategorie == x.IdCategorie);
                    if (exist == null)
                    {
                        var Results = await AddToPost(employ.mailEmployee, employ.Role);
                        if (Results == true)
                        {

                            var result = await _IEmployeeManagementService.addEmployment(employ);
                            if (result.Result == false)
                            {
                                return BadRequest(result);

                            }
                            return Ok(result);
                        }
                        else
                        {
                            return BadRequest("Role existe deja");
                        }
                    }
                    else
                        return BadRequest("utilisateur travaille deja dans cette société");
                }
                    else
                       return BadRequest("utilisateur n'existe pas");
                
                }

                return BadRequest("utilisateur n'existe pas");


        }

       
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer")]
        private async Task<bool> AddToPost(string email, string role)
        {
            if ((email != null) && (role != null))
            {
                var Role_exist = await _RoleManager.RoleExistsAsync(role);
                if (!Role_exist)
                {
                    return false;

                }
                var existing_user = await _usermanager.FindByEmailAsync(email);
                var is_created = await _usermanager.AddToRoleAsync(existing_user, role);
                if (!is_created.Succeeded)
                {
                    return false;
                }

                return true;

            }
            return false;

        }

        [HttpPost]
        [Route("AddPermissionPost")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer")]
        public async Task<IActionResult> AddPermissionPost(AddEmployementRoleDto employ)
        {

            if (employ != null)
            {
                await _IEmployeeManagementService.addEmployment(employ);

                var Role_exist = await _RoleManager.RoleExistsAsync(employ.role);
                if (!Role_exist)
                {
                    return BadRequest("role does noot exist ");
                }
                var existing_user = await _usermanager.FindByEmailAsync(employ.mailEmployee);
                var is_created = await _usermanager.AddToRoleAsync(existing_user, employ.role);
                if (!is_created.Succeeded)
                {
                    return BadRequest(is_created.Errors);
                }
                return Ok($"{employ.role} added for {employ.mailEmployee}");
            }
            return BadRequest("data is wrong");

        }

        [HttpGet]
        [Route("GetEmployement")]
        public async Task<List<Object>> getEmploymentById(string id)
        {
            return  await _IEmployeeManagementService.getEmployementById(id);
        }

        [HttpDelete]
        [Route("FireEmployee")]
        public async Task FireEmployee(deleteEmploymentDto employ)
        {
             await _IEmployeeManagementService.FireEmployee(employ);
        }

        [HttpPost]
        [Route("addPayement")]
        public async Task<IActionResult> addPayement(PayementDto payement)
        {
            var result = _IEmployeeManagementService.addPayement(payement);
            if (result.Result == true)
            {
                return Ok("payement ajouter ");
            }
            else
                return BadRequest("echecde l'ajout");
        }

        [HttpPost]
        [Route("modifierEmployement")]
        public async Task<IActionResult> modifCategorie(UpdateEmployementDto payement)
        {
            var result =await _IEmployeeManagementService.updateEmployement(payement);
            if (result.Result == true)
            {
                return Ok(result);
            }
            else
                return BadRequest(result);
        }


        [HttpPost]
        [Route("getEmployeePayement")]
        public async Task<IActionResult> getEmployeePayement(string id,string idsociete)
        {
            var payement = _context.Payement.Where(x => x.idEmployer == id && x.idsociété == idsociete).ToList();
            if (payement != null)
            {
                return Ok(payement);
            }
            else
                return BadRequest("null");
        }
    }
}

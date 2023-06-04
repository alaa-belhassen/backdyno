using login.data.PostgresConn;
using login.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Platform.ReferencialData.Business.business_services.Authentification;
using Platform.ReferentialData.DataModel.Authentification;
using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using WebApplication1.models;

namespace Platform.ReferencialData.WebAPI.Controllers.Authentification

{
    [ApiController]
    [Route("[controller]")]
    public class ManageUsersController : ControllerBase
    {
        private readonly UserManager<employer3> _usermanager;
        private readonly RoleManager<AspRoles> _RoleManager;
        private readonly IConfiguration _configuration;
        private readonly IAuthentificationService _authentificationService;
        private readonly Postgres _context;
        public ManageUsersController(UserManager<employer3> _usermanager, RoleManager<AspRoles> _RoleManager, IConfiguration _configuration, IAuthentificationService _authentificationService, Postgres _context)
        {   
            this._usermanager = _usermanager;
            this._configuration = _configuration;
            this._RoleManager = _RoleManager;
            this._authentificationService = _authentificationService;
            this._context = _context;
        }
        [HttpDelete]
        [Route("BlockEmployer")]
        public  IActionResult BlockEmployer(string id)
        {
            if ( id != null)
            {
                var foundEmployer = _context.Employer.FirstOrDefault(x => x.moreInfoId == id);
              
                if (foundEmployer != null )
                {
                    if (foundEmployer.Blocked != "null")
                    {
                        return BadRequest("Account Already Blocked");
                    }
                    foundEmployer.Blocked = DateTime.UtcNow.ToString();
                    _context.Employer.UpdateRange(foundEmployer);
                    _context.SaveChanges();
                  return  Ok("Account Blocked");
                }
               return BadRequest("Account Notfound");
            }
           return BadRequest("Account Notfound");

        }

        [HttpDelete]
        [Route("BlockShopowner")]
        public IActionResult BlockShopowner(string id)
        {
            if (id != null)
            {
                var foundShopower = _context.Shopowner.FirstOrDefault(x => x.moreInfoId == id);

                if (foundShopower != null)
                {
                    if (foundShopower.Blocked != "null")
                    {
                        return BadRequest("Account Already Blocked");
                    }
                    foundShopower.Blocked = DateTime.UtcNow.ToString();
                    _context.Shopowner.UpdateRange(foundShopower);
                    _context.SaveChanges();
                    return Ok("Account Blocked");
                }
                return BadRequest("Account Notfound");
            }
            return BadRequest("Account Notfound");


        }


        [HttpDelete]
        [Route("activateEmployee")]
        public IActionResult activateEmployee(string id)
        {
            if (id != null)
            {
                var foundShopower = _context.Employee.FirstOrDefault(x => x.moreInfoId == id );

                if (foundShopower != null)
                {
                    if (foundShopower.activated == true )
                    {
                        return BadRequest("Account Already activated");
                    }
                    foundShopower.activated = true;
                    _context.Employee.UpdateRange(foundShopower);
                    _context.SaveChanges();
                    return Ok("Account activated");
                }
                return BadRequest("Account Notfound");
            }
            return BadRequest("Account Notfound");


        }


        [HttpPut]
        [Route("unBlockEmployer")]
        public IActionResult unBlockEmployer(string id)
        {
            if (id != null)
            {
                var foundEmployer = _context.Employer.FirstOrDefault(x => x.moreInfoId == id);

                if (foundEmployer != null)
                {
                    if (foundEmployer.Blocked == "null")
                    {
                        return BadRequest("Account not Blocked");
                    }
                    foundEmployer.Blocked = "null";
                    _context.Employer.UpdateRange(foundEmployer);
                    _context.SaveChanges();
                    return Ok("Account unBlocked");
                }
                return BadRequest("Account Notfound");
            }
            return BadRequest("Account Notfound");

        }


        [HttpPut]
        [Route("unBlockShopowner")]
        public IActionResult unBlockShopowner(string id)
        {
            if (id != null)
            {
                var foundShopower = _context.Shopowner.FirstOrDefault(x => x.moreInfoId == id);

                if (foundShopower != null)
                {
                    if (foundShopower.Blocked == "null")
                    {
                        return BadRequest("Account not Blocked");
                    }
                    foundShopower.Blocked = "null";
                    _context.Shopowner.UpdateRange(foundShopower);
                    _context.SaveChanges();
                    return Ok("Account unBlocked");
                }
                return BadRequest("Account Notfound");
            }
            return BadRequest("Account Notfound");

        }


        [HttpDelete]
        [Route("deleteEmployer")]
        public async Task DeleteEmployer(string email)
        {
            if (email != null)
            {
                
                var user = await _usermanager.FindByEmailAsync(email);
                if (user != null)
                {
                    BadRequest("user is null");
                }
                var FoundEmployer = await _context.Employer.FirstOrDefaultAsync(x => x.moreInfoId == user.Id);
                _context.Employer.Remove(FoundEmployer);
                _context.SaveChanges();
                await _usermanager.DeleteAsync(user);

            }

        }

    

        [HttpDelete]
        [Route("deleteShopowner")]
        public async Task DeleteShopowner(string email)
        {
            if (email != null)
            {
                var user = await _usermanager.FindByEmailAsync(email);
                if (user != null)
                {
                    BadRequest("user is null");
                }
                var FoundEmployer = await _context.Shopowner.FirstOrDefaultAsync(x => x.moreInfoId == user.Id);
                _context.Shopowner.Remove(FoundEmployer);
                _context.SaveChanges();
                await _usermanager.DeleteAsync(user);

            }

        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<List<Object>> GetUsers(string role,string type)
        {

            var UsersWithRole = await _usermanager.GetUsersInRoleAsync(role);
            List<Object> result = new List<Object>();
            foreach (var user in UsersWithRole)
            {
                var FoundEmployer = new MoreInfoEmployer();
                var FoundShopowner = new ShopOwner();
                var FoundEmployee = new Employee();
                if (role == "employer")
                {
                    if(type == "all")
                    {
                        FoundEmployer = _context.Employer.AsNoTracking().FirstOrDefault(x => x.moreInfoId == user.Id );

                    }
                    else
                    {
                        FoundEmployer = _context.Employer.AsNoTracking().FirstOrDefault(x => x.moreInfoId == user.Id && x.Blocked == "null");

                    }
                    if (FoundEmployer!=null)
                    { 
                    var FoundDemandes =  _context.DemandeTransaction.Where(x => x.IdEmployer == FoundEmployer.moreInfoId).ToList();
                    var FoundEmployment = _context.Employement.Where(x => x.IdEmployer == FoundEmployer.moreInfoId).ToList();
                    FoundEmployer.employer = user;
                    FoundEmployer.demandeTransaction = FoundDemandes;
                    FoundEmployer.employment = FoundEmployment;
                    result.Add(FoundEmployer);
                    }
                }
                if (role == "shopowner")
                {
                    if (type == "all")
                    {
                        FoundShopowner = _context.Shopowner.AsNoTracking().FirstOrDefault(x => x.moreInfoId == user.Id );
                    }
                    else
                    {
                        FoundShopowner = _context.Shopowner.AsNoTracking().FirstOrDefault(x => x.moreInfoId == user.Id && x.Blocked == "null");

                    }
                    if (FoundShopowner!=null) {
                    FoundShopowner.employer = user;
                    result.Add(FoundShopowner);
                    }
                }
                if (role == "employee")
                {
                    if (type == "all")
                    {
                        FoundEmployee = _context.Employee.AsNoTracking().FirstOrDefault(x => x.moreInfoId == user.Id && x.activated == false);

                    }
                
                    if (FoundEmployee != null)
                    {
                        FoundEmployee.user = user;
                        result.Add(FoundEmployee);
                    }
                }

            }

            return result;

        }


        [HttpGet]
        [Route("GetRoles")]
        public  List<AspRoles> GetRoles(string id)
        {
            var Roles = _RoleManager.Roles.Where(x=>x.idEmployer == id).ToList();
            if (Roles == null)
                return Roles;
            return Roles;
        }

        [HttpGet]
        [Route("GetUsersbYid2")]
        public async Task<Object> GetUsersById2(string id,string idemployer)
        {

            var User = await _usermanager.FindByIdAsync(id);
            var Roles = await _usermanager.GetRolesAsync(User);

            foreach (var role in Roles)
            {
                if (role == "employee")
                {
                    var Found = await _context.Employee.AsNoTracking().FirstAsync(x => x.moreInfoId == User.Id);
                    var FoundEmployment = _context.Employement.Where(x => x.IdEmployee == id && x.IdEmployer == idemployer ).ToList();
                    Found.user = User;
                    if (FoundEmployment != null)
                        Found.employment = FoundEmployment;
                    return Found;


                }
                if (role == "employer")
                {
                    var Found = await _context.Employer.AsNoTracking().FirstAsync(x => x.moreInfoId == User.Id);
                    Found.employer = User;
                    return Found;

                }
                if (role == "superuser")
                {
                    var Found = User;
                    return Found;

                }
                if (role == "shopowner")
                {
                    var Found = await _context.Shopowner.AsNoTracking().FirstAsync(x => x.moreInfoId == User.Id);
                    Found.employer = User;
                    return Found;

                }
            }
            return "not found";

        }
        [HttpGet]
        [Route("GetUsersbYid")]
        public async Task<Object> GetUsersById(string id)
        {

            var User = await _usermanager.FindByIdAsync(id);
            var Roles = await _usermanager.GetRolesAsync(User);

            foreach (var role in Roles)
            {
                if (role == "employee")
                {
                    var Found = await _context.Employee.AsNoTracking().FirstAsync(x => x.moreInfoId == User.Id);
                    var FoundEmployment = _context.Employement.Where(x => x.IdEmployee == id).ToList();
                    Found.user = User;
                    if (FoundEmployment != null)
                        Found.employment = FoundEmployment;
                    return Found;


                }
                if (role == "employer")
                {
                    var Found = await _context.Employer.AsNoTracking().FirstAsync(x => x.moreInfoId == User.Id);
                    Found.employer = User;
                    return Found;

                }
                if (role == "superuser")
                {
                    var Found = User;
                    return Found;

                }
                if (role == "shopowner")
                {
                    var Found = await _context.Shopowner.AsNoTracking().FirstAsync(x => x.moreInfoId == User.Id);
                    Found.employer = User;
                    return Found;

                }
            }
            return "not found";

        }
    }
}

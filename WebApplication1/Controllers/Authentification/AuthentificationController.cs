using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using login.data.PostgresConn;
using login.models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using Platform.ReferencialData.Business.business_services.Authentification;
using Platform.ReferencialData.Business.business_services_implementations.Authentification;
using Platform.ReferencialData.Business.Helper;
using Platform.ReferentialData.DataModel.Authentification;
using Platform.ReferentialData.DtoModel.Authentification;
using SmtpServer.Text;
using WebApplication1.models;

namespace Platform.ReferencialData.WebAPI.Controllers.Authentification
{
    [ApiController]
    [Route("[controller]")]

    public class AuthentificationController : ControllerBase
    {

        private readonly Postgres _context;
        private readonly UserManager<employer3> _usermanager;
        private readonly RoleManager<AspRoles> _RoleManager;
        private readonly IConfiguration _configuration;
        private readonly IAuthentificationService _authentificationService;
        public AuthentificationController(UserManager<employer3> _usermanager, Postgres _context, RoleManager<AspRoles> _RoleManager, IConfiguration _configuration, IAuthentificationService _authentificationService)
        {
            this._usermanager = _usermanager;
            this._configuration = _configuration;
            this._RoleManager = _RoleManager;
            this._authentificationService=_authentificationService;
            this._context = _context;
        }

        ///------------ Addrole methode 
        [HttpPost]
        [Route("addRole")]
        public async Task<IActionResult> AddRole([FromBody] RoleRequestDto role)
        {
            if (ModelState.IsValid)
            {
                var Role = new AspRoles()
                {
                    Name = role.RoleName,
                    idEmployer = ""
                };
                var Role_exist = await _RoleManager.RoleExistsAsync(Role.Name);
                if (Role_exist)
                {
                    return BadRequest("Role exist");
                }

                var is_created = await _RoleManager.CreateAsync(Role);
                if (is_created.Succeeded)
                {
                    //Add Token 
                    return Ok("Role created");

                }
                else
                {
                    List<string> errors = new List<string>();
                    foreach (var error in is_created.Errors)
                    {
                        errors.Add(error.Code + error.Description);
                    }

                    return BadRequest(errors);

                }
            }
            return BadRequest();
        }

        

        [HttpGet]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string email, string code , string newpassword)
        {
            if (email == null || code == null)
            {
                return BadRequest(new authResult()
                {
                    Errors = new List<string>()
              {
                  "Invalid Reset confirmation url"
              }
                });
            }
            var user = await _usermanager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new authResult()
                {
                    Errors = new List<string>()
              {
                        "Invalid email param"
                    }
                }); 

            }
            var is_confirmed = await _usermanager.ResetPasswordAsync(user, code,newpassword);
            if (is_confirmed.Succeeded)
            {
                return Ok(new authResult()
                {
                    Result = true,
                    Errors = new List<string>()
                    {
                        "code incorrect"
                    }
                });

            }
            else
            {
                return BadRequest(new authResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "code incorrect"
                    }
                });
            }

        }


        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string email, string code)
        {
            if (email == null || code == null)
            {
                return BadRequest(new authResult()
                {
                    Errors = new List<string>()
              {
                  "Invalid email confirmation url"
              }
                });
            }
            var user = await _usermanager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new authResult()
                {
                    Errors = new List<string>()
              {
                  "Invalid email param"
              }
                });
            }
            var is_confirmed = await _usermanager.ConfirmEmailAsync(user, code);
            if (is_confirmed.Succeeded)
            {
                user.EmailConfirmed = true;
                await _usermanager.UpdateAsync(user);
                return Ok("Thank you for confirming your mail");
               
            }else
            {
                return BadRequest(new authResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "code incorrect"
                    }
                });
            }
           
        }

        [HttpPost]
        [Route("generateToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _authentificationService.verifyAndGenerateToken(tokenRequest);
                if (result == null)
                {
                    return BadRequest(
                        new authResult()
                        {
                            Errors = new List<string>(){
                                "Invalid Token"
                            },
                            Result = false
                        }
                    );
                }
                return Ok(result);
            }
            return BadRequest();
        }

            ///------------ Login methode with jwtToken 
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto user)
        {
            if (ModelState.IsValid)
            {
                var user_exist = await _usermanager.FindByEmailAsync(user.Email);

                if (user_exist == null)
                    return BadRequest(new authResult()
                    {
                        Errors = new List<string>(){
                        "Utilisateur n'existe pas "
                    },
                        Result = false
                    });

                var role_employer = await _usermanager.IsInRoleAsync(user_exist, "employer");
                if (role_employer == true)
                {
                    var employer_exist = await _context.Employer.FirstOrDefaultAsync(x => x.moreInfoId == user_exist.Id);
                    if (employer_exist != null && employer_exist.Blocked != "null")
                        return BadRequest(new authResult()
                        {
                            Errors = new List<string>(){
                        "Utilisateur est bloqué"
                    },
                            Result = false
                        });
                }


                if (user_exist == null  )
                    return BadRequest(new authResult()
                    {
                        Errors = new List<string>(){
                        "Utilisateur n'existe pas"
                    },
                        Result = false
                    });

              

                var isCorrect = await _usermanager.CheckPasswordAsync(user_exist, user.Password);

                if (!isCorrect)
                {
                   
                    return BadRequest(new authResult()
                    {
                        Result = false,
                        Errors = new List<string>(){
                        "Mot de passe incorrecte"
                    }
                    });
                }

                if (!user_exist.EmailConfirmed)
                {
                    return BadRequest(new authResult()
                    {
                        Errors = new List<string>(){
                        "Email doit être confirmer "
                    },
                        Result = false
                    });
                }

                var token = await _authentificationService.GenerateJwtToken(user_exist);
                return Ok(new authResult()
                {
                    Token = token.Token,
                    RefreshToken = token.RefreshToken,
                    Result = true
                });

                //if (isCorrect)
                //{
                //    var token = await _authentificationService.GenerateJwtToken(user_exist);
                //    return Ok(new authResult()
                //    {
                //        Token = token.Token,
                //        RefreshToken = token.RefreshToken,
                //        Result = true
                //    });
                //}
                //else
                //{
                //    return BadRequest(new authResult()
                //    {
                //        Result = false,
                //        Errors = new List<string>(){
                //        "Password incorrect "
                //    }
                //    });
                //}


            }
            return BadRequest();
        }


        

        ///------------ get users Roles
        [HttpGet]
        [Route("GetUsersRoles")]
        public async Task<IActionResult> GetUsersRoutes(string email)
        {

            var user = await _usermanager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new
                {
                    error = "User does not exist "
                });
            }
            var Roles = await _usermanager.GetRolesAsync(user);
            return Ok(Roles);
        }


        private string GenerateCode(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
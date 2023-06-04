using login.models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Platform.ReferencialData.Business.business_services.Authentification;
using Platform.ReferencialData.Business.Helper;
using Platform.ReferentialData.DataModel.Authentification;
using Platform.ReferentialData.DtoModel.Authentification;
using System.Data;

namespace Platform.ReferencialData.WebAPI.Controllers.Authentification
{
    [ApiController]
    [Route("[controller]")]

    public class RegisterController : ControllerBase
    {

        private readonly UserManager<employer3> _usermanager;
        private readonly RoleManager<AspRoles> _RoleManager;
        private readonly IConfiguration _configuration;
        private readonly IAuthentificationService _authentificationService;
        public RegisterController(UserManager<employer3> _usermanager, RoleManager<AspRoles> _RoleManager, IConfiguration _configuration, IAuthentificationService _authentificationService)
        {
            this._usermanager = _usermanager;
            this._configuration = _configuration;
            this._RoleManager = _RoleManager;
            this._authentificationService = _authentificationService;
        }


        async private Task<bool> SendEmail(employer3 createdUser,string password)
        {

            //custom code de confirmation   
            var code = await _usermanager.GenerateEmailConfirmationTokenAsync(createdUser);
            // email link to

            var confirmationUrl = Request.Scheme + "://" + Request.Host + @Url.Action("ConfirmEmail", "Authentification", new { email = createdUser.Email, code });
            //setting the email message
            var emailBody = "<h1 style='color:#4CAF50'>Dear User</h1>" +
                "<p>Thank you for signing up for our service. To complete the registration process, we need to verify your account.</p>" +
                 "<p> Your password is: "+ password + "</p>" +
                "<p> Please click on the link below to verify your email address and activate your account:</p>" +
                "<a style='background:#4CAF50;padding: 15px 32px; text-align: center; text-decoration: none;display: inline-block;font-size: 16px;margin: 4px 2px; cursor: pointer;color:white; 'href=" + System.Text.Encodings.Web.HtmlEncoder.Default.Encode(confirmationUrl) + ">Verification Link</a>" +
                "<p>Once you've verified your account, you'll be able to start using our service right away. If you have any questions or concerns, please don't hesitate to contact our customer support team.</p>" +
                "<p>Thank you for choosing our service!<p>";

            EmailHelperSMTP emailHelper = new EmailHelperSMTP();

            return emailHelper.SendEmail(createdUser.Email, emailBody);

        }

        async private Task<bool> SendEmailReset(employer3 createdUser,string newpassword)
        {

            //custom code de confirmation   
            var code = await _usermanager.GeneratePasswordResetTokenAsync(createdUser);
            // email link to

            var confirmationUrl = Request.Scheme + "://" + Request.Host + @Url.Action("ResetPassword", "Authentification", new { email = createdUser.Email, code , newpassword });
            //setting the email message
            var emailBody = "<h1 style='color:#4CAF50'>Dear User</h1>" +
                "<p>Thank you for signing up for our service. To complete the Rest process, we need to verify your account.</p>" +
                                 "<p> Your Reset password is: " + newpassword + "</p>" +

                "<p> Please click on the link below to verify your email address and reset your account:</p>" +
                "<a style='background:#4CAF50;padding: 15px 32px; text-align: center; text-decoration: none;display: inline-block;font-size: 16px;margin: 4px 2px; cursor: pointer;color:white; 'href=" + System.Text.Encodings.Web.HtmlEncoder.Default.Encode(confirmationUrl) + ">Verification Link</a>" +
                "<p>Once you've verified your account, you'll be able to start using our service right away. If you have any questions or concerns, please don't hesitate to contact our customer support team.</p>" +
                "<p>Thank you for choosing our service!<p>";

            EmailHelperSMTP emailHelper = new EmailHelperSMTP();

            return emailHelper.SendEmail(createdUser.Email, emailBody);

        }


        [HttpPost]
        [Route("modifierMotDePasse")]
        public async Task<IActionResult> modifierMotDePasse(modifierMotDePasseDto message)
        {
            if (message == null)
            {
                return BadRequest("donnée erroné");
            }
            var user = await _usermanager.FindByEmailAsync(message.mail);
            if (user != null)
            {
                var result = await _usermanager.ChangePasswordAsync(user, message.encienMotDePasse, message.motDePasse);
                if (result.Succeeded)
                {
                    return Ok("mot de passe changer avec succée");
                }
                else
                {
                    return BadRequest(result.Errors.ToString());
                }
            }
            return BadRequest("utilisateur n'existant pas ");

        }

        [HttpPost]
        [Route("ResetPassword")]
        
        public async Task<IActionResult> ResetPassword(resetPasswordDto reset)
        {
            if (reset.email != null && reset.Password != null)
            {
                var createdUser = await _usermanager.FindByEmailAsync(reset.email);

                if (await SendEmailReset(createdUser,reset.Password))
                {
                    return Ok("reset need confirmation");
                }
                return BadRequest("EROR");

            }
            return BadRequest("donnée erroné");
        }        
        [HttpPost]

        [Route("RegisterEmployer")]
        ///here you set your application restrictions Roles identifies the users that can access your methode
        ///and you specifie that it uses JwtBearer token
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "superuser")]
        public async Task<IActionResult> RegisterEmployer([FromBody] EmployerInfoDto user)
        {
            if (user != null)
            {
                //create new instance of the Users table model
                var new_user = new employer3()
                {
                    UserName = user.Name,
                    Email = user.Email,
                    EmailConfirmed = false,
                    walletPublicKey = user.walletpublickey
                };
                authResult is_created;
                if (user.Role == "employer")
                {
                    is_created = await _authentificationService.createUser(user);
                }
                else
                {
                    return BadRequest(new authResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Role n'existe pas"
                        }
                    });

                }

                if (!is_created.Result)
                {
                    return BadRequest(is_created);

                }
                else
                {
                    // add Employer information to database 
                    await _authentificationService.RegisterEmployer(user);
                    // Find user by Email
                    var createdUser = await _usermanager.FindByEmailAsync(new_user.Email);
                    // add User Role
                    await _usermanager.AddToRoleAsync(createdUser, user.Role);

                    if (await SendEmail(createdUser,user.Password))
                    {
                        return Ok("email need confirmation");
                    }
                    return BadRequest("EROR");

                }


            }

            return BadRequest("connect to network");

        }





        [HttpPost]
        [Route("RegisterShopowner")]
        ///here you set your application restrictions Roles identifies the users that can access your methode
        ///and you specifie that it uses JwtBearer token
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "superuser")]
        public async Task<IActionResult> RegisterShopowner([FromBody] shopownerDto user)
        {
            if (user != null)
            {
                //create new instance of the Users table model
                var new_user = new employer3()
                {
                    UserName = user.Name,
                    Email = user.Email,
                    EmailConfirmed = false,
                    walletPublicKey = user.walletpublickey

                };
                authResult is_created;
                if (user.Role == "shopowner")
                {
                    is_created = await _authentificationService.createUser(user);
                }
                else
                {
                    return BadRequest(new authResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Role does not match"
                        }
                    });

                }
                if (!is_created.Result)
                {
                    return BadRequest(is_created);

                }
                else
                {
                    // add Employer information to database 
                    await _authentificationService.RegisterShopowner(user);
                    // Find user by Email
                    var createdUser = await _usermanager.FindByEmailAsync(new_user.Email);
                    // add User Role
                    await _usermanager.AddToRoleAsync(createdUser, user.Role);


                    if (await SendEmail(createdUser,user.Password))
                    {
                        return Ok("email need confirmation");
                    }
                    return BadRequest("EROR");


                }


            }

            return BadRequest("bad");

        }


        [HttpPost]
        [Route("RegisterEmployee")]
        ///here you set your application restrictions Roles identifies the users that can access your methode
        ///and you specifie that it uses JwtBearer token
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "employer")]
        public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeDto user)
        {
            if (user != null)
            {
                //create new instance of the Users table model
                var new_user = new employer3()
                {
                    UserName = user.Name,
                    Email = user.Email,
                    EmailConfirmed = false,
                    walletPublicKey = user.walletpublickey

                };
                authResult is_created;
                if (user.Role == "employee")
                {
                    is_created = await _authentificationService.createUser(user);
                }
                else
                {
                    return BadRequest(new authResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Role does not match"
                        }
                    });

                }

                if (!is_created.Result)
                {
                    return BadRequest(is_created);

                }
                else
                {
                    // add Employer information to database 
                    await _authentificationService.RegisterEmployee(user);
                    // Find user by Email
                    var createdUser = await _usermanager.FindByEmailAsync(new_user.Email);
                    // add User Role
                    await _usermanager.AddToRoleAsync(createdUser, user.Role);
                    if (await SendEmail(createdUser,user.Password))
                    {
                        return Ok("email need confirmation");
                    }
                    return BadRequest("EROR");

                }


            }

            return BadRequest("bad");

        }

        [HttpPost]
        [Route("RegisterSuperuser")]
        ///here you set your application restrictions Roles identifies the users that can access your methode
        ///and you specifie that it uses JwtBearer token
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "superuser")]
        public async Task<IActionResult> RegisterSuperuser([FromBody] UserRegistrationRequestDto user)
        {
            if (user != null)
            {
                //create new instance of the Users table model
                var new_user = new employer3()
                {
                    UserName = user.Name,
                    Email = user.Email,
                    EmailConfirmed = false,
                    walletPublicKey = user.walletpublickey,
                };
                authResult is_created;
                if (user.Role == "superuser")
                {
                    is_created = await _authentificationService.createUser(user);
                }
                else
                {
                    return BadRequest(new authResult()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Role does not match"
                        }
                    });

                }
                if (!is_created.Result)
                {
                    return BadRequest(is_created);

                }
                else
                {
                    await _authentificationService.Registersuperuser(user);

                    // Find user by Email
                    var createdUser = await _usermanager.FindByEmailAsync(new_user.Email);
                    // add User Role
                    await _usermanager.AddToRoleAsync(createdUser, user.Role);
                    if (await SendEmail(createdUser,user.Password))
                    {
                        return Ok("email need confirmation");
                    }
                    return BadRequest("EROR");

                }


            }

            return BadRequest("bad");

        }

        /// ----------------confirm email


    }
}

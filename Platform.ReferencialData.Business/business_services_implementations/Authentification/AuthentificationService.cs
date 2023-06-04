using login.data.PostgresConn;
using login.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

using Platform.ReferencialData.Business.business_services.Authentification;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

using Platform.ReferentialData.DtoModel.Authentification;
using WebApplication1.models;
using Platform.ReferentialData.DataModel.Authentification;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Platform.ReferencialData.Business.business_services_implementations.Authentification
{
    public class AuthentificationService : IAuthentificationService
    {
        private TokenValidationParameters _tokenValidationParameters;
        private readonly Postgres _context;
        private readonly UserManager<employer3> _usermanager;
        private readonly IConfiguration _configuration; 
        private readonly RoleManager<AspRoles> _RoleManager;
        public AuthentificationService(UserManager<employer3> _usermanager, Postgres context, IConfiguration configuration, RoleManager<AspRoles> roleManager, TokenValidationParameters _tokenValidationParameters)
        {
            this._usermanager = _usermanager;
            _context = context;
            _configuration = configuration;
            _RoleManager = roleManager;
            this._tokenValidationParameters = _tokenValidationParameters;
        }
        public async Task<authResult> GenerateJwtToken(employer3 user)
        {
            var JwtTokenHandler = new JwtSecurityTokenHandler();
            var claims = await GetClaims(user);
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:secret").Value);
            var TokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(100),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)

            };
            Console.Write(_usermanager.GetRolesAsync(user));
            var token = JwtTokenHandler.CreateToken(TokenDescription);
            var JwtToken =   JwtTokenHandler.WriteToken(token);
            var RefreshToken = new RefreshTokenModel(){
                 Id = Guid.NewGuid().ToString(),
                 JwtId = token.Id,
                 Token = GenerateRefreshToken(21), 
                 AddedDate = DateTime.UtcNow,
                 ExpiryDate = DateTime.UtcNow.AddMonths(6),
                 IsRevoked = false , 
                 IsUsed = false ,
                 UserId = user.Id,
                
             };
             await _context.RefreshTokenTable.AddAsync(RefreshToken);
             await _context.SaveChangesAsync();
            return new authResult(){
                 Token = JwtToken,
                 RefreshToken = RefreshToken.Token,
                 Result = true
             };
        }
        private string GenerateRefreshToken(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<List<Claim>> GetClaims(employer3 user)
        {
            var claims = new List<Claim> {
                    new Claim ("Id" , user.Id),
                    new Claim (JwtRegisteredClaimNames.Sub , user.Email),
                    new Claim (JwtRegisteredClaimNames.Email , user.Email),
                    new Claim (JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                    new Claim (JwtRegisteredClaimNames.Iat , DateTime.Now.ToFileTimeUtc().ToString()),
                    new Claim("wallet" , user.walletPublicKey.ToString()),
                    new Claim("userName" , user.UserName),  

                };
            var userClaims = await _usermanager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var userRoles = await _usermanager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {

                var Role = await _RoleManager.FindByNameAsync(userRole);
                if (Role != null)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                    var Roleclaims = await _RoleManager.GetClaimsAsync(Role);
                    foreach (var roleClaim in Roleclaims)
                    {
                        claims.Add(roleClaim);
                    }
                }

            }

            return claims;
        }
        private DateTime UnixTimeStampToDateTime(long utcExpiryDate)
        {
            var currentTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            currentTime = currentTime.AddSeconds(utcExpiryDate).ToUniversalTime();
            return currentTime;
        }

        public async Task<authResult> verifyAndGenerateToken(RefreshTokenDto tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                _tokenValidationParameters.ValidateLifetime = false;
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validedToken);

                if (validedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (result == false)
                        return null;
                }
                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);
                if (expiryDate < DateTime.Now)
                    return new authResult()
                    {
                        Errors = new List<string>() {
                            "time expired"
                        },
                        Result = false

                    };

                var storedToken = await _context.RefreshTokenTable.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);
                if (storedToken == null)
                    return new authResult()
                    {
                        Errors = new List<string>() {
                            "Invalid Token",
                            "Token not Found"
                        },
                        Result = false
                    };

                if (storedToken.IsUsed)
                    return new authResult()
                    {
                        Errors = new List<string>() {
                            "Invalid Token",
                            "Token IsUsed"
                        },
                        Result = false
                    };

                if (storedToken.IsRevoked)
                    return new authResult()
                    {
                        Errors = new List<string>() {
                            "Invalid Token",
                            "Token IsRevoked"
                        },
                        Result = false
                    };

                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                    return new authResult()
                    {
                        Errors = new List<string>() {
                            "Invalid Token"
                        },
                        Result = false
                    };

                if (storedToken.ExpiryDate < DateTime.Now)
                    return new authResult()
                    {
                        Errors = new List<string>() {
                            "Token Expired"
                        },
                        Result = false
                    };
                storedToken.IsUsed = true;
                _context.RefreshTokenTable.Update(storedToken);
                await _context.SaveChangesAsync();
                var dbUser = await _usermanager.FindByIdAsync(storedToken.UserId);
                
                return await GenerateJwtToken(dbUser);
            }
            catch (Exception e)
            {
                return new authResult()
                {
                    Errors = new List<string>() {
                            "Expired Token"
                        },
                    Result = false
                };
            }
        }

     
        public async Task<authResult> createUser(UserRegistrationRequestDto user)
        {
            
            
                // check if user exist
                var user_exist = await _usermanager.FindByEmailAsync(user.Email);

                if (user_exist != null)
                    return new authResult()
                    {
                        Result = false,
                        Errors = new List<string>(){
                        "Email existant"
                    }
                    };
                //create new instance of the Users table model
                var new_user = new employer3()
                {
                    UserName = user.Name,
                    Email = user.Email,
                    EmailConfirmed = false,
                    walletPublicKey = user.walletpublickey,
                };



                // check if Role exist 
                var Role_exist = await _RoleManager.RoleExistsAsync(user.Role);
                
                if (Role_exist)
                {
                    // create user 
                    IdentityResult is_created = await _usermanager.CreateAsync(new_user, user.Password);
                   if (is_created.Succeeded) {
                        return new authResult()
                        {
                            Result = true,

                        };
                        }
                    else
                        {
                        List<string> errors = new List<string>();
                        foreach (var error in is_created.Errors)
                        {
                            errors.Add(error.Code + error.Description);
                        }
                        return new authResult()
                        {
                            Result = false,
                            Errors = errors
                        };

                            
                        };
                    
                 
                    
                }
                else
                {
                    return   new authResult()
                    {
                        Result = false,
                        Errors = new List<string>(){
                        "Role n'existe pas"
                    }
                    };

                }
                
            
          
        }
        public async Task RegisterEmployer(EmployerInfoDto employer)
        {
            
                var user = await _usermanager.FindByEmailAsync(employer.Email);
                var MoreInfo = new MoreInfoEmployer()
                {
                    moreInfoId = user.Id,
                    matriculeFiscale = employer.matriculeFiscale,
                    numeroTelEntreprise = employer.numeroTelEntreprise,
                    codeTVA = employer.codeTVA,
                    adresseFacturation = employer.adresseFacturation,
                    AdresseEntreprise = employer.AdresseEntreprise,
                    EmailRH = employer.EmailRH,
                    NumTelRH = employer.NumTelRH,
                    PaymentMethode = employer.PaymentMethode,
                    Blocked = "null"
                };
                await _context.Employer.AddAsync(MoreInfo);
            
        }
       public async Task RegisterEmployee(EmployeeDto employee)
        {

            var user = await _usermanager.FindByEmailAsync(employee.Email);
            var Employee = new Employee()
            {
                //EmployeeId = Guid.NewGuid().ToString(),
                moreInfoId = user.Id,
                NumTel = employee.NumTel,
                balance = 0,
                activated = false,
               
            };
            await _context.Employee.AddAsync(Employee);
        }

        public async Task RegisterShopowner(shopownerDto user)
        {
         
                var Finduser = await _usermanager.FindByEmailAsync(user.Email);
                var MoreInfo = new ShopOwner()
                {
                    //ShopOwnerIdMoreInfo = Guid.NewGuid().ToString(),
                    moreInfoId = Finduser.Id,
                    Adresse = user.Adresse,
                    codeTVA = user.codeTVA,
                    adresseFacturation = user.adresseFacturation,          
                    NumTel = user.NumTel,
                    PaymentMethode = user.PaymentMethode,
                    matriculeFiscale = user.matriculeFiscale,
                    commission= user.commission,
                    delaiPayement = user.delaiPayement,
                    Blocked = "null"
             
                };
                await _context.Shopowner.AddAsync(MoreInfo);
            
        }

        public async Task Registersuperuser(UserRegistrationRequestDto user)
        {
            var Finduser = await _usermanager.FindByEmailAsync(user.Email);

            var superuser = new superuser()
            {
                Idsuperuser = Finduser.Id,
            };
            await _context.superuser.AddAsync(superuser);
            _context.SaveChanges();

        }
    }
}

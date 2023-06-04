using login.data.PostgresConn;
using login.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Platform.ReferencialData.Business.business_services.Authentification;
using Platform.ReferentialData.DataModel.Authentification;
using Platform.ReferentialData.DtoModel.Authentification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections;
using Newtonsoft.Json;

namespace Platform.ReferencialData.Business.business_services_implementations.Authentification
{
    public class EmployeeManagementService : IEmployeeManagementService
    {
        private readonly Postgres _context;
        private readonly UserManager<employer3> _usermanager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<AspRoles> _RoleManager;
        public EmployeeManagementService(UserManager<employer3> _usermanager, Postgres context, IConfiguration configuration, RoleManager<AspRoles> roleManager)
        {
            this._usermanager = _usermanager;
            _context = context;
            _configuration = configuration;
            _RoleManager = roleManager;
        }

        public async Task FireEmployee(deleteEmploymentDto employ)
        {
            var employee =await _usermanager.FindByEmailAsync(employ.mailEmployee);
            
            var itemsToDelete = _context.Employement.Where(x => x.IdEmployer == employ.idEmployer && x.IdEmployee == employee.Id);
           if (itemsToDelete!=null)
            {
            
                var roles = await _usermanager.GetRolesAsync(employee);
                var filteredRoles = roles.Where(x => x != "employer" && x != "superuser" && x != "shopowner" && x != "employee").ToList();
                if (filteredRoles.Count > 0)
                {
                
                foreach (var role in filteredRoles)
                {
                    var RolesToDelete = await _usermanager.RemoveFromRoleAsync(employee,role);
                }
                }
                _context.Employement.RemoveRange(itemsToDelete);
                _context.SaveChanges();
            }

        }


        public async Task<IList<string>> getRolesByUserId(string Id)
        {
            if (Id == null)
            {
                return null;
            }
            var user = await _usermanager.FindByIdAsync(Id);
            var roles = await _usermanager.GetRolesAsync(user);
            if ( roles.Count > 0)
            {
                return roles;
            }
            return null;
           
        }


        public async Task<DataResult> updateEmployement(UpdateEmployementDto employ)
        {
            var employee = await _usermanager.FindByEmailAsync(employ.mailEmployee);
            var employer = await _usermanager.FindByIdAsync(employ.IdEmployer);
            var categorie =await _context.categorie.FirstOrDefaultAsync(x => x.IdCategorie == employ.IdCategorie);
            if (employee == null || employer == null || categorie == null)
            {
                return new DataResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "employer or employee or categorie does not exist "
                    }
                };
            }
           
            var exist =await _context.Employement.FirstOrDefaultAsync(x => x.IdEmployer == employer.Id && x.IdEmployee == employee.Id );
            if (exist == null)
            {
                return new DataResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Employment n'exist pas"
                    }
                };
            }
            var role = await _usermanager.IsInRoleAsync(employee,employ.Role);
            if (role == false)
            {
                await _usermanager.RemoveFromRoleAsync(employee, employ.curentRole);
                await _usermanager.AddToRoleAsync(employee, employ.Role);
            }
            exist.IdEmployee = employee.Id;
            exist.IdEmployer = employ.IdEmployer;
            exist.IdCategorie = employ.IdCategorie;
           
            _context.Employement.UpdateRange(exist);
            _context.SaveChanges();

            return new DataResult()
            {
                Result = true,

            };

        }
        public async Task<DataResult> addEmployment(EmployDto employ)
        {
            var employee  = await _usermanager.FindByEmailAsync(employ.mailEmployee);
            var employer = await  _usermanager.FindByIdAsync(employ.IdEmployer);
            var categorie =  _context.categorie.FirstOrDefault(x=> x.IdCategorie == employ.IdCategorie);

            
            if (employee == null || employer == null || categorie == null ) {
                return new DataResult()
                {
                    Result = false,
                    Errors= new List<string>()
                    {
                        "employer or employee or categorie does not exist "
                    }
                };
            }
            
            var data = new Employment()
            {
                IdEmployee = employee.Id,
                IdEmployer = employ.IdEmployer,
                IdCategorie= employ.IdCategorie
            };
            var exist = _context.Employement.FirstOrDefault(x=> x.IdEmployer== employer.Id && x.IdEmployee == employee.Id && categorie.IdCategorie == x.IdCategorie);
            if ( exist != null)
            {
                return new DataResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Employment already exist"
                    }
                };
            }
            await _context.Employement.AddAsync(data);
            await _context.SaveChangesAsync();
            return new DataResult()
            {
                Result = true,
               
            }; 
            
        }

        public async Task<List<Object>> getEmployementById(string id)
        {
            List<Object> result = new List<Object>();
            var Employment = _context.Employement.Where(x=>x.IdEmployer == id).ToList();
            foreach (var i in Employment)
            {
                var Employ = new Employment();
                var Employee = new Employee();
                var employer = await _context.Employer.AsNoTracking().FirstAsync(x => x.moreInfoId == i.IdEmployer) ;
                var employee = await _context.Employee.AsNoTracking().FirstAsync(x => x.moreInfoId == i.IdEmployee);
                var Categorie = await _context.categorie.AsNoTracking().FirstAsync(x => x.IdCategorie == i.IdCategorie);
                var user = await _usermanager.FindByIdAsync(i.IdEmployee);
                var roles = await _usermanager.GetRolesAsync(user);
                
                var roleExist = new List<AspRoles>();
                foreach(var role in roles)
                {
                    
                    if(role!="employee" && await _usermanager.IsInRoleAsync(user, role))
                    {
                        var rolee = await _RoleManager.FindByNameAsync(role);
                        if ( rolee.idEmployer == id )
                            roleExist.Add(rolee);
                    }
                        
                }
                
                Employ = i;
                Employ.employer = employer;
                Employee.user = user;
                Employ.employee = employee;
                Employ.employee.user = user;
                Employ.employer.hasRole = roleExist;
                Employ.Categorie = Categorie;
                result.Add(Employ);
            }
            return result;
        }


        public async Task<List<employer3>> getaccountName(List<string> list)
        {
            var result = new List<employer3>();
            foreach (var item in list)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.walletPublicKey == item);
                result.Add(user);
            }
            return result;

        }
        public async Task<bool> AddRole(RoleRequestDto role)
        {
            if (role != null)
            {
                var Role = new AspRoles()
                {
                    Name = role.RoleName,
                    idEmployer = role.idEmployer
                };
               

                var is_created = await _RoleManager.CreateAsync(Role);
                if (is_created.Succeeded)
                {
                    //Add Token 
                    return true;

                }

            }
            return false;
        }

        public async Task<bool> addClaims(AspRoles existing_Role, List<string> permissions)
        {
            foreach (var permission in permissions)
            {
                var is_created = await _RoleManager.AddClaimAsync(existing_Role, new Claim("Permission", permission));
                if (!is_created.Succeeded)
                {
                    return false;
                }

            }
            return true;
        }

        public async Task addDemandeTransaction(demandeTransactionDto transaction)
        {
            var employer = await _usermanager.FindByIdAsync(transaction.IdEmployer);
            if (employer == null)
            {
                throw new ArgumentException("employer does not exist");
            }
            var data = new demandeTransaction()
            {
                IdDemandeTransaction = Guid.NewGuid().ToString(),
                IdEmployer = employer.Id,
                date = DateTime.UtcNow,
                amount = transaction.amount,
                etat = "encours"
            };
            await _context.DemandeTransaction.AddAsync(data);
            await _context.SaveChangesAsync();

        }

        public async Task<List<Object>> getDemandeTransaction()
        {
            List<Object> result = new List<Object>();
            var demands =  _context.DemandeTransaction.ToList();
            foreach (var i in demands)
            {
                var demande = new demandeTransaction();
                var employer = await _context.Employer.AsNoTracking().FirstAsync(x => x.moreInfoId == i.IdEmployer );
                var user = await _usermanager.FindByIdAsync(i.IdEmployer);
                demande = i;
                demande.employer= employer;
                demande.employer.employer = user;
                result.Add(demande);
            }
            return result;

        }


        public async Task<bool> addTransaction(TransactionDto transaction)
        {
            if (transaction == null) throw new ArgumentNullException();
            var existAdmin =await  _context.superuser.FirstOrDefaultAsync(e => e.Idsuperuser == transaction.Idverificateur);
            var existEmployer = await _context.Employer.FirstOrDefaultAsync(e => e.moreInfoId == transaction.IdEmployer);
            var existDemande = await _context.DemandeTransaction.FirstOrDefaultAsync(e => e.IdDemandeTransaction == transaction.iddemande);
            if( existAdmin != null && existEmployer != null && existDemande!=null &&  existDemande.etat =="encours" ) {
            var Transaction = new TransactionTerminer()
            {
                IdTransaction = Guid.NewGuid().ToString(),
                IdEmployer = transaction.IdEmployer,
                IdVerificateur = transaction.Idverificateur,
                montant = transaction.montant,
                date = DateTime.UtcNow,
                etat = "valid"
            };
                await _context.transactions.AddAsync(Transaction);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<List<demandeTransaction>> getDemandeTransactionByEmployer(string Id)
        {
                return _context.DemandeTransaction.Where(x => x.IdEmployer == Id).ToList();
        }

        public async Task<List<demandePayement>> getDemandePayementByShopowner(string Id)
        {
                return _context.demandePayement.Where(x => x.IdShopowner == Id).ToList();
        }

        public List<demandePayement> getDemandePayementById(string Id)
        {
            return _context.demandePayement.Where(x => x.IdDemandePayement == Id).ToList();
        }


        public async Task<List<Object>> getDemandePayement()
        {
            List<Object> result = new List<Object>();
            var demands = _context.demandePayement.ToList();
            foreach (var i in demands)
            {
                var demande = new demandePayement();
                var employer = await _context.Shopowner.AsNoTracking().FirstAsync(x => x.moreInfoId == i.IdShopowner);
                var user = await _usermanager.FindByIdAsync(i.IdShopowner);
                demande = i;
                demande.shopowner = employer;
                demande.shopowner.employer = user;
                result.Add(demande);
            }
            return result;

        }


        public async Task addDemandePayement(demandePayementDto transaction)
        {
            var employer = await _usermanager.FindByIdAsync(transaction.IdShopowner);
            if (employer == null)
            {
                throw new ArgumentException("employer does not exist");
            }
            var data = new demandePayement()
            {
                IdDemandePayement = Guid.NewGuid().ToString(),
                IdShopowner = employer.Id,
                date = DateTime.UtcNow,
                amount = transaction.amount,
                IdValidateur = "",
                walletValidateur ="",
                etat = "encours"
            };
            await _context.demandePayement.AddAsync(data);
            await _context.SaveChangesAsync();

        }


        public async Task<bool> updateDemandePayement(string id, string value,string idVal)
        {
            if (id == null)
                return false;
            var exist_validateur = await _context.superuser.FirstOrDefaultAsync(x=> x.Idsuperuser == idVal);
            var demande = _context.demandePayement.FirstOrDefault(x => x.IdDemandePayement == id); ;
            if (demande != null && exist_validateur!= null)
            {
                var validateur = await _usermanager.FindByIdAsync(idVal);

                demande.etat = value;
                demande.IdValidateur = validateur.Id;
                demande.walletValidateur = validateur.walletPublicKey;
                _context.demandePayement.UpdateRange(demande);
                _context.SaveChanges();
                return true;
            }
            return false;
        }



        public bool  deleteDemandeTransaction(string id)
        {
            if (id == null)
                return false;
            var demande = _context.DemandeTransaction.FirstOrDefault(x=> x.IdDemandeTransaction == id);
            if (demande != null)
            {
                _context.DemandeTransaction.RemoveRange(demande);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool updateDemandeTransaction(string id,string value)
        {
            if (id == null)
                return false;
            var demande = _context.DemandeTransaction.FirstOrDefault(x => x.IdDemandeTransaction == id);
            if (demande != null)
            {
                demande.etat = value;
                _context.DemandeTransaction.UpdateRange(demande);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> addPayement(PayementDto payement)
        {
            if (payement != null)
            {
                var existAdmin = await _context.Employee.FirstOrDefaultAsync(e => e.moreInfoId == payement.IdEmployee);
                var existEmployer = await _context.Employer.FirstOrDefaultAsync(e => e.moreInfoId == payement.IdEmployer);
                if(existAdmin!= null && existEmployer!= null)
                {

               
                var Payement = new payement()
                {
                    idPayment = Guid.NewGuid().ToString(),
                    idEmployer = payement.IdEmployee,
                    idsociété = payement.IdEmployer,
                    date = DateTime.UtcNow,
                    dayOfwork = payement.dayofwork,
                    montant = payement.montant,
                   

                };
                _context.Payement.Add(Payement);
                _context.SaveChanges();
                return true;
                }
                return false;
                
            }
            return false;
            
        }

        public bool addaccount(string secret,string iduser)
        {
            if ( secret == null && iduser == null)
            {
                return false;
            }
            var account = new accounts()
            {
                iduser = iduser,
                idaccount = Guid.NewGuid().ToString(),
                secret = secret
            };
            _context.accounts.Add(account);
            _context.SaveChanges();
            return true;
        }

        public  string getAccount(string id)
        {
            
           var account = _context.accounts.Where(x => x.iduser == id).ToList();
            string json = JsonConvert.SerializeObject(account, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });


            return json;


        }
    }
}

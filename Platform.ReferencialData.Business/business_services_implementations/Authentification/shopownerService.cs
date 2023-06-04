using login.data.PostgresConn;
using login.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Platform.ReferencialData.Business.business_services.Authentification;
using Platform.ReferentialData.DataModel.Authentification;
using Platform.ReferentialData.DtoModel.Authentification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.models;

namespace Platform.ReferencialData.Business.business_services_implementations.Authentification
{
    public class shopownerService : IshopownerService
    {
        private readonly Postgres _context;
        private readonly UserManager<employer3> _usermanager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<AspRoles> _RoleManager;
        public shopownerService(UserManager<employer3> _usermanager, Postgres context, IConfiguration configuration, RoleManager<AspRoles> roleManager)
        {
            this._usermanager = _usermanager;
            _context = context;
            _configuration = configuration;
            _RoleManager = roleManager;
        }

        public async Task addDemandePayement(demandePayementDto demande)
        {
            var shopowner = await _usermanager.FindByIdAsync(demande.IdShopowner);
            if (shopowner == null)
            {
                throw new ArgumentException("marchant n'existe pas ");
            }

            var data = new demandePayement
            {
                IdDemandePayement = Guid.NewGuid().ToString(),
                IdShopowner = demande.IdShopowner,
                date = DateTime.UtcNow,
                amount = demande.amount,
                etat = "encours"
            };
            await _context.demandePayement.AddAsync(data);
            await _context.SaveChangesAsync();
        }



        public async  Task<List<Object>> getAllDemandePayement( )
        {
              ;

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

        public List<demandePayement> getDemandePayementByshopowner(string Id)
        {
            return _context.demandePayement.Where(x => x.IdDemandePayement == Id).ToList();
        }

        public bool deleteDemande(string id)
        {
            if (id != null)
            {
                 
                var demande = _context.demandePayement.FirstOrDefault(x=> x.IdDemandePayement == id);
                if (demande == null)
                {
                    throw new ArgumentException("marchant n'existe pas ");
                }
                _context.demandePayement.Remove(demande);
                int rowsAffected = _context.SaveChanges();
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                    return false;
                
            }
            return false;

        }
    }
    
}

using login.data.PostgresConn;
using login.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Platform.ReferencialData.Business.business_services.Authentification;
using Platform.ReferentialData.DataModel.Authentification;
using Platform.ReferentialData.DtoModel.Authentification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferencialData.Business.business_services_implementations.Authentification
{
    public class categorieService : ICategorie
    {
        private readonly Postgres _context;
        private readonly UserManager<employer3> _usermanager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<AspRoles> _RoleManager;
        public categorieService(UserManager<employer3> _usermanager, Postgres context, IConfiguration configuration, RoleManager<AspRoles> roleManager)
        {
            this._usermanager = _usermanager;
            _context = context;
            _configuration = configuration;
            _RoleManager = roleManager;
        }

       public string GetAll(string id)
        {
            List<Object> result = new List<Object>();
            var categorie = _context.categorie.Where(X => X.idEmployer == id).ToList();
            foreach (var c in categorie)
            {
                var categ = new Categorie();
                var Foundticket =  _context.Ticket.FirstOrDefault(v => v.IdTicket == c.idticket);
                categ = c;
                categ.ticket = Foundticket;
                result.Add(categ);
            };
            string json = JsonConvert.SerializeObject(categorie, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return json;
        }

        public async Task<bool> UpdateTicket(updateTicketDto ticket)
        {
            if ( ticket != null)
            {
                var Foundticket = _context.Ticket.FirstOrDefault(x=> x.IdTicket == ticket.idTicket);
                var Ticketalreadyexist = _context.Ticket.FirstOrDefault(x=> (x.prixTicket == ticket.prixTicket || x.nameTicket == ticket.nameTicket) && x.IdTicket != ticket.idTicket && x.idEmployer ==ticket.idEmployer);
                if ( Foundticket != null && Ticketalreadyexist==null  )
                {
                    Foundticket.prixTicket = ticket.prixTicket;
                    Foundticket.nameTicket = ticket.nameTicket;
                    _context.Ticket.UpdateRange(Foundticket);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            return false;
        }

        public string getCategorieById(string id)
        {
            Categorie categorie = _context.categorie.FirstOrDefault(x => x.IdCategorie== id);
            if (categorie!=null)
            {
               var  ticket = _context.Ticket.FirstOrDefault(x => x.IdTicket == categorie.idticket);
               categorie.ticket = ticket;

            }
            string json = JsonConvert.SerializeObject(categorie, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return json;
            
        }


        public async Task<bool> UpdateCategorieAsync(updateCategorieDto categorie)
        {
            if (categorie != null)
            {
                var Foundcategorie = _context.categorie.FirstOrDefault(x => x.IdCategorie == categorie.idCategorie);
                var existCategorie =  _context.categorie.FirstOrDefault(x => x.NameCateforie == categorie.NameCateforie && x.IdCategorie != categorie.idCategorie );
                var existTicket =  _context.Ticket.FirstOrDefault(x => x.nameTicket == categorie.nameTicket && x.idEmployer == categorie.idEmployer);
                if (existCategorie == null && Foundcategorie != null && existTicket != null )
                {
                    
                        Foundcategorie.NameCateforie = categorie.NameCateforie;
                        Foundcategorie.idticket = existTicket.IdTicket;
                        _context.categorie.UpdateRange(Foundcategorie);
                        _context.SaveChanges();
                 
                    
                    return true;
                }
                return false;
                
            }
            return false;
        }
  
        public async Task<bool> addTicket(ticketDto ticket)
        {
            
                var exist = await _context.Ticket.FirstOrDefaultAsync(x => (x.prixTicket == ticket.prixTicket || x.nameTicket == ticket.nameTicket) && x.idEmployer == ticket.idEmployer);
                if (exist != null)
                {
                    return false;
                }

                var data = new Ticket()
                {
                    IdTicket = Guid.NewGuid().ToString(),
                    nameTicket = ticket.nameTicket,
                    prixTicket = ticket.prixTicket,
                    idEmployer = ticket.idEmployer,
                    status = false
                };

                await _context.Ticket.AddAsync(data);
                int rowsAffected = await _context.SaveChangesAsync();

                return rowsAffected > 0;
            
        }

        public  List<Ticket> getAllTicket(string id)
        {
                return _context.Ticket.Where(x => x.idEmployer == id).ToList();
        }

        public async Task<bool> deleteCategorie(string id)
        {
            var deleteCategorie = await  _context.categorie.FirstOrDefaultAsync(x => x.IdCategorie == id);
       
            if (deleteCategorie != null)
            {
                var FoundTicket = _context.Ticket.FirstOrDefault(x=> x.IdTicket == deleteCategorie.idticket);
                if (FoundTicket != null) {
                    FoundTicket.status = false;
                    _context.Ticket.UpdateRange(FoundTicket);
                }
                _context.categorie.RemoveRange(deleteCategorie);
                await _context.SaveChangesAsync();
                return true;
            }else
            {
                return false;
            }
                
        }
        public async Task addCategorie(categorieDto ticket)
        {

            var exist = _context.categorie.FirstOrDefault(x => x.NameCateforie == ticket.NameCateforie && x.idEmployer == ticket.idEmployer );
            if (exist != null)
            {
                throw new ArgumentException("categorie already exist .");

            }
            var Foundticket = _context.Ticket.FirstOrDefault(x=> x.nameTicket == ticket.nameTicket && x.idEmployer == ticket.idEmployer);
            var data = new Categorie()
            {
                IdCategorie = Guid.NewGuid().ToString(),
                idticket = Foundticket.IdTicket,
                NameCateforie = ticket.NameCateforie,
                idEmployer = ticket.idEmployer
            };
            Foundticket.status = true;
            await _context.categorie.AddAsync(data);
            await _context.SaveChangesAsync();
        }
    }
}

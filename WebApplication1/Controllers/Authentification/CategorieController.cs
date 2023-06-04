using login.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Platform.ReferencialData.Business.business_services.Authentification;
using Platform.ReferencialData.Business.business_services_implementations.Authentification;
using Platform.ReferentialData.DataModel.Authentification;
using Platform.ReferentialData.DtoModel.Authentification;

namespace Platform.ReferencialData.WebAPI.Controllers.Authentification
{
    [ApiController]
    [Route("[controller]")]
    public class CategorieController : ControllerBase
    {
        private readonly UserManager<employer3> _usermanager;
        private readonly RoleManager<AspRoles> _RoleManager;
        private readonly IConfiguration _configuration;
        private readonly ICategorie _Categorie;

        public CategorieController(UserManager<employer3> _usermanager, RoleManager<AspRoles> _RoleManager, IConfiguration _configuration, ICategorie _Categorie)
        {
            this._usermanager = _usermanager;
            this._configuration = _configuration;
            this._RoleManager = _RoleManager;
            this._Categorie = _Categorie;
        }

        [HttpGet]
        [Route("getAllCategorie")]
        public string getAllCategorie(string id)
        {
             return  _Categorie.GetAll(id);
        }

        [HttpGet]
        [Route("getAllTicket")]
        public List<Ticket> getAllTicket(string id)
        {
            return _Categorie.getAllTicket(id);
        }

        [HttpPost]
        [Route("addTicket")]
        public async Task <IActionResult> addTicket(ticketDto ticket)
        {



            if (ticket == null)
            {
                return BadRequest("valeur entrer sont null");

            }
            var employer_exist =await _usermanager.FindByIdAsync(ticket.idEmployer);
            if (employer_exist != null)
            {
                var result = await _Categorie.addTicket(ticket);
                if (result)
                {
                    return Ok("ticket ajouter");
                }
                else
                {
                    return BadRequest("ticket n'est pas ajouter");
                }
            }
                
            
            return BadRequest("ok");

        }
        [HttpPost]
        [Route("addCategorie")]
        public async Task addCategorie(categorieDto categorie)
        {
            await _Categorie.addCategorie(categorie);

        }

        [HttpGet]
        [Route("getCategorieById")]
        public string getCategorieById(string id)
        {
            var cat =  _Categorie.getCategorieById(id);
            return cat;
        }


        [HttpPut]
        [Route("updateTicket")]
        public  IActionResult updateTicket(updateTicketDto ticket)
        {
           var result =   _Categorie.UpdateTicket(ticket);
           if ( result.Result )
            {
                return Ok("updated");
            }
           return BadRequest("failed to update");
        }

        [HttpPut]
        [Route("updateCategorie")]
        public IActionResult updateCategorie(updateCategorieDto categorie)
        {
            var result = _Categorie.UpdateCategorieAsync(categorie);
            if (result.Result)
            {
                return Ok("updated");
            }
            return BadRequest("failed to update");
        }

        [HttpDelete]
        [Route("DeleteCategorie")]
        public async Task<IActionResult> DeleteCategorie(string id)
        {
            if (id != null)
            {
                var result = await _Categorie.deleteCategorie(id);
                if (!result)
                {
                    return BadRequest("Categorie not found");
                }
                return Ok("Categorie deleted");

            }
            return BadRequest("provide an account id");

        }




    }
}

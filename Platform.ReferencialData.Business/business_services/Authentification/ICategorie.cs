using Platform.ReferentialData.DataModel.Authentification;
using Platform.ReferentialData.DtoModel.Authentification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferencialData.Business.business_services.Authentification
{
    public interface ICategorie
    {
        string GetAll(string id);
        Task<bool> addTicket(ticketDto ticketDto);
        Task<bool> UpdateTicket(updateTicketDto ticketDto);
        Task<bool> UpdateCategorieAsync(updateCategorieDto categorie);

        Task addCategorie(categorieDto categorie);
        Task<bool> deleteCategorie(string id);
        List<Ticket> getAllTicket(string id);
        string getCategorieById(string id);

    }
}

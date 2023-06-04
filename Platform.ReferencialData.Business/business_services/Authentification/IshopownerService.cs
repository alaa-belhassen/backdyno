using Platform.ReferentialData.DataModel.Authentification;
using Platform.ReferentialData.DtoModel.Authentification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferencialData.Business.business_services.Authentification
{
    public interface IshopownerService
    {
        Task addDemandePayement(demandePayementDto demande);
        Task<List<Object>> getAllDemandePayement();
        List<demandePayement> getDemandePayementByshopowner(string Id);
        bool deleteDemande(string id);


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferentialData.DtoModel.Authentification
{
    public class updateTicketDto
    {
        
        public string idTicket { get; set; }
        public string idEmployer{ get; set; }


        public string nameTicket { get; set; }
        public int prixTicket { get; set; }
    }
}

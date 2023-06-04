using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferentialData.DataModel.Authentification
{
    public class Ticket
    {
        [Key]
        public string IdTicket { get; set; }
        public string nameTicket {get;set;}
        public int prixTicket { get; set;}
        public bool status { get; set; }
        public string idEmployer { get; set; }

        public List<Categorie> categorie { get; set; }    
    }
}

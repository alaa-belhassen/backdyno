using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferentialData.DataModel.Authentification
{
    public class Categorie
    {
        [Key]
        public string IdCategorie { get; set; }
        public string NameCateforie { get; set;}
        public string idticket { get; set; }
        public string idEmployer { get; set; }
        public Ticket ticket { get; set; }
        public List<Employment> Employment { get; set; }

    }
}

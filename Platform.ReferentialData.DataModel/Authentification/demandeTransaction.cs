using login.models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferentialData.DataModel.Authentification
{
    public class demandeTransaction
    {
        [Key]
        public string IdDemandeTransaction { get; set; }
        public string IdEmployer { get; set; }
        public DateTime date { get; set; }
        public string amount { get; set; }
        public string etat { get; set; }

        public MoreInfoEmployer employer { get; set; }
    }
}

using login.models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferentialData.DataModel.Authentification
{
    public class TransactionTerminer
    {
        [Key]
        public string IdTransaction { get; set; }
        public string IdVerificateur { get; set; }
        public DateTime date { get; set; }
        public float montant { get; set; }
        public string etat { get; set; }

        public string IdEmployer { get; set; }
        public MoreInfoEmployer employer { get; set; }
    }
}

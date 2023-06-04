using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferentialData.DataModel.Authentification
{
    public class payement
    {
        [Key]
        public string idPayment { get; set; }

        public DateTime date { get; set; }
        public string dayOfwork { get; set; }
        public string idsociété { get; set;}

        public string idEmployer { get; set; }

        public string montant { get; set; }


    }
}

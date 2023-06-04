using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferentialData.DtoModel.Authentification
{
    public class TransactionDto
    {

        public string IdEmployer { get; set; }

        public int montant { get; set; }
    
        public string Idverificateur{ get; set; }

        public string iddemande { get; set; }


    }
}

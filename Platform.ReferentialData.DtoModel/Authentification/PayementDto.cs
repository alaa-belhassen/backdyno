using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferentialData.DtoModel.Authentification
{
    public class PayementDto
    {
        public string IdEmployer{ get; set; }
        public string IdEmployee { get; set; }

        public string dayofwork { get; set; }

        public string montant { get; set; }

    }
}

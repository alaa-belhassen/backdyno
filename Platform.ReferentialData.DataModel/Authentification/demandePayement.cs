using login.models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.models;

namespace Platform.ReferentialData.DataModel.Authentification
{
    public class demandePayement
    {
        [Key]
        public string IdDemandePayement { get; set; }
        public string IdShopowner { get; set; }
        public DateTime date { get; set; }
        public string amount { get; set; }
        public string etat { get; set; }
        public string IdValidateur { get; set; }
        public string walletValidateur { get; set; }

        public ShopOwner shopowner { get; set; }
    }
}

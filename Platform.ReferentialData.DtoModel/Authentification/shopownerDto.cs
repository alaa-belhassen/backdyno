using login.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferentialData.DtoModel.Authentification
{
    public class shopownerDto: UserRegistrationRequestDto
    {
        public string adresseFacturation { get; set; }
        public string codeTVA { get; set; }
        public string Adresse { get; set; }
        public string NumTel { get; set; }
        public string PaymentMethode { get; set; }
        public string matriculeFiscale { get; set; }
        public string commission { get; set;}
        public string delaiPayement { get; set;}
    }
}

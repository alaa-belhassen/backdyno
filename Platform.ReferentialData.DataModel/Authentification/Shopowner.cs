using login.models;
using Microsoft.AspNetCore.Identity;
using Platform.ReferentialData.DataModel.Authentification;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.models
{
    public class ShopOwner
    {
        [Key]
        public string moreInfoId { get; set; }
        public string codeTVA { get; set; }
        public string Adresse { get; set; }
        public string NumTel { get; set; }
        public string PaymentMethode { get; set; }
        public string adresseFacturation { get; set; }
        public string matriculeFiscale { get; set; }
        public string commission { get; set; }
        public string Blocked { get; set; }
        public string delaiPayement { get; set; }
        public employer3 employer { get; set; }
        public List<demandePayement> demandePayement { get; set; }

    }
}

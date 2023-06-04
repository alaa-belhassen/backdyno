using login.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Platform.ReferentialData.DataModel.Authentification
{
    public class accounts
    {
        [Key]
        public string idaccount { get; set; }
        public string secret { get; set; }
        public string iduser { get; set; }
        public employer3 user { get; set; }
    }
}

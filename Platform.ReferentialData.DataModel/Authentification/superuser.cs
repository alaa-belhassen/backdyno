using login.models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferentialData.DataModel.Authentification
{
    public class superuser
    {
        [Key]
        public  string Idsuperuser { get; set; }

        public employer3 user { get; set; }
    }

}

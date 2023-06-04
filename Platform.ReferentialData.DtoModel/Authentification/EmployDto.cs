using login.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferentialData.DtoModel.Authentification
{
    public class EmployDto 
    {
        public string mailEmployee { get; set; }
        public string IdEmployer { get; set; }

        public string IdCategorie { get; set; }
        
        public string Role { get; set; }
    }
}

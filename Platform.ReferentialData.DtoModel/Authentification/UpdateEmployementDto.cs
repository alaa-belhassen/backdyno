using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferentialData.DtoModel.Authentification
{
    public class UpdateEmployementDto
    {
        public string mailEmployee { get; set; }
        public string IdEmployer { get; set; }

        public string IdCategorie { get; set; }

        public string Role { get; set; }

        public string curentRole { get; set; }
    }
}

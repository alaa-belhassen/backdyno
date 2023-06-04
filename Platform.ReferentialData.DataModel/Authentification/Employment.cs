using login.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferentialData.DataModel.Authentification
{
    public class Employment
    {

     
        public string IdEmployee { get; set; }
        public string IdEmployer { get; set; }
        public string IdCategorie { get; set; }
        public Employee employee { get; set; }
        public MoreInfoEmployer employer { get; set; }
        public Categorie Categorie { get; set; }

    
    }
}

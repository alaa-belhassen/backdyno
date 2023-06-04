using login.models;
using Platform.ReferentialData.DataModel.Authentification;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferentialData.DataModel.Authentification
{
    public class Employee 
    {
        [Key]
        public string moreInfoId { get; set; }
        public string NumTel { get; set; }
        public bool activated { get; set; }

        public double balance { get; set; }
        public List<Employment> employment  { get; set; }
        public employer3 user { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferentialData.DataModel.Authentification
{
    public class DataResult
    {
        public bool Result { get; set; }
        public List<string> Errors { get; set; }

    }
}

using login.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.ReferentialData.DtoModel.Authentification
{
    public class EmployeeDto: UserRegistrationRequestDto
    {
        public string NumTel { get; set; }
    }
}

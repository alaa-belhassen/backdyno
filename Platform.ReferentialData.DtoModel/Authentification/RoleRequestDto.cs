using System.ComponentModel.DataAnnotations;

namespace login.models
{
    public class RoleRequestDto{
        [Required]
        public string RoleName{get;set;}

        public string idEmployer { get; set; }
    }

}
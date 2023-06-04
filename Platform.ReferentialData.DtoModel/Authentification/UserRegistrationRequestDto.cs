using System.ComponentModel.DataAnnotations;

namespace login.models
{
    public class UserRegistrationRequestDto{
        [Required]
        public string Name{get;set;}
        [Required]

        public string Password{get;set;}
        [Required]
        public string Email{get;set;}

        [Required]
        public string Role{get;set;}
        [Required]
        public string walletpublickey { get;set;}
    }

}
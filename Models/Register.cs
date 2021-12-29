//#nullable enable
using System.ComponentModel.DataAnnotations;

namespace BookMgtApi.Models
{
    public class Register
    { 
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Email { get; set; } 

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }  

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } 
    }
}
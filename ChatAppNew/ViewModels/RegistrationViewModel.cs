using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppNew.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        [EmailAddress]


        public string Email { get; set; }



        public string Name { get; set; }




        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password does not match!!")]
        public string ConfirmPassword { get; set; }

    }
}

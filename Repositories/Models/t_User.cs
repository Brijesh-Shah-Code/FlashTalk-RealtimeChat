using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class t_User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int c_UserId{get;set;}

        [Required(ErrorMessage = "User name is required")]
        [StringLength(150)]
        public string c_UserName{get;set;}

        [Required(ErrorMessage = "Email is required")]
        [StringLength(150)]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string c_Email{get;set;}

        [Required(ErrorMessage = "Password is required")]
        [StringLength(150)]

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string c_Password{get;set;}
    }
}
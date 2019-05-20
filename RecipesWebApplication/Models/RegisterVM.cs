using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RecipesWebApplication.Models
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "*")]
        public string Username { get; set; }
        [Required(ErrorMessage = "*")]
        public string UserEmail { get; set; }
        [Required(ErrorMessage = "*")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
        public string ReturnURL { get; set; }
        public bool isRemember { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipesWebApplication.Models
{
    public class LoginVm
    {       
            public string Username { get; set; }
            public string Password { get; set; }
            public string ReturnURL { get; set; }
            public bool isRemember { get; set; }        
    }
}
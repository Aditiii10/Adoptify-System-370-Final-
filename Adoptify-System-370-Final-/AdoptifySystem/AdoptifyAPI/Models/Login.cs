using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdoptifyAPI.Models
{
    public class Login
    {
       
            public int UserID { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public int Emp_ID { get; set; }
        
    }
}
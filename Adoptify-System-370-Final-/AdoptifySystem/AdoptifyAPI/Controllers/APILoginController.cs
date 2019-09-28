using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Net.Http;
using AdoptifyAPI.Models;

namespace AdoptifyAPI.Controllers
{
    public class APILoginController : Controller
    {
        private Wollies_ShelterEntities db = new Wollies_ShelterEntities();

        // GET: APILogin
        public Login Login(Login c)
        {
            var r = db.User_.Where(s => s.Username == c.Username && s.Password == c.Password).FirstOrDefault();
            if (r == null) return null;
            Login l = new Login();
            l.Username = r.Username; l.Password = r.Password;
            return l;
            
        }
    }
}
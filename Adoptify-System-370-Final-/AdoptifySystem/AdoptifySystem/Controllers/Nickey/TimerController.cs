﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AdoptifySystem.Controllers.Zinhle
{
    public class TimerController : Controller
    {
        // GET: Timer
        public ActionResult Index(User_ login)
        {
            //int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
            //var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
            //string encrypted = FormsAuthentication.Encrypt(ticket);
            //var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
            //cookie.Expires = DateTime.Now.AddMinutes(timeout);
            //cookie.HttpOnly = true;
            //Response.Cookies.Add(cookie);
            return View();
        }

        public ActionResult AddTimer()
        {
            return View();
        }
        public ActionResult DeleteTimer()
        {
            return View();
        }
        public ActionResult MaintainTimer()
        {
            return View();

        }
    }
}
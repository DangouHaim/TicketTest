using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TicketTest.Models;

namespace TicketTest.Controllers
{
    public class AuthController : Controller
    {
        private DBEntities db = new DBEntities();

        public ActionResult Index()
        {
            if(!string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["uid"].Value))
            {
                return Redirect("/Main/Index");
            }
            return View();
        }

        public ActionResult Authorize(string Mail, string Pass)
        {
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] checkSum = md5.ComputeHash(Encoding.UTF8.GetBytes(Pass));
                string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
                Pass = result;

                if (AuthCheckk(Mail, Pass))
                {
                    return Redirect("/Main/Index");
                }
            }
            catch { }
            return PartialView("FormError");
        }

        private bool AuthCheckk(string mail, string pass)
        {
            bool res = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(mail) &&
                !string.IsNullOrWhiteSpace(pass))
                {
                    foreach (var v in db.User)
                    {
                        if (v.Mail.ToLower().Trim() == mail.ToLower().Trim() &&
                            v.Pass.Trim() == pass.Trim())
                        {
                            HttpContext.Response.Cookies["uid"].Value = v.ID.ToString();
                            res = true;
                        }
                    }
                }
            }
            catch { }
            return res;
        }

        public ActionResult LogOut()
        {
            HttpContext.Response.Cookies["uid"].Value = "";
            return Redirect("/Home/Index");
        }
    }
}
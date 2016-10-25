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
    public class RegController : Controller
    {
        private DBEntities db = new DBEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string Name, string LName, string Mail, string Pass, string ComformPass, string BirthDate)
        {
            if(!string.IsNullOrWhiteSpace(Name) &&
                !string.IsNullOrWhiteSpace(LName) &&
                !string.IsNullOrWhiteSpace(Mail) &&
                !string.IsNullOrWhiteSpace(Pass) &&
                !string.IsNullOrWhiteSpace(ComformPass) &&
                !string.IsNullOrWhiteSpace(BirthDate))
            {
                if(!ThisUserExist(Mail) && Pass == ComformPass)
                {
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] checkSum = md5.ComputeHash(Encoding.UTF8.GetBytes(Pass));
                    string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
                    Pass = result;

                    User u = new User() { Name = Name, LName = LName, Mail = Mail, Pass = Pass, BirthDate = BirthDate };
                    db.User.Add(u);
                    db.SaveChanges();

                    return Redirect("/Auth/Index");
                }
            }
            return PartialView("FormError");
        }

        public bool ThisUserExist(string Mail)
        {
            bool res = false;
            foreach(var v in db.User)
            {
                if(v.Mail.ToLower().Trim() == Mail.ToLower().Trim())
                {
                    res = true;
                    break;
                }
            }
            return res;
        }
    }
}
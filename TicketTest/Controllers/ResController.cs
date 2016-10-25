using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using TicketTest.Models;

namespace TicketTest.Controllers
{
    public class ResController : Controller
    {
        private DBEntities db = new DBEntities();
        
        public ActionResult Index()
        {
            List<Order> res = new List<Order>();
            foreach(var v in db.Order)
            {
                if(v.OwnerID.ToString() == HttpContext.Request.Cookies["uid"].Value)
                {
                    res.Add(v);
                }
            }
            return View(res);
        }

        public ActionResult Reserve(string Flight)
        {
            if(!string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["uid"].Value))
            {
                int fid = -1;
                if(!string.IsNullOrWhiteSpace(Flight))
                {
                    fid = int.Parse(Flight);
                }
                Flight fl = db.Flight.Find(fid);

                if (fl.PlacesCount > 0)
                {
                    Order o = new Order()
                    {
                        FlightID = fid,
                        OwnerID = int.Parse(HttpContext.Request.Cookies["uid"].Value),
                        Status = "Reserved"
                    };
                    db.Order.Add(o);
                    db.SaveChanges();

                    if (fl != null)
                    {
                        fl.PlacesCount--;
                        db.SaveChanges();
                    }
                }
            }
            return Index();
        }

        public ActionResult Purcast(string Purcast)
        {
            int pc = -1;
            if(!string.IsNullOrWhiteSpace(Purcast))
            {
                pc = int.Parse(Purcast);
            }

            Order o = db.Order.Find(pc);
            if (o != null)
            {
                o.Status = "Purcasted";
                db.SaveChanges();

                Ticket t = new Ticket()
                {
                    OrderID = o.ID,
                    Status = o.Status,
                    UserNum = o.OwnerID,
                    UserName = db.User.Find(o.OwnerID).Name.Trim(),
                    UserLname = db.User.Find(o.OwnerID).LName.Trim(),
                    UserDocNum = o.ID
                };
                db.Ticket.Add(t);
                db.SaveChanges();
            }
            return Redirect("/Res/Index");
        }

        public ActionResult Show()
        {
            return Redirect("/Res/Index");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketTest.Models;

namespace TicketTest.Controllers
{
    public class MainController : Controller
    {
        private DBEntities db = new DBEntities();

        public ActionResult Index()
        {
            return View(db.BusStation.ToList());
        }

        public ActionResult BusStationSearch(string Query)
        {
            if (!string.IsNullOrWhiteSpace(Query))
            {
                List<BusStation> res = new List<BusStation>();
                foreach (var v in db.BusStation)
                {
                    if (v.Name.ToLower().IndexOf(Query.ToLower()) > -1)
                    {
                        res.Add(v);
                    }
                }
                return View(res);
            }
            return View(db.BusStation.ToList());
        }

        public ActionResult FlightSearch(string StartPointName, string EndPointName, string Date)
        {
            if (!string.IsNullOrWhiteSpace(StartPointName) ||
                !string.IsNullOrWhiteSpace(EndPointName) ||
                !string.IsNullOrWhiteSpace(Date))
            {
                int startPID = GetBusStateIDByName(StartPointName), 
                    endPID = GetBusStateIDByName(EndPointName);

                List<Flight> res = new List<Flight>();

                foreach(var v in db.Flight)
                {
                    int AddLim = 0;
                    if (startPID == -1)
                    {
                        AddLim++;
                    }
                    else
                    {
                        if(v.StartBusStationID == startPID)
                        {
                            AddLim++;
                        }
                    }
                    if (endPID == -1)
                    {
                        AddLim++;
                    }
                    else
                    {
                        if(v.EndBusStationID == endPID)
                        {
                            AddLim++;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(Date))
                    {
                        AddLim++;
                    }
                    else
                    {
                        if(v.DateTimeFrom == Date || v.DateTimeTo == Date)
                        {
                            AddLim++;
                        }
                    }
                    if (AddLim == 3)
                    {
                        res.Add(v);
                    }
                }

                return View(res);
            }
            return View(db.Flight.ToList());
        }

        private int GetBusStateIDByName(string Name)
        {
            int res = -1;
            foreach(var v in db.BusStation)
            {
                if(v.Name.ToLower().IndexOf(Name.ToLower().Trim()) > -1)
                {
                    res = v.ID;
                }
            }
            return res;
        }

        public ActionResult ReserveOrPurcast()
        {
            return Redirect("/Res/Index");
        }
    }
}
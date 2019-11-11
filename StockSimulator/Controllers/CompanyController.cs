using StockSimulator.Models;
using StockSimulator.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockSimulator.Controllers
{
    public class CompanyController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Company
        public ActionResult Index()
        {
            var companies = from c in db.Companies
                            orderby c.ID
                            select c;
            return View(companies);
        }

        // GET: Company?tickerSymbol=AAPL
        [HttpGet]
        public ActionResult Index(string tickerSymbol)
        {
            var companies = from c in db.Companies
                            where c.TickerSymbol.Contains(tickerSymbol)
                            orderby c.ID
                            select c;
            return View(companies);
        }

        // GET: Company/Details/5
        public ActionResult Details(int id, string ticker)
        {
            return RedirectToAction("Search", "Stock", new { tickerSymbol = ticker });
        }

        // GET: Company/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Company/Create
        [HttpPost]
        public ActionResult Create(Company c)
        {
            try
            {
                db.Companies.Add(c);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Company/Edit/5
        public ActionResult Edit(int id)
        {
            Company c = db.Companies.Single(m => m.ID == id);
            return View(c);
        }

        // POST: Company/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Company c = db.Companies.Single(m => m.ID == id);
                if (TryUpdateModel(c))
                {
                    //TODO: database code
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(c);
            }
            catch
            {
                return View();
            }
        }

        // GET: Company/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                Company c = db.Companies.Single(m => m.ID == id);
                db.Companies.Remove(c);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Company/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Company c = db.Companies.Single(m => m.ID == id);
                db.Companies.Remove(c);
                if (TryUpdateModel(c))
                {
                    //TODO: database code
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(c);
            }
            catch
            {
                return View();
            }
        }

        [NonAction]
        public List<Company> GetCompanyList()
        {
            return new List<Company>{
                new Company{
                    ID = 1,
                    CompanyName = "Alphabet Inc",
                    TickerSymbol = "GOOG",
                    StartDataDate = DateTime.Parse(DateTime.Today.ToString())
                },

                new Company{
                    ID = 2,
                    CompanyName = "Apple Computer Inc",
                    TickerSymbol = "AAPL",
                    StartDataDate = DateTime.Parse(DateTime.Today.ToString())
                },

                new Company{
                    ID = 3,
                    CompanyName = "Facebook Inc",
                    TickerSymbol = "FB",
                    StartDataDate = DateTime.Parse(DateTime.Today.ToString())
                },

                new Company{
                    ID = 4,
                    CompanyName = "Intel Corporation",
                    TickerSymbol = "INTC",
                    StartDataDate = DateTime.Parse(DateTime.Today.ToString())
                },

                new Company{
                    ID = 5,
                    CompanyName = "Microsoft Corporation",
                    TickerSymbol = "MSFT",
                    StartDataDate = DateTime.Parse(DateTime.Today.ToString())
                },
            };
        }
    }
}

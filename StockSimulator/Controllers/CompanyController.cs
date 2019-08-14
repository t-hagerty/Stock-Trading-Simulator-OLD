using StockSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockSimulator.Controllers
{
    public class CompanyController : Controller
    {
        // GET: Company
        public ActionResult Index()
        {
            var companies = from c in GetCompanyList()
                            orderby c.ID
                            select c;
            return View(companies);
        }

        // GET: Company/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Company/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Company/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

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
            return View();
        }

        // POST: Company/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Company/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Company/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
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

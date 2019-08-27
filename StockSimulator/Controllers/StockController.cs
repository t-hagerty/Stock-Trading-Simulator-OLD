using StockSimulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockSimulator.Controllers
{
    public class StockController : Controller
    {
        private StockDBContext db = new StockDBContext();

        // GET: Stock
        public ActionResult Index()
        {
            var stocks = from s in db.StockCandlesticks
                            orderby s.ID
                            select s;
            return View(stocks);
        }

        // GET: Stock/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Stock/Create
        public ActionResult Create()
        {
            CompanyDBContext cdb = new CompanyDBContext();
            ViewBag.CompanyID = new SelectList(cdb.Companies, "ID", "TickerSymbol");
            return View();
        }

        // POST: Stock/Create
        [HttpPost]
        public ActionResult Create(StockCandlestick s)
        {
            CompanyDBContext cdb = new CompanyDBContext();
            ViewBag.CompanyID = new SelectList(cdb.Companies, "ID", "TickerSymbol");
            try
            {
                s.Company = cdb.Companies.Find(s.CompanyId);
                db.StockCandlesticks.Add(s);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Stock/Edit/5
        public ActionResult Edit(int id)
        {
            CompanyDBContext cdb = new CompanyDBContext();
            ViewBag.CompanyID = new SelectList(cdb.Companies, "ID", "TickerSymbol");
            StockCandlestick s = db.StockCandlesticks.Single(m => m.ID == id);
            return View(s);
        }

        // POST: Stock/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            CompanyDBContext cdb = new CompanyDBContext();
            ViewBag.CompanyID = new SelectList(cdb.Companies, "ID", "TickerSymbol");
            try
            {
                StockCandlestick s = db.StockCandlesticks.Single(m => m.ID == id);
                if(TryUpdateModel(s))
                {
                    //TODO: database code
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(s);
            }
            catch
            {
                return View();
            }
        }

        // GET: Stock/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                StockCandlestick s = db.StockCandlesticks.Single(m => m.ID == id);
                db.StockCandlesticks.Remove(s);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Stock/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                StockCandlestick s = db.StockCandlesticks.Single(m => m.ID == id);
                db.StockCandlesticks.Remove(s);
                if (TryUpdateModel(s))
                {
                    //TODO: database code
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(s);
            }
            catch
            {
                return View();
            }
        }
    }
}

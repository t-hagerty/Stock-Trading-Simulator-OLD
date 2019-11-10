using StockSimulator.Models;
using StockSimulator.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace StockSimulator.Controllers
{
    public class StockController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Stock
        public async Task<ActionResult> Index()
        {
            await db.RetrieveStockDataDayMinutes("AAPL");
            var stocks = from s in db.StockCandlesticks
                            orderby s.ID
                            select s;
            return View(stocks.ToList());
        }

        // GET: Stock/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Stock/Create
        public ActionResult Create()
        {
            ViewBag.CompanyID = new SelectList(db.Companies, "ID", "TickerSymbol");
            return View();
        }

        // POST: Stock/Create
        [HttpPost]
        public ActionResult Create(StockCandlestick s)
        {
            ViewBag.CompanyID = new SelectList(db.Companies, "ID", "TickerSymbol");
            try
            {
                s.Company = db.Companies.Find(s.CompanyId);
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
            ViewBag.CompanyID = new SelectList(db.Companies, "ID", "TickerSymbol");
            StockCandlestick s = db.StockCandlesticks.Single(m => m.ID == id);
            return View(s);
        }

        // POST: Stock/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            ViewBag.CompanyID = new SelectList(db.Companies, "ID", "TickerSymbol");
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

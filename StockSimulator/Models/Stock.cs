using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSimulator.Models
{
    /*
     * Provides a history of stocks bought/sold in the past as well as a history for currently owned stocks.
     */
    public class Stock
    {
        public int ID { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int CompanyID { get; set; }
        public virtual Company Company { get; set; }
        public int Quantity { get; set; } //Negative is the user sold stocks, positive if the user bought stocks.
        public decimal Price { get; set; }
        public DateTime PurchasedTime { get; set; }
        public DateTime SoldTime { get; set; } //null if not sold and the user currently owns this stock.
    }
}
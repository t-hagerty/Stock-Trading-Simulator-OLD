using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockSimulator.Models
{
    /*
     * Models orders that haven't been completed yet in a separate table from stocks for ease of seeing if we can complete unfulfilled orders regularly
     */
    public class Order
    {
        public int ID { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public int CompanyID { get; set; }
        public virtual Company Company { get; set; }
        public int Quantity { get; set; } //Negative if user is selling owned stocks, positive if user is buying stocks.
        public decimal PriceLimit { get; set; } //If selling stocks, this stores the minimum price, if buying, this stores the maximum price the user is willing to pay.
        public DateTime OrderPlacedTime { get; set; }
        
    }
}
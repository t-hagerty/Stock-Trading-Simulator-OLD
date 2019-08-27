using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace StockSimulator.Models
{
    public class StockCandlestick
    {
        public int ID { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public int Volume { get; set; }
    }

    public class StockDBContext : DbContext
    {
        public StockDBContext()
        { }
        public DbSet<StockCandlestick> StockCandlesticks { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
    }
}
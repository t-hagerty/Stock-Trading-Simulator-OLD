using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace StockSimulator.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        
        public decimal Funds { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockCandlestick> StockCandlesticks { get; set; }

        private IEX_DataSource stockDataSource = new IEX_DataSource();

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public void RetrieveStockData(string tickerSymbol)
        {
            //TODO: add code to check if valid ticker symbol

            //TODO: add code to check the database first if we already have this data
            //this method doesnt check a range and only gets current data, but it could still apply for when the market is closed i guess?
            StockCandlestick stockCandlestick = stockDataSource.GetStockData(tickerSymbol).Result;
            if(stockCandlestick != null)
            {
                stockCandlestick.Timestamp = System.DateTime.Now;
                try
                {
                    stockCandlestick.Company = Companies.Single(c => c.TickerSymbol == tickerSymbol);
                }
                catch(System.Exception e) //Matching company not found in DB
                {
                    //TODO: maybe add code here later to make an api call to get company details of the ticker symbol and add to company DB
                    //(the api obviously says it's a real company if we've reached this point)
                    return;
                }
                stockCandlestick.CompanyId = stockCandlestick.Company.ID;
                StockCandlesticks.Add(stockCandlestick);
                SaveChanges();
            }
        }

        public void RetrieveStockDataFromRange(string tickerSymbol, string range)
        {
            StockCandlesticks.AddRange(stockDataSource.GetStockDataRange(tickerSymbol, range).Result);
        }

        public async Task RetrieveStockDataDayMinutes(string tickerSymbol)
        {
            //TODO: add code to check if valid ticker symbol

            //TODO: add code to check the database first if we already have this data
            var result = await stockDataSource.GetStockDataDayMinutes(tickerSymbol);
            if (result != null && result.Count() != 0)
            {
                Company company = Companies.Single(c => c.TickerSymbol == tickerSymbol);
                foreach (var s in result)
                {
                    s.Company = company;
                    s.CompanyId = company.ID;
                }
                StockCandlesticks.AddRange(result);
                SaveChanges();
            }
        }

        public void RetrieveStockDataDate(string tickerSymbol, System.DateTime date)
        {
            StockCandlesticks.Add(stockDataSource.GetStockDataDate(tickerSymbol, date).Result);
        }
    }
}
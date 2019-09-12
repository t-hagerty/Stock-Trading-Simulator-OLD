using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StockSimulator.Models;

namespace StockSimulator.DAL
{
    public class MarketInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var companies = new List<Company>
            {
                new Company{CompanyName="Facebook Inc.",TickerSymbol="FB",StartDataDate=DateTime.Now},
                new Company{CompanyName="Apple Inc.",TickerSymbol="AAPL",StartDataDate=DateTime.Now},
                new Company{CompanyName="Amazon.com, Inc.",TickerSymbol="AMZN",StartDataDate=DateTime.Now},
                new Company{CompanyName="Netflix Inc.",TickerSymbol="NFLX",StartDataDate=DateTime.Now},
                new Company{CompanyName="Alphabet Inc.",TickerSymbol="GOOG",StartDataDate=DateTime.Now},
                new Company{CompanyName="Acme Corporation",TickerSymbol="ACME",StartDataDate=DateTime.Parse("8/29/2019 12:00 AM")}
            };

            companies.ForEach(c => context.Companies.Add(c));
            context.SaveChanges();
        }
    }

}
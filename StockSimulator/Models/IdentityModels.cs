﻿using System;
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

        public async Task<StockCandlestick> RetrieveStockData(string tickerSymbol)
        {
            Company company;
            try
            {
                company = Companies.Single(c => c.TickerSymbol == tickerSymbol);
            }
            catch (System.Exception e) //Matching company not found in DB
            {
                company = await stockDataSource.GetCompanyDetails(tickerSymbol);
                Companies.Add(company);
                SaveChanges();
            }

            //code to check the database first if we already have this data
            //this method doesnt check a range and only gets current data, but it could still apply for when the market is closed i guess?
            StockCandlestick result;
            try
            {
                //TODO: code to change datetime to closing time if closed
                DateTime time = DateTime.Now;
                result = StockCandlesticks.Single(s => s.Company == company && s.Timestamp == time);
                return result;
            }
            catch (System.Exception e) //Matching stock candlestick data not found in DB, need to get it
            {
                result = await stockDataSource.GetStockData(tickerSymbol);
                if (result != null)
                {
                    result.Timestamp = System.DateTime.Now;
                    result.Company = company;
                    result.CompanyId = result.Company.ID;
                    StockCandlesticks.Add(result);
                    SaveChanges();
                    return result;
                }
                return null;
            }
        }

        public async Task RetrieveStockDataFromRange(string tickerSymbol, string range)
        {
            Company company;
            try
            {
                company = Companies.Single(c => c.TickerSymbol == tickerSymbol);
            }
            catch (System.Exception e) //Matching company not found in DB
            {
                company = await stockDataSource.GetCompanyDetails(tickerSymbol);
                Companies.Add(company);
                SaveChanges();
            }

            //TODO: add code to check the database first if we already have this data
            var result = await stockDataSource.GetStockDataRange(tickerSymbol, range);
            if (result != null && result.Count() != 0)
            {
                foreach (var s in result)
                {
                    s.Company = company;
                    s.CompanyId = company.ID;
                    try
                    {
                        StockCandlesticks.Single(sc => sc.CompanyId == s.CompanyId && sc.Timestamp == s.Timestamp);
                    }
                    catch (System.InvalidOperationException e)
                    {
                        //No records (or hopefully not multiple!) in DB matching s, so let's add it
                        StockCandlesticks.Add(s);
                    }
                }
                SaveChanges();
            }
        }

        public async Task RetrieveStockDataDayMinutes(string tickerSymbol)
        {
            Company company;
            try
            {
                company = Companies.Single(c => c.TickerSymbol == tickerSymbol);
            }
            catch (System.Exception e) //Matching company not found in DB
            {
                company = await stockDataSource.GetCompanyDetails(tickerSymbol);
                Companies.Add(company);
                SaveChanges();
            }

            //TODO: add code to check the database first if we already have this data
            var result = await stockDataSource.GetStockDataDayMinutes(tickerSymbol);
            if (result != null && result.Count() != 0)
            {
                foreach (var s in result)
                {
                    s.Company = company;
                    s.CompanyId = company.ID;
                    try
                    {
                        StockCandlesticks.Single(sc => sc.CompanyId == s.CompanyId && sc.Timestamp == s.Timestamp);
                    }
                    catch(System.InvalidOperationException e)
                    {
                        //No records (or hopefully not multiple!) in DB matching s, so let's add it
                        StockCandlesticks.Add(s);
                    }
                }
                SaveChanges();
            }
        }

        public async Task RetrieveStockDataDate(string tickerSymbol, System.DateTime date)
        {
            Company company;
            try
            {
                company = Companies.Single(c => c.TickerSymbol == tickerSymbol);
            }
            catch (System.Exception e) //Matching company not found in DB
            {
                company = await stockDataSource.GetCompanyDetails(tickerSymbol);
                Companies.Add(company);
                SaveChanges();
            }

            //TODO: add code to check the database first if we already have this data
            date = date.AddHours(16);
            var result = await stockDataSource.GetStockDataDate(tickerSymbol, date);
            if(result == null)
            {
                return;
            }
            result.Timestamp = date;
            result.Company = company;
            result.CompanyId = company.ID;
            try
            {
                StockCandlesticks.Single(sc => sc.CompanyId == result.CompanyId && sc.Timestamp == result.Timestamp);
            }
            catch (System.InvalidOperationException e)
            {
                //No records (or hopefully not multiple!) in DB matching s, so let's add it
                StockCandlesticks.Add(result);
                SaveChanges();
            }
        }
    }
}
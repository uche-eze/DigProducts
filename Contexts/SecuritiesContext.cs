using DigitalProductAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalProductAPI.Contexts
{
    public class SecuritiesContext : DbContext
    {
        public DbSet<TreasuryBill> TreasuryBills { get; set; }
        public DbSet<Bond> Bonds { get; set; }
        public DbSet <Currency> Currencies { get; set; }

        public SecuritiesContext(DbContextOptions<SecuritiesContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }
    }
}

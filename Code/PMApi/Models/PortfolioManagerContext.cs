using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace PMApi.Models
{
    public class PortfolioManagerContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public PortfolioManagerContext() : base("PortfolioManagerContext")
        {
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            Database.SetInitializer<PortfolioManagerContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //DONT DO THIS ANYMORE
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Vote>().ToTable("Votes")
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //Portfolio -> Portoflio Positions one to many relationship
             /*modelBuilder.Entity<PortfolioPosition>()
                    .HasRequired<Portfolio>(s => s.PortfolioId)
                    .WithMany(s => s.PortfolioPositions)
                    .HasForeignKey(s => s.PortfolioId);*/
            /*modelBuilder.Entity<PortfolioPosition>()
               .HasRequired(e => e.Portoflio)
               .WithMany(d => d.PortfolioPositions);*/

            /*modelBuilder.Entity<PortfolioPosition>()
                        .HasOptional(e => e.Portoflio)
                        .WithMany(d => d.PortfolioPositions)
                        .HasForeignKey(e => e.PortfolioId);*/
        }
        public DbSet<PriceValue> PriceValues { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<PortfolioPosition> PortfolioPositions { get; set; }

        public DbSet<Valuation> Valuations { get; set; }
        public DbSet<ValuationDetail> ValuationDetails { get; set; }

    }
}
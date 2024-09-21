using Microsoft.EntityFrameworkCore;
using MigrationHelper.Models;

namespace MigrationHelper.Db;



public class MigHelperCtx : DbContext
    {

         public MigHelperCtx()
    {
      
    }

        public MigHelperCtx (DbContextOptions<MigHelperCtx> options)
            : base(options)
        {
        }

         protected override void OnConfiguring(DbContextOptionsBuilder options) {
         options.UseSqlite($"Data Source=MigHelper.db");
  
        }

        public DbSet<PayPeriod> PayPeriods { get; set; }
         public DbSet<GccNames> GccNames { get; set; }

         public DbSet<ScoreCache> ScoreCache { get; set; }

         public DbSet<ScoreBreakdown> ScoreBreakdown { get; set; }

         public DbSet<Countries> Countries { get; set; }

      
    }

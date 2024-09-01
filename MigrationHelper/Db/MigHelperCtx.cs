using Microsoft.EntityFrameworkCore;
using MigrationHelper.Models;

namespace MigrationHelper.Db;



public class MigHelperCtx : DbContext
    {
        public string DbPath { get; }

         public MigHelperCtx()
    {
        DbPath =  "MigHelper.db";
      
    }

        public MigHelperCtx (DbContextOptions<MigHelperCtx> options)
            : base(options)
        {
        }

         protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

        public DbSet<PayPeriod> PayPeriods { get; set; }

    }

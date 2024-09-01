using Microsoft.EntityFrameworkCore;
using MigrationHelper.Models;

namespace MigrationHelper.Db;



public class MigHelperCtx : DbContext
    {
        public string DbPath { get; }

         public MigHelperCtx()
    {
        DbPath =  "MigHelper.db";
        Console.WriteLine($"path {DbPath}");
    }

        public MigHelperCtx (DbContextOptions<MigHelperCtx> options)
            : base(options)
        {
        }

         protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

        public DbSet<PayPeriodGcc> PayPeriodGccs { get; set; }
        public DbSet<PayPeriod> PayPeriods { get; set; }

    }

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MigrationHelper.Db;

#nullable disable

namespace MigrationHelper.Migrations
{
    [DbContext(typeof(MigHelperCtx))]
    partial class MigHelperCtxModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("MigrationHelper.Models.GccNames", b =>
                {
                    b.Property<int>("GccNamesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Gcc")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Migrated")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Month")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("GccNamesId");

                    b.ToTable("GCCNames");
                });

            modelBuilder.Entity("MigrationHelper.Models.PayPeriod", b =>
                {
                    b.Property<int>("PayPeriodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Close")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CutOff")
                        .HasColumnType("TEXT");

                    b.Property<string>("Gcc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Lcc")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Open")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PayDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("PayGroup")
                        .HasColumnType("TEXT");

                    b.HasKey("PayPeriodId");

                    b.ToTable("PayPeriods");
                });

            modelBuilder.Entity("MigrationHelper.Models.ScoreBreakdown", b =>
                {
                    b.Property<int>("ScoreBreakdownId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Day")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Gcc")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Message")
                        .HasColumnType("TEXT");

                    b.Property<int>("Month")
                        .HasColumnType("INTEGER");

                    b.Property<ushort>("Sc")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("ScoreBreakdownId");

                    b.ToTable("ScoreBreakdown");
                });

            modelBuilder.Entity("MigrationHelper.Models.ScoreCache", b =>
                {
                    b.Property<int>("ScoreCacheId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Color")
                        .HasColumnType("TEXT");

                    b.Property<int>("Day")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Gcc")
                        .HasColumnType("TEXT");

                    b.Property<int>("Month")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Percentage")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Score")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("ScoreCacheId");

                    b.ToTable("ScoreCache");
                });
#pragma warning restore 612, 618
        }
    }
}

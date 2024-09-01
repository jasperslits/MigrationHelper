using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigrationHelper.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PayPeriodGccs",
                columns: table => new
                {
                    PayPeriodGccId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Gcc = table.Column<string>(type: "TEXT", nullable: false),
                    PayGroup = table.Column<string>(type: "TEXT", nullable: false),
                    CutOff = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PayDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Open = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Close = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayPeriodGccs", x => x.PayPeriodGccId);
                });

            migrationBuilder.CreateTable(
                name: "PayPeriods",
                columns: table => new
                {
                    PayPeriodId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Gcc = table.Column<string>(type: "TEXT", nullable: false),
                    Lcc = table.Column<string>(type: "TEXT", nullable: false),
                    PayGroup = table.Column<string>(type: "TEXT", nullable: false),
                    Open = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Close = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CutOff = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PayDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayPeriods", x => x.PayPeriodId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayPeriodGccs");

            migrationBuilder.DropTable(
                name: "PayPeriods");
        }
    }
}

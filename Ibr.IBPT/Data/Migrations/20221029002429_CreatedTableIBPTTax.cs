using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ibr.IBPT.Data.Migrations
{
    public partial class CreatedTableIBPTTax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IBPTTaxes",
                columns: table => new
                {
                    UF = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Ex = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    FederalNational = table.Column<decimal>(type: "numeric", nullable: false),
                    ImportedFederal = table.Column<decimal>(type: "numeric", nullable: false),
                    State = table.Column<decimal>(type: "numeric", nullable: false),
                    Municipal = table.Column<decimal>(type: "numeric", nullable: false),
                    StartValidity = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndValidity = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IBPTTaxes", x => new { x.UF, x.Code, x.Ex });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IBPTTaxes");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyContext.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuoteProperties",
                columns: table => new
                {
                    QuotePropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SomeQuotePropertyData = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteProperties", x => x.QuotePropertyId);
                });

            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    QuoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SomeQuoteData = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    SomeMoreQuoteData = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ComputedColumn = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, computedColumnSql: "(case when [SomeQuoteData]>[SomeMoreQuoteData] then [SomeQuoteData] else [SomeMoreQuoteData] end)", stored: true),
                    QuotePropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnotherQuotePropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.QuoteId);
                    table.ForeignKey(
                        name: "FK1",
                        column: x => x.QuotePropertyId,
                        principalTable: "QuoteProperties",
                        principalColumn: "QuotePropertyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK2",
                        column: x => x.AnotherQuotePropertyId,
                        principalTable: "QuoteProperties",
                        principalColumn: "QuotePropertyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_AnotherQuotePropertyId",
                table: "Quotes",
                column: "AnotherQuotePropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_QuotePropertyId",
                table: "Quotes",
                column: "QuotePropertyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.DropTable(
                name: "QuoteProperties");
        }
    }
}

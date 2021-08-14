using Microsoft.EntityFrameworkCore.Migrations;

namespace Forex.Migrations
{
    public partial class Init4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StockCode",
                table: "Stocks",
                newName: "StockName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StockName",
                table: "Stocks",
                newName: "StockCode");
        }
    }
}

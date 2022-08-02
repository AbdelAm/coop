using Microsoft.EntityFrameworkCore.Migrations;

namespace coop2._0.Migrations
{
    public partial class addmotiftotransactiontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Motif",
                table: "Transactions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Motif",
                table: "Transactions");
        }
    }
}

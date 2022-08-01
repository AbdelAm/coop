using Microsoft.EntityFrameworkCore.Migrations;

namespace coop2._0.Migrations
{
    public partial class updatebankaccountattributesaddstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "BankAccounts");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "BankAccounts",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "BankAccounts");

            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "BankAccounts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}

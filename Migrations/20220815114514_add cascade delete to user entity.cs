using Microsoft.EntityFrameworkCore.Migrations;

namespace coop2._0.Migrations
{
    public partial class addcascadedeletetouserentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_AspNetUsers_UserId",
                table: "BankAccounts");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_AspNetUsers_UserId",
                table: "BankAccounts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_AspNetUsers_UserId",
                table: "BankAccounts");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_AspNetUsers_UserId",
                table: "BankAccounts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

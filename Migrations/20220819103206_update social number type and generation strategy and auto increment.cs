using Microsoft.EntityFrameworkCore.Migrations;

namespace coop2._0.Migrations
{
    public partial class updatesocialnumbertypeandgenerationstrategyandautoincrement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE `aspnetusers` MODIFY COLUMN `SocialNumber` bigint AUTO_INCREMENT UNIQUE;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

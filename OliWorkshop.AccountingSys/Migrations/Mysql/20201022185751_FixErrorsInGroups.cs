using Microsoft.EntityFrameworkCore.Migrations;

namespace OliWorkshop.AccountingSys.Migrations.Mysql
{
    public partial class FixErrorsInGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EarnCategoryId",
                table: "CountableGroup");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "EarnCategoryId",
                table: "CountableGroup",
                type: "int unsigned",
                nullable: false,
                defaultValue: 0u);
        }
    }
}

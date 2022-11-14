using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorldOfPowerTools.DAL.SqlServer.Migrations
{
    public partial class ChangeModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Availablity",
                table: "Products",
                newName: "Availability");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Availability",
                table: "Products",
                newName: "Availablity");
        }
    }
}

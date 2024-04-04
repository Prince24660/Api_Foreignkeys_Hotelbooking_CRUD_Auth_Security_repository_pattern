using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Foreignkey.Migrations
{
    public partial class alterid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "id",
                table: "employees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "employees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

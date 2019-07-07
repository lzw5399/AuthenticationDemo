using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityDemo.Migrations
{
    public partial class addCustomerTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerTag",
                table: "AspNetUsers",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerTag",
                table: "AspNetUsers");
        }
    }
}

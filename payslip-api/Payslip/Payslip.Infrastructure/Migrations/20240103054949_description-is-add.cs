using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payslip.Infrastructure.Migrations
{
    public partial class descriptionisadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Bank",
                table: "UserPayslips",
                newName: "DescriptionsAmount");

            migrationBuilder.AddColumn<string>(
                name: "Descriptions",
                table: "UserPayslips",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descriptions",
                table: "UserPayslips");

            migrationBuilder.RenameColumn(
                name: "DescriptionsAmount",
                table: "UserPayslips",
                newName: "Bank");
        }
    }
}

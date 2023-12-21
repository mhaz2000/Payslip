using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payslip.Infrastructure.Migrations
{
    public partial class userpayslipisadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersonnelCode",
                table: "AspNetUsers",
                newName: "CardNumber");

            migrationBuilder.CreateTable(
                name: "UserPayslips",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalSalaryAndBenefits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalDeductions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetPayable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    SalaryAndBenefits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Durations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalaryAndBenefitsAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deductions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeductionsAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPayslips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPayslips_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPayslips_UserId",
                table: "UserPayslips",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPayslips");

            migrationBuilder.RenameColumn(
                name: "CardNumber",
                table: "AspNetUsers",
                newName: "PersonnelCode");
        }
    }
}

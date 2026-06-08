using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemBibliotekiOnline.Migrations
{
    public partial class FixDueDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE Loans
                SET DueDate = DATEADD(day, 30, LoanDate)
                WHERE DueDate = '0001-01-01';
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
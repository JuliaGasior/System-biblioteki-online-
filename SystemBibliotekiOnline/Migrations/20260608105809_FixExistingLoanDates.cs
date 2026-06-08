using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemBibliotekiOnline.Migrations
{
    /// <inheritdoc />
    public partial class FixExistingLoanDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        UPDATE Loans
        SET DueDate = DATEADD(day, 30, LoanDate)
        WHERE YEAR(DueDate) = 1;
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}

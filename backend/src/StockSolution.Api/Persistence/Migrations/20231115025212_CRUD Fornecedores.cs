using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockSolution.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CRUDFornecedores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Code",
                table: "Suppliers",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Suppliers_Code",
                table: "Suppliers");
        }
    }
}

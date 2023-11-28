using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StockSolution.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AjustandoPKProdutoProdutoComercial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductComercialProducts",
                table: "ProductComercialProducts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SellingDate",
                table: "Sales",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ProductComercialProducts",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductComercialProducts",
                table: "ProductComercialProducts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComercialProducts_ProductId",
                table: "ProductComercialProducts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductComercialProducts",
                table: "ProductComercialProducts");

            migrationBuilder.DropIndex(
                name: "IX_ProductComercialProducts_ProductId",
                table: "ProductComercialProducts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SellingDate",
                table: "Sales",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ProductComercialProducts",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductComercialProducts",
                table: "ProductComercialProducts",
                columns: new[] { "ProductId", "ComercialProductId" });
        }
    }
}

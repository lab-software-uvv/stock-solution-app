﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockSolution.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ProdutoProdutoComercial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "ProductComercialProducts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ProductComercialProducts");
        }
    }
}

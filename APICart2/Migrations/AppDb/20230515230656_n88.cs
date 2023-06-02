using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APICart2.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class n88 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
            name: "BarCode",
            table: "Products",
            nullable: true);

            migrationBuilder.UpdateData(
            table: "Products",
            keyColumn: "ProductId",
            keyValue: 1,
            column: "BarCode",
            value: "PB-11");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                column: "BarCode",
                value: "PB-12");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                column: "BarCode",
                value: "PB-13");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                column: "BarCode",
                value: "PB-14");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                column: "BarCode",
                value: "PB-15");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6,
                column: "BarCode",
                value: "PE-31");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7,
                column: "BarCode",
                value: "PE-32");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8,
                column: "BarCode",
                value: "PE-33");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9,
                column: "BarCode",
                value: "PE-34");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10,
                column: "BarCode",
                value: "PE-35");

            migrationBuilder.UpdateData(
               table: "Products",
               keyColumn: "ProductId",
               keyValue: 11,
               column: "BarCode",
               value: "PE-36");

            migrationBuilder.UpdateData(
               table: "Products",
               keyColumn: "ProductId",
               keyValue: 12,
               column: "BarCode",
               value: "PF-21");

            migrationBuilder.UpdateData(
               table: "Products",
               keyColumn: "ProductId",
               keyValue: 13,
               column: "BarCode",
               value: "PF-22");

            migrationBuilder.UpdateData(
               table: "Products",
               keyColumn: "ProductId",
               keyValue: 14,
               column: "BarCode",
               value: "PF-23");

            migrationBuilder.UpdateData(
               table: "Products",
               keyColumn: "ProductId",
               keyValue: 15,
               column: "BarCode",
               value: "PF-24");

            migrationBuilder.UpdateData(
               table: "Products",
               keyColumn: "ProductId",
               keyValue: 16,
               column: "BarCode",
               value: "PF-25");

            migrationBuilder.UpdateData(
               table: "Products",
               keyColumn: "ProductId",
               keyValue: 17,
               column: "BarCode",
               value: "PF-26");

            migrationBuilder.UpdateData(
               table: "Products",
               keyColumn: "ProductId",
               keyValue: 18,
               column: "BarCode",
               value: "PC-41" );

            migrationBuilder.UpdateData(
               table: "Products",
               keyColumn: "ProductId",
               keyValue: 19,
               column: "BarCode",
               value: "PC-42");

            migrationBuilder.UpdateData(
               table: "Products",
               keyColumn: "ProductId",
               keyValue: 20,
               column: "BarCode",
               value: "PC-43");

            migrationBuilder.UpdateData(
               table: "Products",
               keyColumn: "ProductId",
               keyValue: 21,
               column: "BarCode",
               value: "PC-44");

            migrationBuilder.UpdateData(
               table: "Products",
               keyColumn: "ProductId",
               keyValue: 22,
               column: "BarCode",
               value: "PC-45");

            migrationBuilder.UpdateData(
               table: "Products",
               keyColumn: "ProductId",
               keyValue: 23,
               column: "BarCode",
               value: "PC-46");






        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropColumn(
            name: "BarCode",
            table: "Products");



        }
    }
}

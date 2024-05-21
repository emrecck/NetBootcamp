using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetBootcamp.API.Migrations
{
    /// <inheritdoc />
    public partial class check : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Barcode", "Created", "Stock" },
                values: new object[] { "30d6d758-4851-41ee-a17e-e56a7e1b391e", new DateTime(2024, 5, 19, 10, 51, 10, 104, DateTimeKind.Local).AddTicks(3468), 0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Barcode", "Created", "Stock" },
                values: new object[] { "16939347-20aa-4c60-a3b8-a2e032d9ba89", new DateTime(2024, 5, 19, 10, 51, 10, 104, DateTimeKind.Local).AddTicks(3523), 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Barcode", "Created" },
                values: new object[] { "e322a3c8-7160-487c-a3b0-26c5579bae47", new DateTime(2024, 5, 17, 20, 44, 40, 282, DateTimeKind.Local).AddTicks(9461) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Barcode", "Created" },
                values: new object[] { "1318fc0c-bcb8-45e0-953f-bbecfd25fb30", new DateTime(2024, 5, 17, 20, 44, 40, 282, DateTimeKind.Local).AddTicks(9515) });
        }
    }
}

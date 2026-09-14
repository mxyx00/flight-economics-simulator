using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddAirportFees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DepartureFee",
                table: "Airports",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LandingFeePer1000Lb",
                table: "Airports",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DepartureFee", "LandingFeePer1000Lb" },
                values: new object[] { 1200.0, 7.5 });

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DepartureFee", "LandingFeePer1000Lb" },
                values: new object[] { 1600.0, 9.5 });

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DepartureFee", "LandingFeePer1000Lb" },
                values: new object[] { 1100.0, 7.25 });

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DepartureFee", "LandingFeePer1000Lb" },
                values: new object[] { 900.0, 5.5 });

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DepartureFee", "LandingFeePer1000Lb" },
                values: new object[] { 850.0, 4.75 });

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DepartureFee", "LandingFeePer1000Lb" },
                values: new object[] { 1400.0, 8.25 });

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DepartureFee", "LandingFeePer1000Lb" },
                values: new object[] { 950.0, 6.0 });

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DepartureFee", "LandingFeePer1000Lb" },
                values: new object[] { 1000.0, 6.25 });

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DepartureFee", "LandingFeePer1000Lb" },
                values: new object[] { 1300.0, 8.0 });

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "DepartureFee", "LandingFeePer1000Lb" },
                values: new object[] { 900.0, 5.25 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartureFee",
                table: "Airports");

            migrationBuilder.DropColumn(
                name: "LandingFeePer1000Lb",
                table: "Airports");
        }
    }
}

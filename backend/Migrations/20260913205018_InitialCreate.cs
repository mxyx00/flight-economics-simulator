using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Airports",
                columns: new[] { "Id", "City", "Code", "Latitude", "Longitude", "Name" },
                values: new object[,]
                {
                    { 1, "Los Angeles", "LAX", 33.942501, -118.40799699999999, "Los Angeles International Airport" },
                    { 2, "New York", "JFK", 40.639446999999997, -73.779317000000006, "John F. Kennedy International Airport" },
                    { 3, "Chicago", "ORD", 41.9786, -87.904799999999994, "Chicago O'Hare International Airport" },
                    { 4, "Dallas", "DFW", 32.896801000000004, -97.038002000000006, "Dallas Fort Worth International Airport" },
                    { 5, "Atlanta", "ATL", 33.636699999999998, -84.428100999999998, "Hartsfield-Jackson Atlanta International Airport" },
                    { 6, "San Francisco", "SFO", 37.619805999999997, -122.374821, "San Francisco International Airport" },
                    { 7, "Seattle", "SEA", 47.449001000000003, -122.308998, "Seattle-Tacoma International Airport" },
                    { 8, "Miami", "MIA", 25.793199999999999, -80.290604000000002, "Miami International Airport" },
                    { 9, "Boston", "BOS", 42.3643, -71.005202999999995, "Boston Logan International Airport" },
                    { 10, "Denver", "DEN", 39.861697999999997, -104.672997, "Denver International Airport" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Airports_Code",
                table: "Airports",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Airports");
        }
    }
}

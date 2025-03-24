using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalkRevise.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDataToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("488d8c58-aa04-4023-b10d-db3d85b54b97"), "Hard" },
                    { new Guid("aba0e9fa-6146-40b4-b813-20a5f9a4872d"), "Easy" },
                    { new Guid("d62a659f-0b5b-4e2a-ab4b-bd636c185a46"), "Medium" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Description", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("00f69924-2bac-4589-a7ea-956a091259fb"), "BOP", "The Bay of Plenty is a region in the North Island of New Zealand. The region takes its name from the large bay at its heart.", "Bay of Plenty", "https://upload.wikimedia.org/wikipedia/commons/thumb/1/1b/Northland_region_locator_map.svg/1200px-Northland_region_locator_map.svg.png" },
                    { new Guid("18608543-2350-4e91-b553-41c62c376dd8"), "WKT", "Waikato is a region in the upper North Island of New Zealand. Waikato is known for its dairy farming and horse breeding.", "Waikato", "https://upload.wikimedia.org/wikipedia/commons/thumb/1/1b/Northland_region_locator_map.svg/1200px-Northland_region_locator_map.svg.png" },
                    { new Guid("3ce23c73-b1f3-48a4-9102-9dc079f29d4e"), "NLD", "Northland is a region in the northernmost part of New Zealand. Northland is the warmest region in New Zealand.", "Northland", "https://upload.wikimedia.org/wikipedia/commons/thumb/1/1b/Northland_region_locator_map.svg/1200px-Northland_region_locator_map.svg.png" },
                    { new Guid("c4a251c6-4eff-4c51-977c-719542149fa7"), "AKL", "Auckland is a metropolitan city in the North Island of New Zealand. The most populous urban area in the country.", "Auckland", "https://upload.wikimedia.org/wikipedia/commons/thumb/1/1b/Northland_region_locator_map.svg/1200px-Northland_region_locator_map.svg.png" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("488d8c58-aa04-4023-b10d-db3d85b54b97"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("aba0e9fa-6146-40b4-b813-20a5f9a4872d"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("d62a659f-0b5b-4e2a-ab4b-bd636c185a46"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("00f69924-2bac-4589-a7ea-956a091259fb"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("18608543-2350-4e91-b553-41c62c376dd8"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("3ce23c73-b1f3-48a4-9102-9dc079f29d4e"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("c4a251c6-4eff-4c51-977c-719542149fa7"));
        }
    }
}

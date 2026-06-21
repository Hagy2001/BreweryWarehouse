using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BreweryWarehouse.Web.Migrations
{
    /// <inheritdoc />
    public partial class RestrictStockEntryForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockEntries_Container_ContainerId",
                table: "StockEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_StockEntries_WarehouseLocations_LocationId",
                table: "StockEntries");

            migrationBuilder.AddForeignKey(
                name: "FK_StockEntries_Container_ContainerId",
                table: "StockEntries",
                column: "ContainerId",
                principalTable: "Container",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockEntries_WarehouseLocations_LocationId",
                table: "StockEntries",
                column: "LocationId",
                principalTable: "WarehouseLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockEntries_Container_ContainerId",
                table: "StockEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_StockEntries_WarehouseLocations_LocationId",
                table: "StockEntries");

            migrationBuilder.AddForeignKey(
                name: "FK_StockEntries_Container_ContainerId",
                table: "StockEntries",
                column: "ContainerId",
                principalTable: "Container",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockEntries_WarehouseLocations_LocationId",
                table: "StockEntries",
                column: "LocationId",
                principalTable: "WarehouseLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

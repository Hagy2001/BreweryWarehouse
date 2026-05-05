using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BreweryWarehouse.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BeerStyles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlcoholPercentage = table.Column<double>(type: "float", nullable: false),
                    IBU = table.Column<int>(type: "int", nullable: false),
                    ColorEBC = table.Column<double>(type: "float", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeerStyles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateHired = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aisle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Shelf = table.Column<int>(type: "int", nullable: false),
                    MaxCapacity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Container",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SLCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BestBefore = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Size = table.Column<int>(type: "int", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackagingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BeerStyleId = table.Column<int>(type: "int", nullable: true),
                    Material = table.Column<int>(type: "int", nullable: true),
                    HeadType = table.Column<int>(type: "int", nullable: true),
                    VolumeInLitres = table.Column<int>(type: "int", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastInspection = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Keg_BeerStyleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Container", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Container_BeerStyles_BeerStyleId",
                        column: x => x.BeerStyleId,
                        principalTable: "BeerStyles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Container_BeerStyles_Keg_BeerStyleId",
                        column: x => x.Keg_BeerStyleId,
                        principalTable: "BeerStyles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContainerId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DateReceived = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CanId = table.Column<int>(type: "int", nullable: true),
                    KegId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockEntries_Container_CanId",
                        column: x => x.CanId,
                        principalTable: "Container",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockEntries_Container_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "Container",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockEntries_Container_KegId",
                        column: x => x.KegId,
                        principalTable: "Container",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockEntries_WarehouseLocations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "WarehouseLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Container_BeerStyleId",
                table: "Container",
                column: "BeerStyleId");

            migrationBuilder.CreateIndex(
                name: "IX_Container_Keg_BeerStyleId",
                table: "Container",
                column: "Keg_BeerStyleId");

            migrationBuilder.CreateIndex(
                name: "IX_StockEntries_CanId",
                table: "StockEntries",
                column: "CanId");

            migrationBuilder.CreateIndex(
                name: "IX_StockEntries_ContainerId",
                table: "StockEntries",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_StockEntries_KegId",
                table: "StockEntries",
                column: "KegId");

            migrationBuilder.CreateIndex(
                name: "IX_StockEntries_LocationId",
                table: "StockEntries",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "StockEntries");

            migrationBuilder.DropTable(
                name: "Container");

            migrationBuilder.DropTable(
                name: "WarehouseLocations");

            migrationBuilder.DropTable(
                name: "BeerStyles");
        }
    }
}

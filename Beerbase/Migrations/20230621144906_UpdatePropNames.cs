using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beerbase.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePropNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BarBeer_Bars_BarsBarId",
                table: "BarBeer");

            migrationBuilder.DropForeignKey(
                name: "FK_Beers_Breweries_BreweryId",
                table: "Beers");

            migrationBuilder.RenameColumn(
                name: "BarsBarId",
                table: "BarBeer",
                newName: "BarsServedAtBarId");

            migrationBuilder.AlterColumn<int>(
                name: "BreweryId",
                table: "Beers",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_BarBeer_Bars_BarsServedAtBarId",
                table: "BarBeer",
                column: "BarsServedAtBarId",
                principalTable: "Bars",
                principalColumn: "BarId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Beers_Breweries_BreweryId",
                table: "Beers",
                column: "BreweryId",
                principalTable: "Breweries",
                principalColumn: "BreweryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BarBeer_Bars_BarsServedAtBarId",
                table: "BarBeer");

            migrationBuilder.DropForeignKey(
                name: "FK_Beers_Breweries_BreweryId",
                table: "Beers");

            migrationBuilder.RenameColumn(
                name: "BarsServedAtBarId",
                table: "BarBeer",
                newName: "BarsBarId");

            migrationBuilder.AlterColumn<int>(
                name: "BreweryId",
                table: "Beers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BarBeer_Bars_BarsBarId",
                table: "BarBeer",
                column: "BarsBarId",
                principalTable: "Bars",
                principalColumn: "BarId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Beers_Breweries_BreweryId",
                table: "Beers",
                column: "BreweryId",
                principalTable: "Breweries",
                principalColumn: "BreweryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

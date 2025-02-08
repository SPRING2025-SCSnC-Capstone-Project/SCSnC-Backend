using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PatchRelationshipInOrderFlow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IncludeToppings_ToppingId",
                table: "IncludeToppings");

            migrationBuilder.CreateIndex(
                name: "IX_IncludeToppings_ToppingId",
                table: "IncludeToppings",
                column: "ToppingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IncludeToppings_ToppingId",
                table: "IncludeToppings");

            migrationBuilder.CreateIndex(
                name: "IX_IncludeToppings_ToppingId",
                table: "IncludeToppings",
                column: "ToppingId",
                unique: true);
        }
    }
}

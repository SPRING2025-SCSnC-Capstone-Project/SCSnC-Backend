using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PatchingIncludeTopping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IncludeToppingOrderDetail");

            migrationBuilder.CreateIndex(
                name: "IX_IncludeToppings_OrderDetailId",
                table: "IncludeToppings",
                column: "OrderDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_IncludeToppings_OrderDetails_OrderDetailId",
                table: "IncludeToppings",
                column: "OrderDetailId",
                principalTable: "OrderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncludeToppings_OrderDetails_OrderDetailId",
                table: "IncludeToppings");

            migrationBuilder.DropIndex(
                name: "IX_IncludeToppings_OrderDetailId",
                table: "IncludeToppings");

            migrationBuilder.CreateTable(
                name: "IncludeToppingOrderDetail",
                columns: table => new
                {
                    IncludeToppingsId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderDetailsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncludeToppingOrderDetail", x => new { x.IncludeToppingsId, x.OrderDetailsId });
                    table.ForeignKey(
                        name: "FK_IncludeToppingOrderDetail_IncludeToppings_IncludeToppingsId",
                        column: x => x.IncludeToppingsId,
                        principalTable: "IncludeToppings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncludeToppingOrderDetail_OrderDetails_OrderDetailsId",
                        column: x => x.OrderDetailsId,
                        principalTable: "OrderDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncludeToppingOrderDetail_OrderDetailsId",
                table: "IncludeToppingOrderDetail",
                column: "OrderDetailsId");
        }
    }
}

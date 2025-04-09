using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddToppingPriceAtDifferentBranches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Toppings");

            migrationBuilder.CreateTable(
                name: "ToppingPricesAtBranches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ToppingId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToppingPrice = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToppingPricesAtBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToppingPricesAtBranches_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToppingPricesAtBranches_Toppings_ToppingId",
                        column: x => x.ToppingId,
                        principalTable: "Toppings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToppingPricesAtBranches_BranchId",
                table: "ToppingPricesAtBranches",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ToppingPricesAtBranches_ToppingId",
                table: "ToppingPricesAtBranches",
                column: "ToppingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToppingPricesAtBranches");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Toppings",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

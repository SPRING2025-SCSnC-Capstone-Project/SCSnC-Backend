using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSlotFromReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Slots_SlotId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_SlotId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "SlotId",
                table: "Reservations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SlotId",
                table: "Reservations",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_SlotId",
                table: "Reservations",
                column: "SlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Slots_SlotId",
                table: "Reservations",
                column: "SlotId",
                principalTable: "Slots",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Event_To_Reservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Events_ReservationId",
                table: "Events");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ReservationId",
                table: "Events",
                column: "ReservationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Events_ReservationId",
                table: "Events");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ReservationId",
                table: "Events",
                column: "ReservationId");
        }
    }
}

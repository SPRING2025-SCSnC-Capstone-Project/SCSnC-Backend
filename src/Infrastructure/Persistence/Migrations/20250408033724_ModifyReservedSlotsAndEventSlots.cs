using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModifyReservedSlotsAndEventSlots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReserveDate",
                table: "ReservedSlots");

            migrationBuilder.AddColumn<LocalDate>(
                name: "ReserveDate",
                table: "Reservations",
                type: "date",
                nullable: false,
                defaultValue: new NodaTime.LocalDate(1, 1, 1));

            migrationBuilder.AddColumn<LocalDate>(
                name: "EventDate",
                table: "Events",
                type: "date",
                nullable: false,
                defaultValue: new NodaTime.LocalDate(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReserveDate",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "EventDate",
                table: "Events");

            migrationBuilder.AddColumn<LocalDate>(
                name: "ReserveDate",
                table: "ReservedSlots",
                type: "date",
                nullable: false,
                defaultValue: new NodaTime.LocalDate(1, 1, 1));
        }
    }
}

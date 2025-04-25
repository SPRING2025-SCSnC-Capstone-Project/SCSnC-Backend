using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_TimeStart_TimeEnd_To_Reservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<LocalDateTime>(
                name: "CreatedAt",
                table: "Reservations",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0));

            migrationBuilder.AddColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Reservations",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0));

            migrationBuilder.AddColumn<LocalTime>(
                name: "TimeEnd",
                table: "Reservations",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<LocalTime>(
                name: "TimeStart",
                table: "Reservations",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "TimeEnd",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "TimeStart",
                table: "Reservations");
        }
    }
}

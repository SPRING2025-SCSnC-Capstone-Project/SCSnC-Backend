using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveReserveDateFromReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReserveDate",
                table: "Reservations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<LocalDate>(
                name: "ReserveDate",
                table: "Reservations",
                type: "date",
                nullable: false,
                defaultValue: new NodaTime.LocalDate(1, 1, 1));
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddReservedSlotsAndEventSlots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ReservationDate",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "EventEndTime",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventStartTime",
                table: "Events");

            migrationBuilder.CreateTable(
                name: "EventSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SlotId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventSlots_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSlots_Slots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "Slots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservedSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SlotId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReservationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReserveDate = table.Column<LocalDate>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservedSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservedSlots_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservedSlots_Slots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "Slots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventSlots_EventId",
                table: "EventSlots",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSlots_SlotId",
                table: "EventSlots",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedSlots_ReservationId",
                table: "ReservedSlots",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedSlots_SlotId",
                table: "ReservedSlots",
                column: "SlotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventSlots");

            migrationBuilder.DropTable(
                name: "ReservedSlots");

            migrationBuilder.AddColumn<LocalTime>(
                name: "EndTime",
                table: "Reservations",
                type: "time",
                nullable: false,
                defaultValue: new NodaTime.LocalTime(0, 0));

            migrationBuilder.AddColumn<LocalDate>(
                name: "ReservationDate",
                table: "Reservations",
                type: "date",
                nullable: false,
                defaultValue: new NodaTime.LocalDate(1, 1, 1));

            migrationBuilder.AddColumn<LocalTime>(
                name: "StartTime",
                table: "Reservations",
                type: "time",
                nullable: false,
                defaultValue: new NodaTime.LocalTime(0, 0));

            migrationBuilder.AddColumn<LocalTime>(
                name: "EventEndTime",
                table: "Events",
                type: "time",
                nullable: false,
                defaultValue: new NodaTime.LocalTime(0, 0));

            migrationBuilder.AddColumn<LocalTime>(
                name: "EventStartTime",
                table: "Events",
                type: "time",
                nullable: false,
                defaultValue: new NodaTime.LocalTime(0, 0));
        }
    }
}

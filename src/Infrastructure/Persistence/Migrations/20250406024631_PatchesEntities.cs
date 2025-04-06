using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PatchesEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_UserId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Workspaces_WorkspaceId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Slots_SlotId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Events_UserId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventEndDate",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventStartDate",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "WorkspaceId",
                table: "Events",
                newName: "ReservationId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_WorkspaceId",
                table: "Events",
                newName: "IX_Events_ReservationId");

            migrationBuilder.AlterColumn<Guid>(
                name: "SlotId",
                table: "Reservations",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<LocalDate>(
                name: "ReservationDate",
                table: "Reservations",
                type: "date",
                nullable: false,
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<LocalTime>(
                name: "EndTime",
                table: "Reservations",
                type: "time",
                nullable: false,
                defaultValue: new NodaTime.LocalTime(0, 0));

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

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Reservations_ReservationId",
                table: "Events",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Slots_SlotId",
                table: "Reservations",
                column: "SlotId",
                principalTable: "Slots",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Reservations_ReservationId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Slots_SlotId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "EndTime",
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

            migrationBuilder.RenameColumn(
                name: "ReservationId",
                table: "Events",
                newName: "WorkspaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_ReservationId",
                table: "Events",
                newName: "IX_Events_WorkspaceId");

            migrationBuilder.AlterColumn<Guid>(
                name: "SlotId",
                table: "Reservations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "ReservationDate",
                table: "Reservations",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(LocalDate),
                oldType: "date");

            migrationBuilder.AddColumn<LocalDateTime>(
                name: "EventEndDate",
                table: "Events",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0));

            migrationBuilder.AddColumn<LocalDateTime>(
                name: "EventStartDate",
                table: "Events",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Events",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId",
                table: "Events",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_UserId",
                table: "Events",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Workspaces_WorkspaceId",
                table: "Events",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Slots_SlotId",
                table: "Reservations",
                column: "SlotId",
                principalTable: "Slots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

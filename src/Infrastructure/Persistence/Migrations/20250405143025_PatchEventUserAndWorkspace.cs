using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PatchEventUserAndWorkspace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImgCover",
                table: "Events",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "EventDate",
                table: "Events",
                newName: "EventStartDate");

            migrationBuilder.AddColumn<string>(
                name: "CoverImageLink",
                table: "Events",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<LocalDateTime>(
                name: "EventEndDate",
                table: "Events",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Events",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Events",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceId",
                table: "Events",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId",
                table: "Events",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_WorkspaceId",
                table: "Events",
                column: "WorkspaceId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_UserId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Workspaces_WorkspaceId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_UserId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_WorkspaceId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CoverImageLink",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventEndDate",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Events",
                newName: "ImgCover");

            migrationBuilder.RenameColumn(
                name: "EventStartDate",
                table: "Events",
                newName: "EventDate");
        }
    }
}

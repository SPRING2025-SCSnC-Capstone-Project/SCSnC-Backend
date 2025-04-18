using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PatchWorkspaceMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "WorkspaceMedias");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WorkspaceMedias");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "WorkspaceMedias");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<LocalDateTime>(
                name: "CreatedAt",
                table: "WorkspaceMedias",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WorkspaceMedias",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "WorkspaceMedias",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0));
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Connect_Workspace_Media_To_WorkspaceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkspaceMedias_Workspaces_WorkspaceId",
                table: "WorkspaceMedias");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "WorkspaceId",
                table: "WorkspaceMedias",
                newName: "WorkspaceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkspaceMedias_WorkspaceId",
                table: "WorkspaceMedias",
                newName: "IX_WorkspaceMedias_WorkspaceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkspaceMedias_WorkspaceTypes_WorkspaceTypeId",
                table: "WorkspaceMedias",
                column: "WorkspaceTypeId",
                principalTable: "WorkspaceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkspaceMedias_WorkspaceTypes_WorkspaceTypeId",
                table: "WorkspaceMedias");

            migrationBuilder.RenameColumn(
                name: "WorkspaceTypeId",
                table: "WorkspaceMedias",
                newName: "WorkspaceId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkspaceMedias_WorkspaceTypeId",
                table: "WorkspaceMedias",
                newName: "IX_WorkspaceMedias_WorkspaceId");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Events",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkspaceMedias_Workspaces_WorkspaceId",
                table: "WorkspaceMedias",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

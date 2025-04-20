using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_ApplicationDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workspaces_WorkspaceTypeAtBranch_WorkspaceTypeAtBranchId",
                table: "Workspaces");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkspaceTypeAtBranch_Branches_BranchId",
                table: "WorkspaceTypeAtBranch");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkspaceTypeAtBranch_WorkspaceTypes_WorkspaceTypeId",
                table: "WorkspaceTypeAtBranch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkspaceTypeAtBranch",
                table: "WorkspaceTypeAtBranch");

            migrationBuilder.RenameTable(
                name: "WorkspaceTypeAtBranch",
                newName: "WorkspaceTypeAtBranches");

            migrationBuilder.RenameIndex(
                name: "IX_WorkspaceTypeAtBranch_WorkspaceTypeId",
                table: "WorkspaceTypeAtBranches",
                newName: "IX_WorkspaceTypeAtBranches_WorkspaceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkspaceTypeAtBranch_BranchId",
                table: "WorkspaceTypeAtBranches",
                newName: "IX_WorkspaceTypeAtBranches_BranchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkspaceTypeAtBranches",
                table: "WorkspaceTypeAtBranches",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workspaces_WorkspaceTypeAtBranches_WorkspaceTypeAtBranchId",
                table: "Workspaces",
                column: "WorkspaceTypeAtBranchId",
                principalTable: "WorkspaceTypeAtBranches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkspaceTypeAtBranches_Branches_BranchId",
                table: "WorkspaceTypeAtBranches",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkspaceTypeAtBranches_WorkspaceTypes_WorkspaceTypeId",
                table: "WorkspaceTypeAtBranches",
                column: "WorkspaceTypeId",
                principalTable: "WorkspaceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workspaces_WorkspaceTypeAtBranches_WorkspaceTypeAtBranchId",
                table: "Workspaces");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkspaceTypeAtBranches_Branches_BranchId",
                table: "WorkspaceTypeAtBranches");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkspaceTypeAtBranches_WorkspaceTypes_WorkspaceTypeId",
                table: "WorkspaceTypeAtBranches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkspaceTypeAtBranches",
                table: "WorkspaceTypeAtBranches");

            migrationBuilder.RenameTable(
                name: "WorkspaceTypeAtBranches",
                newName: "WorkspaceTypeAtBranch");

            migrationBuilder.RenameIndex(
                name: "IX_WorkspaceTypeAtBranches_WorkspaceTypeId",
                table: "WorkspaceTypeAtBranch",
                newName: "IX_WorkspaceTypeAtBranch_WorkspaceTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkspaceTypeAtBranches_BranchId",
                table: "WorkspaceTypeAtBranch",
                newName: "IX_WorkspaceTypeAtBranch_BranchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkspaceTypeAtBranch",
                table: "WorkspaceTypeAtBranch",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workspaces_WorkspaceTypeAtBranch_WorkspaceTypeAtBranchId",
                table: "Workspaces",
                column: "WorkspaceTypeAtBranchId",
                principalTable: "WorkspaceTypeAtBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkspaceTypeAtBranch_Branches_BranchId",
                table: "WorkspaceTypeAtBranch",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkspaceTypeAtBranch_WorkspaceTypes_WorkspaceTypeId",
                table: "WorkspaceTypeAtBranch",
                column: "WorkspaceTypeId",
                principalTable: "WorkspaceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_WorkspaceTypeAtBranch_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workspaces_WorkspaceTypes_WorkspaceTypeId",
                table: "Workspaces");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkspaceTypeId",
                table: "Workspaces",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceTypeAtBranchId",
                table: "Workspaces",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "WorkspaceTypeAtBranch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkspaceTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    BranchId = table.Column<Guid>(type: "uuid", nullable: false),
                    PriceAdjust = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false),
                    LastUpdatedAt = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkspaceTypeAtBranch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkspaceTypeAtBranch_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkspaceTypeAtBranch_WorkspaceTypes_WorkspaceTypeId",
                        column: x => x.WorkspaceTypeId,
                        principalTable: "WorkspaceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workspaces_WorkspaceTypeAtBranchId",
                table: "Workspaces",
                column: "WorkspaceTypeAtBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceTypeAtBranch_BranchId",
                table: "WorkspaceTypeAtBranch",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceTypeAtBranch_WorkspaceTypeId",
                table: "WorkspaceTypeAtBranch",
                column: "WorkspaceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workspaces_WorkspaceTypeAtBranch_WorkspaceTypeAtBranchId",
                table: "Workspaces",
                column: "WorkspaceTypeAtBranchId",
                principalTable: "WorkspaceTypeAtBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workspaces_WorkspaceTypes_WorkspaceTypeId",
                table: "Workspaces",
                column: "WorkspaceTypeId",
                principalTable: "WorkspaceTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workspaces_WorkspaceTypeAtBranch_WorkspaceTypeAtBranchId",
                table: "Workspaces");

            migrationBuilder.DropForeignKey(
                name: "FK_Workspaces_WorkspaceTypes_WorkspaceTypeId",
                table: "Workspaces");

            migrationBuilder.DropTable(
                name: "WorkspaceTypeAtBranch");

            migrationBuilder.DropIndex(
                name: "IX_Workspaces_WorkspaceTypeAtBranchId",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "WorkspaceTypeAtBranchId",
                table: "Workspaces");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkspaceTypeId",
                table: "Workspaces",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Workspaces_WorkspaceTypes_WorkspaceTypeId",
                table: "Workspaces",
                column: "WorkspaceTypeId",
                principalTable: "WorkspaceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

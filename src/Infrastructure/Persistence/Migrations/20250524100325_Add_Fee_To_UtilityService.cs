using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Fee_To_UtilityService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceFee",
                table: "WorkspaceUtilityServices");

            migrationBuilder.AddColumn<double>(
                name: "Fee",
                table: "UtilityServices",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fee",
                table: "UtilityServices");

            migrationBuilder.AddColumn<double>(
                name: "ServiceFee",
                table: "WorkspaceUtilityServices",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

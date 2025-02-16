using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PatchingNullableAndTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ItemWithSize",
                newName: "ItemWithSizes");
            
            migrationBuilder.RenameTable(
                name: "IncludeTopping",
                newName: "IncludeToppings");
            
            migrationBuilder.AlterColumn<Guid>(
                name: "VoucherId",
                table: "Orders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "ItemWithSizes",
                newName: "ItemWithSize");
            
            migrationBuilder.RenameTable(
                name: "IncludeToppings",
                newName: "IncludeTopping");
            
            migrationBuilder.AlterColumn<Guid>(
                name: "VoucherId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
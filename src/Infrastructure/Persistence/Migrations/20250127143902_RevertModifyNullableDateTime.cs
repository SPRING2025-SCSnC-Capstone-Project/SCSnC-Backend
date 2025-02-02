using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RevertModifyNullableDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Vouchers",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0),
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0),
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Toppings",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0),
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Tables",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0),
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Sizes",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0),
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Orders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0),
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Items",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0),
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "ItemCategories",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0),
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Feedbacks",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0),
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Events",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new NodaTime.LocalDateTime(1, 1, 1, 0, 0),
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Vouchers",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Users",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Toppings",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Tables",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Sizes",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Orders",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Items",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "ItemCategories",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Feedbacks",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<LocalDateTime>(
                name: "LastUpdatedAt",
                table: "Events",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(LocalDateTime),
                oldType: "timestamp without time zone");
        }
    }
}

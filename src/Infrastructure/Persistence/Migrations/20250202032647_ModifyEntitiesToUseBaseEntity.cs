using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModifyEntitiesToUseBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncludeToppingOrderDetail_IncludeTopping_IncludeToppingsInc~",
                table: "IncludeToppingOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_IncludeToppingOrderDetail_OrderDetails_OrderDetailsOrderDet~",
                table: "IncludeToppingOrderDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "WorkspaceTypeId",
                table: "WorkspaceTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "WorkspaceId",
                table: "Workspaces",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "VoucherId",
                table: "Vouchers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserVoucherId",
                table: "UserVouchers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "Transaction",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ToppingId",
                table: "Toppings",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "TableId",
                table: "Tables",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SlotId",
                table: "Slots",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SizeId",
                table: "Sizes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Roles",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "Payment",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Orders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "OrderDetailId",
                table: "OrderDetails",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "JoinEventId",
                table: "JoinEvents",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ItemWithSizeId",
                table: "ItemWithSize",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Items",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ItemCategoryId",
                table: "ItemCategories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "OrderDetailsOrderDetailId",
                table: "IncludeToppingOrderDetail",
                newName: "OrderDetailsId");

            migrationBuilder.RenameColumn(
                name: "IncludeToppingsIncludeToppingId",
                table: "IncludeToppingOrderDetail",
                newName: "IncludeToppingsId");

            migrationBuilder.RenameIndex(
                name: "IX_IncludeToppingOrderDetail_OrderDetailsOrderDetailId",
                table: "IncludeToppingOrderDetail",
                newName: "IX_IncludeToppingOrderDetail_OrderDetailsId");

            migrationBuilder.RenameColumn(
                name: "IncludeToppingId",
                table: "IncludeTopping",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FeedbackId",
                table: "Feedbacks",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Events",
                newName: "Id");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Reservations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IncludeToppingOrderDetail_IncludeTopping_IncludeToppingsId",
                table: "IncludeToppingOrderDetail",
                column: "IncludeToppingsId",
                principalTable: "IncludeTopping",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IncludeToppingOrderDetail_OrderDetails_OrderDetailsId",
                table: "IncludeToppingOrderDetail",
                column: "OrderDetailsId",
                principalTable: "OrderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IncludeToppingOrderDetail_IncludeTopping_IncludeToppingsId",
                table: "IncludeToppingOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_IncludeToppingOrderDetail_OrderDetails_OrderDetailsId",
                table: "IncludeToppingOrderDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "WorkspaceTypes",
                newName: "WorkspaceTypeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Workspaces",
                newName: "WorkspaceId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Vouchers",
                newName: "VoucherId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserVouchers",
                newName: "UserVoucherId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Transaction",
                newName: "TransactionId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Toppings",
                newName: "ToppingId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tables",
                newName: "TableId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Slots",
                newName: "SlotId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Sizes",
                newName: "SizeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Roles",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Payment",
                newName: "PaymentId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Orders",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "OrderDetails",
                newName: "OrderDetailId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "JoinEvents",
                newName: "JoinEventId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ItemWithSize",
                newName: "ItemWithSizeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Items",
                newName: "ItemId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ItemCategories",
                newName: "ItemCategoryId");

            migrationBuilder.RenameColumn(
                name: "OrderDetailsId",
                table: "IncludeToppingOrderDetail",
                newName: "OrderDetailsOrderDetailId");

            migrationBuilder.RenameColumn(
                name: "IncludeToppingsId",
                table: "IncludeToppingOrderDetail",
                newName: "IncludeToppingsIncludeToppingId");

            migrationBuilder.RenameIndex(
                name: "IX_IncludeToppingOrderDetail_OrderDetailsId",
                table: "IncludeToppingOrderDetail",
                newName: "IX_IncludeToppingOrderDetail_OrderDetailsOrderDetailId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "IncludeTopping",
                newName: "IncludeToppingId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Feedbacks",
                newName: "FeedbackId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Events",
                newName: "EventId");

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "Reservations",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_IncludeToppingOrderDetail_IncludeTopping_IncludeToppingsInc~",
                table: "IncludeToppingOrderDetail",
                column: "IncludeToppingsIncludeToppingId",
                principalTable: "IncludeTopping",
                principalColumn: "IncludeToppingId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IncludeToppingOrderDetail_OrderDetails_OrderDetailsOrderDet~",
                table: "IncludeToppingOrderDetail",
                column: "OrderDetailsOrderDetailId",
                principalTable: "OrderDetails",
                principalColumn: "OrderDetailId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetapi.Migrations
{
    /// <inheritdoc />
    public partial class FixedDetalleVentaProductIdLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleVentas_Products_ProductId1",
                table: "DetalleVentas");

            migrationBuilder.DropIndex(
                name: "IX_DetalleVentas_ProductId1",
                table: "DetalleVentas");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "DetalleVentas");

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "DetalleVentas",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleVentas_ProductId",
                table: "DetalleVentas",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleVentas_Products_ProductId",
                table: "DetalleVentas",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalleVentas_Products_ProductId",
                table: "DetalleVentas");

            migrationBuilder.DropIndex(
                name: "IX_DetalleVentas_ProductId",
                table: "DetalleVentas");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "DetalleVentas",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "ProductId1",
                table: "DetalleVentas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_DetalleVentas_ProductId1",
                table: "DetalleVentas",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleVentas_Products_ProductId1",
                table: "DetalleVentas",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

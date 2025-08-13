using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MisCanchas.EfSqlRepository.Migrations
{
    /// <inheritdoc />
    public partial class DepositPropertyField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Deposit",
                table: "Fields",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deposit",
                table: "Fields");
        }
    }
}

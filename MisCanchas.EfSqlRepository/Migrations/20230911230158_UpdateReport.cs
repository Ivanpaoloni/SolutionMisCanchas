using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MisCanchas.EfSqlRepository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Booking",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Canceled",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "In",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Out",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Booking",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Canceled",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "In",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Out",
                table: "Reports");
        }
    }
}

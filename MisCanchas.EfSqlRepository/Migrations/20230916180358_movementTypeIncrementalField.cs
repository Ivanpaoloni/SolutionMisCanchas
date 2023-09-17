using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MisCanchas.EfSqlRepository.Migrations
{
    /// <inheritdoc />
    public partial class movementTypeIncrementalField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Incremental",
                table: "MovementTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Incremental",
                table: "MovementTypes");
        }
    }
}

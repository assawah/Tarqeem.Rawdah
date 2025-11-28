using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tarqeem.CA.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class deleteUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "usr",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "usr",
                table: "Users");
        }
    }
}

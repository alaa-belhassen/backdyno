using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Platform.ReferentialData.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class activationEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "activated",
                table: "Employee",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "activated",
                table: "Employee");
        }
    }
}

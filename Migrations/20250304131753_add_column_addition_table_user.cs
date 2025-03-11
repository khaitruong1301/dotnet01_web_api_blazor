using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi_blazor.Migrations
{
    /// <inheritdoc />
    public partial class add_column_addition_table_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Additional",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Additional",
                table: "Users");
        }
    }
}

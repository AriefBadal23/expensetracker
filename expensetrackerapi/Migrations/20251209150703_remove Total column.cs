using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace expensetrackerapi.Migrations
{
    /// <inheritdoc />
    public partial class removeTotalcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "Buckets");

            migrationBuilder.RenameColumn(
                name: "icon",
                table: "Buckets",
                newName: "Icon");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Icon",
                table: "Buckets",
                newName: "icon");

            migrationBuilder.AddColumn<int>(
                name: "Total",
                table: "Buckets",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}

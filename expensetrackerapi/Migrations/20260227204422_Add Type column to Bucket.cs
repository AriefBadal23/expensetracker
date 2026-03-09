using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace expensetrackerapi.Migrations
{
    /// <inheritdoc />
    public partial class AddTypecolumntoBucket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIncome",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Buckets",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Buckets");

            migrationBuilder.AddColumn<bool>(
                name: "IsIncome",
                table: "Transactions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}

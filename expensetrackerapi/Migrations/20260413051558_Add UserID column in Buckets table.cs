using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace expensetrackerapi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIDcolumninBucketstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Buckets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Buckets");
        }
    }
}

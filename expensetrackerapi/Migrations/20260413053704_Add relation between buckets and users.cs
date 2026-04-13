using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace expensetrackerapi.Migrations
{
    /// <inheritdoc />
    public partial class Addrelationbetweenbucketsandusers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Buckets_ApplicationUserId",
                table: "Buckets",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buckets_AspNetUsers_ApplicationUserId",
                table: "Buckets",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buckets_AspNetUsers_ApplicationUserId",
                table: "Buckets");

            migrationBuilder.DropIndex(
                name: "IX_Buckets_ApplicationUserId",
                table: "Buckets");
        }
    }
}

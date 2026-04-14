using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace expensetrackerapi.Migrations
{
    /// <inheritdoc />
    public partial class ManytoManyrelationshipbucketsandusers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buckets_AspNetUsers_ApplicationUserId",
                table: "Buckets");

            migrationBuilder.DropIndex(
                name: "IX_Buckets_ApplicationUserId",
                table: "Buckets");

            migrationBuilder.CreateTable(
                name: "UserBuckets",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(type: "text", nullable: false),
                    BucketId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBuckets", x => new { x.ApplicationUserId, x.BucketId });
                    table.ForeignKey(
                        name: "FK_UserBuckets_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBuckets_Buckets_BucketId",
                        column: x => x.BucketId,
                        principalTable: "Buckets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBuckets_BucketId",
                table: "UserBuckets",
                column: "BucketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBuckets");

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
    }
}

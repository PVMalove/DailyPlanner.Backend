using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DailyPlanner.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserToken_UserId",
                table: "UserToken");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpiryTime",
                table: "UserToken",
                newName: "RefreshTokenExpireTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_UserId",
                table: "UserToken",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserToken_UserId",
                table: "UserToken");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpireTime",
                table: "UserToken",
                newName: "RefreshTokenExpiryTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_UserId",
                table: "UserToken",
                column: "UserId");
        }
    }
}

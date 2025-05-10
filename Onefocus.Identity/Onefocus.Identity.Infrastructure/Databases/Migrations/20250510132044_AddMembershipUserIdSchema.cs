using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onefocus.Identity.Infrastructure.Databases.Migrations
{
    /// <inheritdoc />
    public partial class AddMembershipUserIdSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MembershipUserId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MembershipUserId",
                table: "AspNetUsers",
                column: "MembershipUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MembershipUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MembershipUserId",
                table: "AspNetUsers");
        }
    }
}

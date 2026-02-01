using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onefocus.Wallet.Infrastructure.Databases.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Option");

            migrationBuilder.DropTable(
                name: "OutboxEvent");

            migrationBuilder.CreateTable(
                name: "SearchIndexQueue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IndexName = table.Column<string>(type: "text", nullable: false),
                    DocumentId = table.Column<string>(type: "text", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    VectorSearchTerms = table.Column<string>(type: "jsonb", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchIndexQueue", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchIndexQueue");

            migrationBuilder.CreateTable(
                name: "Option",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    OptionType = table.Column<int>(type: "integer", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Option", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Option_User_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutboxEvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DocumentId = table.Column<string>(type: "text", nullable: false),
                    IndexName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    VectorSearchTerms = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxEvent", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Option_OwnerUserId",
                table: "Option",
                column: "OwnerUserId");
        }
    }
}

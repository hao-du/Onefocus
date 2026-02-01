using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onefocus.Search.Infrastructure.Databases.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}

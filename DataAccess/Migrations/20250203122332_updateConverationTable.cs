using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateConverationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdateRelative",
                table: "Conversations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastUpdateRelative",
                table: "Conversations",
                type: "nvarchar(max)",
                nullable: false,
                computedColumnSql: "CASE WHEN DATEDIFF(MINUTE, LastUpdate, GETUTCDATE()) < 1 THEN 'Just now' WHEN DATEDIFF(MINUTE, LastUpdate, GETUTCDATE()) < 60 THEN CAST(DATEDIFF(MINUTE, LastUpdate, GETUTCDATE()) AS NVARCHAR(50)) + ' minutes ago' WHEN DATEDIFF(HOUR, LastUpdate, GETUTCDATE()) < 24 THEN CAST(DATEDIFF(HOUR, LastUpdate, GETUTCDATE()) AS NVARCHAR(50)) + ' hours ago' WHEN DATEDIFF(DAY, LastUpdate, GETUTCDATE()) < 30 THEN CAST(DATEDIFF(DAY, LastUpdate, GETUTCDATE()) AS NVARCHAR(50)) + ' days ago' WHEN DATEDIFF(MONTH, LastUpdate, GETUTCDATE()) < 12 THEN CAST(DATEDIFF(MONTH, LastUpdate, GETUTCDATE()) AS NVARCHAR(50)) + ' months ago' ELSE CAST(DATEDIFF(YEAR, LastUpdate, GETUTCDATE()) AS NVARCHAR(50)) + ' years ago' END");
        }
    }
}

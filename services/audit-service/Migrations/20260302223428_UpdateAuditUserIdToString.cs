using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIK.AuditService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditUserIdToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AuditLogs",
                type: "varchar(24)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "AuditLogs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(24)",
                oldNullable: true);
        }
    }
}

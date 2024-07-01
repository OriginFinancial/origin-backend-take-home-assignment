using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAccessManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_eligibility_file_line_eligibility_file_eligibility_file_id",
                table: "eligibility_file_line");

            migrationBuilder.AddColumn<string>(
                name: "message",
                table: "eligibility_file",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "message",
                table: "eligibility_file");

            migrationBuilder.AddForeignKey(
                name: "FK_eligibility_file_line_eligibility_file_eligibility_file_id",
                table: "eligibility_file_line",
                column: "eligibility_file_id",
                principalTable: "eligibility_file",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

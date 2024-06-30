using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace UserAccessManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "eligibility_file",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    employer_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("eligibility_file_pk", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    email = table.Column<string>(type: "varchar(255)", nullable: false),
                    full_name = table.Column<string>(type: "longtext", nullable: false),
                    country = table.Column<string>(type: "longtext", nullable: false),
                    birth_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    employer_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    eligibility_file_id = table.Column<long>(type: "bigint", nullable: false),
                    eligibility_file_line_id = table.Column<long>(type: "bigint", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("employee_pk", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "eligibility_file_line",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    content = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    line_type = table.Column<int>(type: "int", nullable: false),
                    eligibility_file_id = table.Column<long>(type: "bigint", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("eligibility_file_line_pk", x => x.id);
                    table.ForeignKey(
                        name: "FK_eligibility_file_line_eligibility_file_eligibility_file_id",
                        column: x => x.eligibility_file_id,
                        principalTable: "eligibility_file",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "eligibility_file_employer_id_idx",
                table: "eligibility_file",
                column: "employer_id");

            migrationBuilder.CreateIndex(
                name: "eligibility_file_line_eligibility_file_id_idx",
                table: "eligibility_file_line",
                column: "eligibility_file_id");

            migrationBuilder.CreateIndex(
                name: "employee_eligibility_file_id_idx",
                table: "employee",
                column: "eligibility_file_id");

            migrationBuilder.CreateIndex(
                name: "employee_eligibility_file_line_id_idx",
                table: "employee",
                column: "eligibility_file_line_id");

            migrationBuilder.CreateIndex(
                name: "employee_email_idx",
                table: "employee",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "employee_employer_id_idx",
                table: "employee",
                column: "employer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "eligibility_file_line");

            migrationBuilder.DropTable(
                name: "employee");

            migrationBuilder.DropTable(
                name: "eligibility_file");
        }
    }
}

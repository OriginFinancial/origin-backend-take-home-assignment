using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace UserAccessManagement.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Employer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employer", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "employee",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    email = table.Column<string>(type: "longtext", nullable: false),
                    full_name = table.Column<string>(type: "longtext", nullable: false),
                    country = table.Column<string>(type: "longtext", nullable: false),
                    birth_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    employer_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    eligibility_file_line_id = table.Column<long>(type: "bigint", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("employee_pk", x => x.id);
                    table.ForeignKey(
                        name: "FK_employee_Employer_employer_id",
                        column: x => x.employer_id,
                        principalTable: "Employer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_employee_eligibility_file_line_eligibility_file_line_id",
                        column: x => x.eligibility_file_line_id,
                        principalTable: "eligibility_file_line",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_eligibility_file_line_eligibility_file_id",
                table: "eligibility_file_line",
                column: "eligibility_file_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_eligibility_file_line_id",
                table: "employee",
                column: "eligibility_file_line_id");

            migrationBuilder.CreateIndex(
                name: "IX_employee_employer_id",
                table: "employee",
                column: "employer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "employee");

            migrationBuilder.DropTable(
                name: "Employer");

            migrationBuilder.DropTable(
                name: "eligibility_file_line");
        }
    }
}

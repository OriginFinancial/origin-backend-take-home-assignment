﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserAccessManagement.Infrastructure.Data.Context;

#nullable disable

namespace UserAccessManagement.Infrastructure.Data.Migrations
{
    [DbContext(typeof(UserAccessManagementDbContext))]
    [Migration("20240701052124_v3")]
    partial class v3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("UserAccessManagement.Domain.Entities.EligibilityFile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false)
                        .HasColumnName("active");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created_at");

                    b.Property<Guid>("EmployerId")
                        .HasColumnType("char(36)")
                        .HasColumnName("employer_id");

                    b.Property<string>("Message")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("message");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("updated_at");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)")
                        .HasColumnName("url");

                    b.HasKey("Id")
                        .HasName("eligibility_file_pk");

                    b.HasIndex("EmployerId")
                        .HasDatabaseName("eligibility_file_employer_id_idx");

                    b.ToTable("eligibility_file", (string)null);
                });

            modelBuilder.Entity("UserAccessManagement.Domain.Entities.EligibilityFileLine", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false)
                        .HasColumnName("active");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)")
                        .HasColumnName("content");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created_at");

                    b.Property<long>("EligibilityFileId")
                        .HasColumnType("bigint")
                        .HasColumnName("eligibility_file_id");

                    b.Property<int>("LineType")
                        .HasColumnType("int")
                        .HasColumnName("line_type");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("eligibility_file_line_pk");

                    b.HasIndex("EligibilityFileId")
                        .HasDatabaseName("eligibility_file_line_eligibility_file_id_idx");

                    b.ToTable("eligibility_file_line", (string)null);
                });

            modelBuilder.Entity("UserAccessManagement.Domain.Entities.Employee", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false)
                        .HasColumnName("active");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("birth_date");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("country");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("created_at");

                    b.Property<long>("EligibilityFileId")
                        .HasColumnType("bigint")
                        .HasColumnName("eligibility_file_id");

                    b.Property<long>("EligibilityFileLineId")
                        .HasColumnType("bigint")
                        .HasColumnName("eligibility_file_line_id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("email");

                    b.Property<Guid>("EmployerId")
                        .HasColumnType("char(36)")
                        .HasColumnName("employer_id");

                    b.Property<string>("FullName")
                        .HasColumnType("longtext")
                        .HasColumnName("full_name");

                    b.Property<decimal?>("Salary")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("salary");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("employee_pk");

                    b.HasIndex("EligibilityFileId")
                        .HasDatabaseName("employee_eligibility_file_id_idx");

                    b.HasIndex("EligibilityFileLineId")
                        .HasDatabaseName("employee_eligibility_file_line_id_idx");

                    b.HasIndex("Email")
                        .HasDatabaseName("employee_email_idx");

                    b.HasIndex("EmployerId")
                        .HasDatabaseName("employee_employer_id_idx");

                    b.ToTable("employee", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}

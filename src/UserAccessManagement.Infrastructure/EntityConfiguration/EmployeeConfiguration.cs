using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserAccessManagement.Domain.Entities;

namespace UserAccessManagement.Infrastructure.Data.EntityConfiguration;

internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employee");

        builder.HasKey(t => t.Id).HasName("employee_pk");
        builder.Property(t => t.Id).HasColumnName("id").IsRequired();

        builder.Property(t => t.Email).HasColumnName("email").IsRequired();
        builder.Property(t => t.FullName).HasColumnName("full_name").IsRequired();
        builder.Property(t => t.Country).HasColumnName("country").IsRequired();
        builder.Property(t => t.BirthDate).HasColumnName("birth_date");
        builder.Property(t => t.Salary).HasColumnName("salary");
        builder.Property(t => t.EmployerId).HasColumnName("employer_id").IsRequired();
        builder.Property(t => t.EligibilityFileId).HasColumnName("eligibility_file_id").IsRequired();
        builder.Property(t => t.EligibilityFileLineId).HasColumnName("eligibility_file_line_id").IsRequired();

        builder.Property(t => t.Active).HasColumnName("active").IsRequired().HasDefaultValue(false);
        builder.Property(t => t.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(t => t.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(t => t.Email).HasDatabaseName("employee_email_idx");
        builder.HasIndex(t => t.EmployerId).HasDatabaseName("employee_employer_id_idx");
        builder.HasIndex(t => t.EligibilityFileId).HasDatabaseName("employee_eligibility_file_id_idx");
        builder.HasIndex(t => t.EligibilityFileLineId).HasDatabaseName("employee_eligibility_file_line_id_idx");
    }
}
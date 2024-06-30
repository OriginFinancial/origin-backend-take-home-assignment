using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserAccessManagement.Domain.Entities;

namespace UserAccessManagement.Infrastructure.Data.EntityConfiguration;

internal class EligibilityFileConfiguraration : IEntityTypeConfiguration<EligibilityFile>
{
    public void Configure(EntityTypeBuilder<EligibilityFile> builder)
    {
        builder.ToTable("eligibility_file");

        builder.HasKey(t => t.Id).HasName("eligibility_file_pk");
        builder.Property(t => t.Id).HasColumnName("id").IsRequired();

        builder.Property(t => t.Url).HasColumnName("url").HasMaxLength(500).IsRequired();
        builder.Property(t => t.EmployerId).HasColumnName("employer_id").IsRequired();
        builder.Property(t => t.Status).HasColumnName("status").IsRequired();

        builder.Property(t => t.Active).HasColumnName("active").IsRequired().HasDefaultValue(false);
        builder.Property(t => t.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(t => t.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(t => t.EmployerId).HasDatabaseName("eligibility_file_employer_id_idx");
    }
}
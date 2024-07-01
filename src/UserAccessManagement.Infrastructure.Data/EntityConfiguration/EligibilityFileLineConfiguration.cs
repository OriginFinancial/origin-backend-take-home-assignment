using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserAccessManagement.Domain.Entities;

namespace UserAccessManagement.Infrastructure.Data.EntityConfiguration;

internal class EligibilityFileLineConfiguration : IEntityTypeConfiguration<EligibilityFileLine>
{
    public void Configure(EntityTypeBuilder<EligibilityFileLine> builder)
    {
        builder.ToTable("eligibility_file_line");

        builder.HasKey(t => t.Id).HasName("eligibility_file_line_pk");
        builder.Property(t => t.Id).HasColumnName("id").IsRequired();

        builder.Property(t => t.Content).HasColumnName("content").HasMaxLength(1000).IsRequired();
        builder.Property(t => t.LineType).HasColumnName("line_type").IsRequired();
        builder.Property(t => t.EligibilityFileId).HasColumnName("eligibility_file_id").IsRequired();

        builder.Property(t => t.Active).HasColumnName("active").IsRequired().HasDefaultValue(false);
        builder.Property(t => t.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(t => t.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(t => t.EligibilityFileId).HasDatabaseName("eligibility_file_line_eligibility_file_id_idx");
    }
}
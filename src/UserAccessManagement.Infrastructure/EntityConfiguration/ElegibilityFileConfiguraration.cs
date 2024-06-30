using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserAccessManagement.Domain.Entities;

namespace UserAccessManagement.Infrastructure.Data.EntityConfiguration;

internal class ElegibilityFileConfiguraration : IEntityTypeConfiguration<ElegibilityFile>
{
    public void Configure(EntityTypeBuilder<ElegibilityFile> builder)
    {
        builder.ToTable("elegibility_file");

        builder.HasKey(t => t.Id).HasName("elegibility_file_pk");
        builder.Property(t => t.Id).HasColumnName("id").IsRequired();

        builder.Property(t => t.EmployerId).HasColumnName("employer_id").IsRequired();
        builder.Property(t => t.Status).HasColumnName("status").IsRequired();

        builder.Property(t => t.Active).HasColumnName("active").IsRequired().HasDefaultValue(false);
        builder.Property(t => t.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(t => t.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(t => t.EmployerId).HasDatabaseName("elegibility_file_employer_id_idx");
    }
}
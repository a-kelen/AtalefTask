using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AtalefTask.Models.Configurations
{
    public class SmartMatchItemConfiguration : IEntityTypeConfiguration<SmartMatchItem>
    {
        public void Configure(EntityTypeBuilder<SmartMatchItem> builder)
        {
            builder.ToTable("SmartMatchResult");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.UniqueValue).IsRequired();
            builder.Property(x => x.Date).IsRequired();

            builder.HasIndex(x => x.UserId).IsUnique();
            builder.HasIndex(x => x.UniqueValue).IsUnique();
        }
    }
}

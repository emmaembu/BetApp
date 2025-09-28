using BetApp.Infrastructure.Persistence.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BetApp.Infrastructure.Persistence.Configurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<MatchEntity>
    {
        public void Configure(EntityTypeBuilder<MatchEntity> builder)
        {
            builder.ToTable("Matches");
            builder.HasKey(m => m.Id);
            builder.Property(bs => bs.Id).HasDefaultValueSql("NEWSEQUENTIALID()")
               .ValueGeneratedNever();
            builder.Property(m => m.HomeTeam).IsRequired().HasMaxLength(100);
            builder.Property(m => m.AwayTeam).IsRequired().HasMaxLength(100);
            builder.Property(m => m.StartTime).IsRequired();
            builder.HasMany(m => m.Markets)
                   .WithOne(mt => mt.Match)
                   .HasForeignKey(mt => mt.MatchId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

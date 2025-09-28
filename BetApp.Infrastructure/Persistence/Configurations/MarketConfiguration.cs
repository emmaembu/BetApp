using BetApp.Domain.Enums;
using BetApp.Infrastructure.Persistence.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Infrastructure.Persistence.Configurations
{
    public class MarketConfiguration : IEntityTypeConfiguration<MarketEntity>
    {
        public void Configure(EntityTypeBuilder<MarketEntity> builder)
        {
            builder.ToTable("Markets");
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id).HasDefaultValueSql("NEWSEQUENTIALID()")
                .ValueGeneratedNever();

            builder.Property(m => m.MatchId)
                .IsRequired();

            builder.Property(m => m.BetType)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(m => m.Odds)
                .HasColumnType("decimal(18,4)")
                .IsRequired(false);

            builder.Property(m => m.IsTopOffer)
                .IsRequired();

            builder.Property(m => m.IsActive)
                .IsRequired();

            // relationship configured on Match side as well, keep behavior consistent
            builder.HasOne(m => m.Match)
                   .WithMany(ma => ma.Markets)
                   .HasForeignKey(m => m.MatchId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Optional: unique constraint per match+type so a match cannot have duplicate market type
            builder.HasIndex(m => new { m.MatchId, m.BetType })
                   .IsUnique();

        }
    }
}

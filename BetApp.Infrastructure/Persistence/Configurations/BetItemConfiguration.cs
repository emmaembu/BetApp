using BetApp.Infrastructure.Persistence.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BetApp.Infrastructure.Persistence.Configurations
{
    public class BetItemConfiguration : IEntityTypeConfiguration<BetItemEntity>
    {
        public void Configure(EntityTypeBuilder<BetItemEntity> builder)
        {
            builder.ToTable("Markets");
            builder.ToTable("BetItems");

        builder.HasKey(bi => bi.Id);

        builder.Property(bi => bi.Id).HasDefaultValueSql("NEWSEQUENTIALID()")
               .ValueGeneratedNever();

        builder.Property(bi => bi.BetSlipId)
               .IsRequired();

        builder.Property(bi => bi.MarketId)
               .IsRequired();

        builder.Property(bi => bi.OddsAtPlacement)
               .HasColumnType("decimal(18,4)")
               .IsRequired();

        builder.Property(bi => bi.BetType)
               .HasConversion<int>()
               .IsRequired();

        builder.HasOne(bi => bi.BetSlipEntity)
               .WithMany(bs => bs.BetItems)
               .HasForeignKey(bi => bi.BetSlipId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bi => bi.MarketEntity)
               .WithMany()
               .HasForeignKey(bi => bi.MarketId)
               .OnDelete(DeleteBehavior.Restrict);

        // unique constraint to avoid duplicate market on same bet slip
        builder.HasIndex(bi => new { bi.BetSlipId, bi.MarketId
    })
               .IsUnique();
}
} 
    }

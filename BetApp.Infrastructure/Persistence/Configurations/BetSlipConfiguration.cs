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
    public class BetSlipConfiguration : IEntityTypeConfiguration<BetSlipEntity>
    {
        public void Configure(EntityTypeBuilder<BetSlipEntity> builder)
        {
            builder.ToTable("BetSlips");

        builder.HasKey(bs => bs.Id);

        builder.Property(bs => bs.Id).HasDefaultValueSql("NEWSEQUENTIALID()")
               .ValueGeneratedNever();

        builder.Property(bs => bs.WalletId)
               .IsRequired();

        builder.Property(bs => bs.Stake)
               .HasColumnType("decimal(18,4)")
               .IsRequired();

        builder.Property(bs => bs.TotalOdds)
               .HasColumnType("decimal(18,6)") // preserve precision for multiplication
               .IsRequired();

        builder.Property(bs => bs.FeePercent)
               .HasColumnType("decimal(5,4)")
               .IsRequired();

        builder.Property(bs => bs.PotentialPayout)
               .HasColumnType("decimal(18,4)")
               .IsRequired();

        builder.Property(bs => bs.PlacedAt)
               .IsRequired();

        builder.HasOne(bs => bs.Wallet)
               .WithMany()
               .HasForeignKey(bs => bs.WalletId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(bs => bs.BetItems)
               .WithOne(i => i.BetSlip)
               .HasForeignKey(i => i.BetSlipId)
               .OnDelete(DeleteBehavior.Cascade);

        // Optional: index by WalletId + PlacedAt for retrieval
        builder.HasIndex(bs => new { bs.WalletId, bs.PlacedAt
    });
    }
}
    }

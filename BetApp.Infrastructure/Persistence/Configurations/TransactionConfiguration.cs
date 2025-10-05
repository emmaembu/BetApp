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
    public class TransactionConfiguration : IEntityTypeConfiguration<TransactionEntity>
    {
        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.ToTable("Transactions");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            builder.Property(t => t.Amount).HasColumnType("decimal(18,4)").IsRequired();
            builder.Property(t => t.BalanceBefore).HasColumnType("decimal(18,4)").IsRequired();
            builder.Property(t => t.BalanceAfter).HasColumnType("decimal(18,4)").IsRequired();
            builder.Property(t => t.TransactionType).HasConversion<int>().IsRequired();
            builder.Property(t => t.CreatedAt).IsRequired();
            builder.Property(t => t.Description).HasMaxLength(500).IsRequired(false);


            builder.HasOne(t => t.Wallet)
                   .WithMany(w => w.Transactions)
                   .HasForeignKey(t => t.WalletId)      
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
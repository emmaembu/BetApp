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
    public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessageEntity>
    {
        public void Configure(EntityTypeBuilder<OutboxMessageEntity> builder)
        {
            builder.ToTable("OutboxMessages");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Type).IsRequired();
            builder.Property(o => o.Payload).IsRequired();

        }
    }
}

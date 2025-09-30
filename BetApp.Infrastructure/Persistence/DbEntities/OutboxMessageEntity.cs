using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Infrastructure.Persistence.DbEntities
{
    public class OutboxMessageEntity
    {
        public Guid Id { get; set;} 
        public DateTime OccuredOn { get; set;}

        public Guid AggregateId { get; set; }
        public string Type { get; set; } = string.Empty;

        public string Payload { get; set;} = string.Empty;

        public DateTime? ProcessedOn { get; set;}

        public string Error { get; set; } = string.Empty;
    }
}

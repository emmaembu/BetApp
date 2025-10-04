using BetApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Domain.Events
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }

        public Guid AggregateId { get; set; }
        public DateTime OccuredOn { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedOn { get; set; } 
        public string Type { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;

        public string? Error { get; set; }

        public OutboxMessage(string type, Guid aggregateId, string payload) 
        {
            Type = type;
            AggregateId = aggregateId;  
            Payload = payload;
        }
    }
}

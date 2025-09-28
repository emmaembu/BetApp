using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Domain.Entities
{
    public class Match
    {
        public Guid Id { get; set; }
        public string HomeTeam { get; set; } = null!;
        public string AwayTeam { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public ICollection Markets { get; set; } = new List<Market>();
    }
}

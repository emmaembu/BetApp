using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Application.DTOs
{
    public class MatchDto
    {
        public Guid Id {  get; set; }

        public string Home { get; set; } = null!;

        public string Away { get; set; } = null!;

        public DateTime StartTime { get; set; }
    }
}

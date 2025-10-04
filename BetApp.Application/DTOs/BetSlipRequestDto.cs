using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Application.DTOs
{
    public class BetSlipRequestDto
    {
        public Guid WalletId { get; set; }

        public List<BetItemDto> Items { get; set; } = new();
    }
}

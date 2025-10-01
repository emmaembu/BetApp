using BetApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Application.Interfaces
{
    public interface IBetSlipValidator
    {
        Task ValidateAsync(BetSlip betSlip);
    }
}

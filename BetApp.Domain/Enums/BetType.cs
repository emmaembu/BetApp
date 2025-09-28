using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Domain.Enums
{
    public enum BetType
    {
        Home = 1,       // 1
        Away = 2,       // 2
        Draw = 3,       // X
        HomeOrDraw = 4, // 1X
        DrawOrAway = 5, // X2
        HomeOrAway = 6  // 12
    }
}

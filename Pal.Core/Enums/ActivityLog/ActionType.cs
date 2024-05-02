using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Enums.ActivityLog
{
    public enum ActionType: int
    {
        Add = 0,
        Update = 1,
        Delete = 2,
        Repeat = 3,
        Approve = 4,
        DisApprove = 5,
        View = 6,
        Visite = 7,
        DeletePermenantly = 8,
        Restore = 9,
    }
}

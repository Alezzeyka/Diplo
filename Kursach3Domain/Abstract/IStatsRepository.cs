using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;

namespace Kursach3Domain.Abstract
{
    public interface IStatsRepository
    {
        IEnumerable<UserStats> Stats { get; }
        void SaveStat(UserStats stats);
        UserStats DeleteStats(int ID);
    }
}

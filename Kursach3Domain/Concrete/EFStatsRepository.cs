using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;
using Kursach3Domain.Abstract;

namespace Kursach3Domain.Concrete
{
    public class EFStatsRepository : IStatsRepository
    {
        EFTestDbContext context = new EFTestDbContext();

        public IEnumerable<UserStats> Stats
        {
            get { return context.Stats; }
        }
        public void SaveStat(UserStats stats)
        {
            if (stats.Id == 0)
                context.Stats.Add(stats);
            else
            {
                UserStats dbEntry = context.Stats.Find(stats.Id);
                if (dbEntry != null)
                {
                    dbEntry.score = stats.score;
                }
            }
            context.SaveChanges();
        }
        public UserStats DeleteStats(int StatId)
        {
            UserStats dbEntry = context.Stats.Find(StatId);
            if (dbEntry != null)
            {
                context.Stats.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
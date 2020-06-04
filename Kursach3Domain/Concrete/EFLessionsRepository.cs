using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;
using Kursach3Domain.Abstract;

namespace Kursach3Domain.Concrete
{
    public class EFLessionsRepository : ILessionssRepository
    {
        EFTestDbContext context = new EFTestDbContext();

        public IEnumerable<Lessions> Lessions 
        {
            get { return context.Lessions; }
        }
        public void Savelession(Lessions lessions)
        {
            if (lessions.Id == 0)
                context.Lessions.Add(lessions);
            else
            {
                Lessions dbEntry = context.Lessions.Find(lessions.Id);
                if (dbEntry != null)
                {
                    dbEntry.Theme=lessions.Theme;
                    dbEntry.Course = lessions.Course;
                    dbEntry.ImgId = lessions.ImgId;
                }
            }
            context.SaveChanges();
        }
        public Lessions DeleteLession(int LessionId)
        {
            Lessions dbEntry = context.Lessions.Find(LessionId);
            if (dbEntry != null)
            {
                context.Lessions.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
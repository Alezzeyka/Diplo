using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;
using Kursach3Domain.Abstract;

namespace Kursach3Domain.Concrete
{
    public class EFPicturesRepository : IPicturesRepository
    {
        EFTestDbContext context = new EFTestDbContext();

        public IEnumerable<Picture> Pictures
        {
            get { return context.Pictures; }
        }
        public void SavePicture(Picture pic)
        {
            if (pic.Id == 0)
                context.Pictures.Add(pic);
            else
            {
                Picture dbEntry = context.Pictures.Find(pic.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = pic.Name;
                    dbEntry.Image = pic.Image;
                }
            }
            context.SaveChanges();
        }
        public Picture DeletePicture(int ID)
        {
            Picture dbEntry = context.Pictures.Find(ID);
            if (dbEntry != null)
            {
                context.Pictures.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}

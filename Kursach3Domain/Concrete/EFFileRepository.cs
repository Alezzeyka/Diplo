using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;
using Kursach3Domain.Abstract;

namespace Kursach3Domain.Concrete
{
    public class EFFileRepository:IFileRepository
    {
        EFTestDbContext context = new EFTestDbContext();

        public IEnumerable<Files> _Files
        {
            get { return context.Files; }
        }
        public void SaveFile(Files File)
        {
            if (File.Id == 0)
                context.Files.Add(File);
            else
            {
                Files dbEntry = context.Files.Find(File.Id);
                if (dbEntry != null)
                {
                    dbEntry.FileName = File.FileName;
                    dbEntry.FileType = File.FileType;
                    dbEntry.FilePath = File.FilePath;
                    dbEntry.Name = File.Name;
                }
            }
            context.SaveChanges();
        }
        public Files DeleteFile(int ID)
        {
            Files dbEntry = context.Files.Find(ID);
            if (dbEntry != null)
            {
                context.Files.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
        public void DeleteFiles(IEnumerable<Files> File)
        {
            context.Files.RemoveRange(File);
            context.SaveChanges();
            return;
        }

    }
}

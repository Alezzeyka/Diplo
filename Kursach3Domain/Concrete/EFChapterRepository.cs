using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;
using Kursach3Domain.Abstract;

namespace Kursach3Domain.Concrete
{
    public class EFChapterRepository : IChapterRepository
    {
        EFTestDbContext context = new EFTestDbContext();

        public IEnumerable<Chapter> _Chapters
        {
            get { return context.Chapters; }
        }
        public void SaveChapter(Chapter chapter)
        {
            if (chapter.Id == 0)
                context.Chapters.Add(chapter);
            else
            {
                Chapter dbEntry = context.Chapters.Find(chapter.Id);
                if (dbEntry != null)
                {
                    dbEntry.ImgId = chapter.ImgId;
                    dbEntry.LessionId = chapter.LessionId;
                    dbEntry.PdfId = chapter.PdfId;
                    dbEntry.VideoId = chapter.VideoId;
                    dbEntry.Text = chapter.Text;
                }
            }
            context.SaveChanges();
        }
        public Chapter DeleteChapter(int ID)
        {
            Chapter dbEntry = context.Chapters.Find(ID);
            if (dbEntry != null)
            {
                context.Chapters.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
        public void DeleteChapters(IEnumerable<Chapter> chapter)
        {
            context.Chapters.RemoveRange(chapter);
            context.SaveChanges();
            return;
        }

    }
}

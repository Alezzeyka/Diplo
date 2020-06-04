using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;

namespace Kursach3Domain.Abstract
{
    public interface IChapterRepository
    {
        IEnumerable<Chapter> _Chapters { get; }
        void SaveChapter(Chapter chapter);
        Chapter DeleteChapter(int ID);
        void DeleteChapters(IEnumerable<Chapter> chapters);
    }
}
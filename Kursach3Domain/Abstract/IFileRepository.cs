using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;

namespace Kursach3Domain.Abstract
{
    public interface IFileRepository
    {
        IEnumerable<Files> _Files { get; }
        void SaveFile(Files file);
        Files DeleteFile(int ID);
        void DeleteFiles(IEnumerable<Files> Files);
    }
}

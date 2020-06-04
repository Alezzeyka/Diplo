using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;

namespace Kursach3Domain.Abstract
{
    public interface IMultiChoiceRepository
    {
        IEnumerable<MultiChoice> Lines { get; }
        void SaveLine(MultiChoice Line);
        MultiChoice DeleteLine(int ID);
        void DeleteLines(IEnumerable<MultiChoice> Lines);
    }
}

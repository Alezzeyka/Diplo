using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;

namespace Kursach3Domain.Abstract
{
    public interface ILessionssRepository
    {
        IEnumerable<Lessions> Lessions { get; }
        void Savelession(Lessions lessions);
        Lessions DeleteLession(int ID);
    }
}

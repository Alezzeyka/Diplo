using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;

namespace Kursach3Domain.Abstract
{
    public interface IPicturesRepository
    {
        IEnumerable<Picture> Pictures { get; }
        void SavePicture(Picture pic);
        Picture DeletePicture(int ID);
    }
}

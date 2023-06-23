﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;

namespace Kursach3Domain.Abstract
{
    public interface ISubjectRepository
    {
        IEnumerable<Subject> _subjects { get; }
        void SaveSubject(Subject subject);
        Subject DeleteSubject(int ID);
        void DeleteSubjects(IEnumerable<Subject> subjects);
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;

namespace Kursach3Domain.Abstract
{
    public interface IAnswersRepository
    {
        IEnumerable<Answers> Answers { get; }
        void SaveAnswer(Answers answers);
        Answers DeleteAnswer(int ID);
        void DeleteAnswers(IEnumerable<Answers> answers);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach3Domain.Entities;

namespace Kursach3Domain.Abstract
{
    public interface IQuestionRepository
    {
        IEnumerable<Question> Question { get; }
        void SaveQuestion(Question question);
        Question DeleteQuest(int ID);
    }
}

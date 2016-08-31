using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIApplication.Models
{
    public class TestModel
    {
        public string Name;
        public int Time;
        public int Id;
        public int QCategoryId;
        public int QSubcategoryId;
        public List<QuestionModel> Questions;
        public List<int> RemovedAnswersIds;

    }
}

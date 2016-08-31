using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIApplication.Models
{
    public class QuestionModel
    {
        public int Id;
        public string Text;
        public int QTypeId;
        public List<AnswerModel> Answers;
    }
}

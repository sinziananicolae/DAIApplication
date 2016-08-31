using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAIApplication.Models
{
    public class AnswerModel
    {
        public int Id;
        public int QuestionId;
        public string Answer;
        public bool Correct;
    }
}

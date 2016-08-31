using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAIApplication.Data.Database;

namespace DAIApplication.Services.Answer
{
    public class AnswerService
    {

        private DbEntities _dbEntities;

        public AnswerService()
        {
            _dbEntities = new DbEntities();
        }

        public object GetAllAnswersForQuestion(int questionId)
        {
            IEnumerable<QAnswer> allAnswers = _dbEntities.QAnswers.Where(f => f.QuestionId == questionId).ToList();
            List<object> allAnswersList = new List<object>();

            foreach (QAnswer answer in allAnswers)
            {
                allAnswersList.Add(new
                {
                    answer.Id,
                    answer.Answer,
                    answer.Correct
                });
            }

            return allAnswersList;
        }

        public object SaveAnswers(IList<QAnswer> answers, int questionId)
        {
            foreach (QAnswer answer in answers)
            {
                answer.QuestionId = questionId;

                _dbEntities.QAnswers.Add(answer);
                _dbEntities.SaveChanges();
            }

            return new
            {
                success = true,
                message = "Success",
                data = new { }
            };
        }
    }
}

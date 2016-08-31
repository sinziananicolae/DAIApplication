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
                if (answer.Id == 0)
                {
                    answer.QuestionId = questionId;
                    _dbEntities.QAnswers.Add(answer);
                }
                else
                {
                    var originalAnswer = _dbEntities.QAnswers.Find(answer.Id);

                    if (originalAnswer != null)
                    {
                        originalAnswer.Answer = answer.Answer;
                        originalAnswer.Correct = answer.Correct;
                    }
                }

                _dbEntities.SaveChanges();
            }

            return new
            {
                success = true,
                message = "Success",
                data = new { }
            };
        }

        public void RemoveAnswer(int answerId)
        {
            _dbEntities.QAnswers.Remove(_dbEntities.QAnswers.FirstOrDefault(f => f.Id == answerId));
            _dbEntities.SaveChanges();
        }
    }
}

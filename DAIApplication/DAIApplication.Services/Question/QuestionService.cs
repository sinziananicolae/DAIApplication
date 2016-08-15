using System.Collections.Generic;
using System.Linq;
using DAIApplication.Data.Database;
using DAIApplication.Services.Answer;

namespace DAIApplication.Services.Question
{
    public class QuestionService
    {
        private DbEntities _dbEntities;
        private AnswerService _answerService;

        public QuestionService()
        {
            _dbEntities = new DbEntities();
            _answerService = new AnswerService();
        }

        public List<object> GetAllQuestions()
        {
            List<object> allQuestionsList = new List<object>();

            IEnumerable<Data.Database.Question> allQuestions = _dbEntities.Questions.ToList();
            foreach (Data.Database.Question question in allQuestions)
            {
                allQuestionsList.Add(new
                {
                    question.Id,
                    question.QTypeId,
                    question.Text,
                    question.QCategoryId,
                    question.QSubcategoryId,
                    question.Time,
                    Answers = _answerService.GetAllAnswersForQuestion(question.Id)
                });
            }

            return allQuestionsList;
        }

        public List<object> GetQuestionsBySubcategory(int subcategoryId)
        {
            List<object> allQuestionsList = new List<object>();

            IEnumerable<Data.Database.Question> allQuestions = _dbEntities.Questions.ToList().Where(f => f.QSubcategoryId == subcategoryId);
            foreach (Data.Database.Question question in allQuestions)
            {
                allQuestionsList.Add(new
                {
                    question.Id,
                    question.QTypeId,
                    question.Text,
                    question.QCategoryId,
                    question.QSubcategoryId,
                    question.Time,
                    Answers = _answerService.GetAllAnswersForQuestion(question.Id)
                });
            }

            return allQuestionsList;
        }

        public object AddQuestion(Data.Database.Question question, IList<QAnswer> answers)
        {
            if (question.QTypeId == 0 || question.QCategoryId == 0 ||
                question.Text == null || question.Time == null || question.UserId == null)
                return new
                {
                    success = false,
                    message = "Field missing"
                };

            _dbEntities.Questions.Add(question);
            _dbEntities.SaveChanges();

            _answerService.SaveAnswers(answers, question.Id);

            return new
            {
                success = true,
                message = "Success",
                data = new
                {
                    question.Id
                }
            };
        }
    }
}

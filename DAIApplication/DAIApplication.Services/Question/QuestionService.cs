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
                    Answers = _answerService.GetAllAnswersForQuestion(question.Id)
                });
            }

            return allQuestionsList;
        }

        public List<object> GetQuestionsByTestId(int testId)
        {
            List<object> allQuestionsList = new List<object>();

            IEnumerable<QuestionInTest> allQuestions = _dbEntities.QuestionInTests.ToList().Where(f => f.TestId == testId);
            foreach (QuestionInTest questionInTest in allQuestions)
            {
                var question = _dbEntities.Questions.FirstOrDefault(f => f.Id == questionInTest.QuestionId);
                if (question != null)
                    allQuestionsList.Add(new
                    {
                        question.Id,
                        question.QTypeId,
                        question.Text,
                        Answers = _answerService.GetAllAnswersForQuestion(question.Id)
                    });
            }

            return allQuestionsList;
        }

        public int AddQuestion(Data.Database.Question question, List<QAnswer> answers)
        {
            if (question.QTypeId == 0 || question.Text == null || question.UserId == null)
                return -1;

            _dbEntities.Questions.Add(question);
            _dbEntities.SaveChanges();

            _answerService.SaveAnswers(answers, question.Id);

            return question.Id;
        }

        public List<object> GetQuestionTypes()
        {
            List<object> allQuestionsTypesList = new List<object>();

            IEnumerable<QType> allQuestionTypes = _dbEntities.QTypes.ToList();
            foreach (QType qType in allQuestionTypes)
            {
                allQuestionsTypesList.Add(new
                {
                    qType.Id,
                    qType.Name
                });
            }

            return allQuestionsTypesList;
        }
    }
}

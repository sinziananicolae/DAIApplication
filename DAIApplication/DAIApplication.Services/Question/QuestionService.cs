using System;
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

        public List<object> GetQuestionsByTestId(int testId, string userRole)
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
                        Answers = _answerService.GetAllAnswersForQuestion(question.Id, userRole)
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

        public int UpdateQuestion(Data.Database.Question question, List<QAnswer> answers)
        {

            var originalQuestion = _dbEntities.Questions.Find(question.Id);

            if (originalQuestion != null)
            {
                originalQuestion.Text = question.Text;
                _dbEntities.SaveChanges();
            }

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

        public bool DeleteQuestion(int questionId)
        {
            try
            {
                QuestionInTest qt = _dbEntities.QuestionInTests.FirstOrDefault(f => f.QuestionId == questionId);
                _dbEntities.QuestionInTests.Remove(qt);
                _dbEntities.SaveChanges();

                var questionToDelete = _dbEntities.Questions.Find(questionId);
                _dbEntities.Questions.Remove(questionToDelete);
                _dbEntities.SaveChanges();
            }
            catch (Exception)
            {

                return false;
            }
            

            return true;
        }
    }
}

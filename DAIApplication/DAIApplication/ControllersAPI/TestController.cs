using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAIApplication.Data.Database;
using DAIApplication.Models;
using DAIApplication.Services.Answer;
using DAIApplication.Services.Question;
using DAIApplication.Services.Test;
using Microsoft.AspNet.Identity;

namespace DAIApplication.ControllersAPI
{
    public class TestController : ApiController
    {
        private QuestionService _questionService;
        private AnswerService _answerService;
        private TestService _testService;

        public TestController()
        {
            _questionService = new QuestionService();
            _answerService = new AnswerService();
            _testService = new TestService();
        }

        // GET api/test
        public object Get()
        {
            var userId = User.Identity.GetUserId();

            var tests = _testService.GetTestsByUserId(userId);

            return new
            {
                success = true,
                data = tests
            };
        }

        [HttpGet]
        [Route("api/test/all")]
        public object GetAllTests()
        {
            var tests = _testService.GetAllTests();

            return new
            {
                success = true,
                data = tests
            };
        }


        [HttpGet]
        public object Get(int id)
        {
            var test = _testService.GetTestById(id);

            return new
            {
                success = true,
                data = test
            };
        }

        // POST api/test
        public object Post([FromBody]TestModel test)
        {
            var userId = User.Identity.GetUserId();
            List<int> questionsIds = new List<int>();

            foreach (QuestionModel question in test.Questions)
            {
                Question currentQuestion = new Question
                {
                    QTypeId = question.QTypeId,
                    Text = question.Text,
                    UserId = userId
                };

                List<QAnswer> answersList = new List<QAnswer>();
                foreach (AnswerModel answer in question.Answers)
                {
                    QAnswer currentAnswer = new QAnswer
                    {
                        Answer = answer.Answer,
                        Correct = answer.Correct
                    };
                    answersList.Add(currentAnswer);
                }

                var questionId = _questionService.AddQuestion(currentQuestion, answersList);
                questionsIds.Add(questionId);
            }

            Test newTest = new Test
            {
                CategoryId = test.QCategoryId,
                SubcategoryId = test.QSubcategoryId,
                Name = test.Name,
                Time = test.Time,
                UserId = userId
            };
            _testService.AddTest(newTest, questionsIds);


            return new { };
        }

        // PUT api/test
        public object Put([FromBody]TestModel test)
        {
            var userId = User.Identity.GetUserId();
            List<int> questionsIds = new List<int>();

            foreach (QuestionModel question in test.Questions)
            {
                Question currentQuestion = new Question
                {
                    QTypeId = question.QTypeId,
                    Text = question.Text,
                    UserId = userId,
                    Id = question.Id
                };

                List<QAnswer> answersList = new List<QAnswer>();
                foreach (AnswerModel answer in question.Answers)
                {
                    QAnswer currentAnswer = new QAnswer
                    {
                        Answer = answer.Answer,
                        Correct = answer.Correct,
                        Id = answer.Id
                    };
                    answersList.Add(currentAnswer);
                }

                if (question.Id == 0)
                {
                    var questionId = _questionService.AddQuestion(currentQuestion, answersList);
                    questionsIds.Add(questionId);
                }
                else
                {
                    _questionService.UpdateQuestion(currentQuestion, answersList);
                }
            }

            Test newTest = new Test
            {
                CategoryId = test.QCategoryId,
                SubcategoryId = test.QSubcategoryId,
                Name = test.Name,
                Time = test.Time,
                UserId = userId,
                Id = test.Id
            };

            if (test.Id == 0)
                _testService.AddTest(newTest, questionsIds);
            else
                _testService.UpdateTest(newTest, questionsIds);


            foreach (int answerId in test.RemovedAnswersIds)
            {
                _answerService.RemoveAnswer(answerId);
            }

            return new { };
        }

        [HttpDelete]
        public object Delete(int id)
        {
            var test = _testService.DeleteTest(id);

            return new
            {
                success = true,
                data = test
            };
        }
    }
}

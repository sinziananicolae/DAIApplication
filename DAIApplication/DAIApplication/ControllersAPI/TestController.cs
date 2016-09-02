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
using DAIApplication.Services.UserService;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAIApplication.ControllersAPI
{
    public class TestController : ApiController
    {
        private QuestionService _questionService;
        private AnswerService _answerService;
        private TestService _testService;
        private UserService _userService;
        private UserManager<ApplicationUser> UserManager { get; set; }
        private ApplicationDbContext ApplicationDbContext { get; set; }


        public TestController()
        {
            _questionService = new QuestionService();
            _answerService = new AnswerService();
            _testService = new TestService();
            _userService = new UserService();
            ApplicationDbContext = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
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
        [Route("api/test/{id}")]
        public object Get(int id)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var userRole = _userService.GetUserRole(user.Email);

            var test = _testService.GetTestById(id, userRole);

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

            foreach (int questionId in test.RemovedQuestionsIds)
            {
                _questionService.DeleteQuestion(questionId);
            }

            return new { };
        }

        [HttpDelete]
        [Route("api/test/{id}")]
        public object Delete(int id)
        {
            var test = _testService.DeleteTest(id);

            return new
            {
                success = true,
                data = test
            };
        }

        [HttpPost]
        [Route("api/test/{id}")]
        public object SubmitTest(int id, [FromBody] TestSubmissionModel test)
        {
            var userId = User.Identity.GetUserId();
            var tests = _testService.AssessTest(id, test.Answers, test.Time, userId);

            return tests;
        }

        [HttpGet]
        [Route("api/test-result/{testId}/{resultId?}")]
        public object GetTestResults(int testId, int resultId = 0)
        {
            var userId = User.Identity.GetUserId();
            var test = _testService.GetTestSummary(testId, resultId, userId);

            return test;
        }

        [HttpGet]
        [Route("api/admin-test/{testId}")]
        public object GetAdminTestInfo(int testId)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var userRole = _userService.GetUserRole(user.Email);

            var test = _testService.GetTestById(testId, userRole);
            var testInfo = _testService.GetAdminTestInfo(testId);

            return new
            {
                success = true,
                data = new
                {
                  Test = test,
                  TestInfo = testInfo  
                }
            };
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAIApplication.Data.Database;
using DAIApplication.Services.Question;

namespace DAIApplication.Services.Test
{
    public class TestService
    {
        private readonly DbEntities _dbEntities;
        private readonly QuestionService _questionService;

        public TestService()
        {
            _dbEntities = new DbEntities();
            _questionService = new QuestionService();
        }

        public List<object> GetAllTests()
        {
            List<object> allTestsList = new List<object>();

            IEnumerable<Data.Database.Test> allTests = _dbEntities.Tests.ToList();
            foreach (Data.Database.Test test in allTests)
            {
                allTestsList.Add(new
                {
                    test.Id,
                    Category = new
                    {
                        test.Category.Id,
                        test.Category.Name
                    },
                    Subcategory = new
                    {
                        test.Subcategory.Id,
                        test.Subcategory.Name
                    },
                    test.Name,
                    QuestionsNo = test.QuestionInTests.Count
                });
            }

            return allTestsList;
        }

        public object GetTestById(int testId)
        {

            var currentTest = _dbEntities.Tests.FirstOrDefault(f => f.Id == testId);

            var test = new
            {
                currentTest.Id,
                currentTest.Name,
                Category = new
                {
                    currentTest.Category.Id,
                    currentTest.Category.Name
                },
                Subcategory = new
                {
                    currentTest.Subcategory.Id,
                    currentTest.Subcategory.Name
                },
                currentTest.Time,
                Questions = _questionService.GetQuestionsByTestId(currentTest.Id)
            };

            return test;
        }

        public List<object> GetTestsByUserId(string userId)
        {
            List<object> allTestsList = new List<object>();

            IEnumerable<Data.Database.Test> allTests = _dbEntities.Tests.ToList().Where(f=> f.UserId == userId);
            foreach (Data.Database.Test test in allTests)
            {
                IEnumerable<UserTest> performedTests = _dbEntities.UserTests.ToList().Where(f => f.TestId == test.Id);

                var sumScore = 0;
                foreach (UserTest performedTest in performedTests)
                {
                    sumScore += performedTest.Score;
                }

                allTestsList.Add(new
                {
                    test.Id,
                    Category = new
                    {
                        test.Category.Id,
                        test.Category.Name
                    },
                    Subcategory = new
                    {
                        test.Subcategory.Id,
                        test.Subcategory.Name
                    },
                    test.Name,
                    QuestionsNo = test.QuestionInTests.Count,
                    test.Time,
                    Visits = test.UserTests.Count,
                    AvgScore = test.UserTests.Count == 0 ? 0 : sumScore / test.UserTests.Count
                });
            }

            return allTestsList;
        }

        public object AddTest(Data.Database.Test test, List<int> questionsIds)
        {
            _dbEntities.Tests.Add(test);
            _dbEntities.SaveChanges();

            foreach (int questionId in questionsIds)
            {
                QuestionInTest qt = new QuestionInTest();
                qt.QuestionId = questionId;
                qt.TestId = test.Id;

                _dbEntities.QuestionInTests.Add(qt);
                _dbEntities.SaveChanges();
            }

            return new
            {
                success = true,
                message = "Success",
                data = new
                {
                    test.Id
                }
            };
        }

        public object UpdateTest(Data.Database.Test test, List<int> questionsIds)
        {

            var originalTest = _dbEntities.Tests.Find(test.Id);

            if (originalTest != null)
            {
                originalTest.CategoryId = test.CategoryId;
                originalTest.SubcategoryId = test.SubcategoryId;
                originalTest.Name = test.Name;
                originalTest.Time = test.Time;
                _dbEntities.SaveChanges();
            }

            foreach (int questionId in questionsIds)
            {
                QuestionInTest qt = new QuestionInTest();
                qt.QuestionId = questionId;
                qt.TestId = test.Id;

                _dbEntities.QuestionInTests.Add(qt);
                _dbEntities.SaveChanges();
            }

            return new
            {
                success = true,
                message = "Success",
                data = new
                {
                    test.Id
                }
            };
        }

        public object AddUserTest(UserTest userTest)
        {
            if (userTest.TestId == 0 || userTest.UserId == null)
                return new
                {
                    success = false,
                    message = "Field missing"
                };

            _dbEntities.UserTests.Add(userTest);
            _dbEntities.SaveChanges();

            return new
            {
                success = true,
                message = "Success",
                data = new
                {
                    userTest.Id
                }
            };
        }

        public object AssessTest(int testId, IList<int> answersIds, string time, string userId)
        {
            var maximumScore = answersIds.Count();
            int currentScore = 0;

            foreach (int answerId in answersIds)
            {
                var answer = _dbEntities.QAnswers.FirstOrDefault(f => f.Id == answerId);
                if (answer.Correct)
                {
                    currentScore++;
                }
            }

            UserTest ut = new UserTest();
            ut.TestId = testId;
            ut.UserId = userId;
            ut.MaxScore = maximumScore;
            ut.Score = currentScore;
            ut.Time = time;

            AddUserTest(ut);

            return new
            {
                success = true,
                message = "Success",
                data = new { }
            };
        }

    }
}

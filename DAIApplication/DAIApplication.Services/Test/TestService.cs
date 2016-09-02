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
                    CategoryId = test.Category.Id,
                    CategoryName = test.Category.Name,
                    SubcategoryId = test.Subcategory.Id,
                    SubcategoryName = test.Subcategory.Name,
                    test.Name,
                    test.Time,
                    test.Timestamp,
                    QuestionsNo = test.QuestionInTests.Count,
                    TestResults = test.UserTests.Select(f => new { f.Time, f.Score }).ToList()
                });
            }

            return allTestsList;
        }

        public object GetTestById(int testId, string userRole)
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
                Questions = _questionService.GetQuestionsByTestId(currentTest.Id, userRole),
                currentTest.Timestamp
            };

            return test;
        }

        public List<object> GetTestsByUserId(string userId)
        {
            List<object> allTestsList = new List<object>();

            IEnumerable<Data.Database.Test> allTests = _dbEntities.Tests.ToList().Where(f => f.UserId == userId);
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
                    AvgScore = test.UserTests.Count == 0 ? 0 : sumScore / test.UserTests.Count,
                    test.Timestamp
                });
            }

            return allTestsList;
        }

        public object AddTest(Data.Database.Test test, List<int> questionsIds)
        {
            test.Timestamp = DateTime.Now;
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

        public int AddUserTest(UserTest userTest)
        {
            if (userTest.TestId == 0 || userTest.UserId == null)
                return 0;

            userTest.Timestamp = DateTime.Now;
            _dbEntities.UserTests.Add(userTest);
            _dbEntities.SaveChanges();

            return userTest.Id;
        }

        public object AssessTest(int testId, Dictionary<int, List<int>> answers, int time, string userId)
        {
            Data.Database.Test test = _dbEntities.Tests.FirstOrDefault(f => f.Id == testId);
            var maximumScore = test.QuestionInTests.Count;
            var currentScore = 0;

            foreach (var kvp in answers)
            {
                var questionId = kvp.Key;
                List<int> chosenAnswers = kvp.Value;

                Data.Database.Question question = _dbEntities.Questions.FirstOrDefault(f => f.Id == questionId);
                if (question.QTypeId == 1 && chosenAnswers.Count > 0)
                {
                    var answer = question.QAnswers.FirstOrDefault(f => f.Correct == true);
                    if (answer.Id == chosenAnswers[0])
                    {
                        currentScore += 1;
                    }
                }
                else if (chosenAnswers.Count > 0)
                {
                    var correctAnswers = _dbEntities.QAnswers.Where(f => f.QuestionId == question.Id && f.Correct).Select(item => item.Id).ToList();
                    var isEqual = new HashSet<int>(chosenAnswers).SetEquals(correctAnswers);
                    if (isEqual)
                        currentScore += 1;
                }
            }

            UserTest ut = new UserTest
            {
                TestId = testId,
                UserId = userId,
                MaxScore = maximumScore,
                Score = currentScore,
                Time = time
            };

            var id = AddUserTest(ut);

            return new
            {
                success = true,
                message = "Success",
                data = new { Id = id }
            };
        }

        public object DeleteTest(int id)
        {
            try
            {
                var testToDelete = _dbEntities.Tests.Find(id);
                List<int> qIds = new List<int>();
                foreach (QuestionInTest question in testToDelete.QuestionInTests)
                {
                    qIds.Add(question.QuestionId);
                }

                _dbEntities.Tests.Remove(testToDelete);
                _dbEntities.SaveChanges();

                foreach (int qId in qIds)
                {
                    var questionToDelete = _dbEntities.Questions.Find(qId);
                    _dbEntities.Questions.Remove(questionToDelete);
                    _dbEntities.SaveChanges();
                }
            }
            catch (Exception e)
            {

                return new
                {
                    success = false,
                    message = e.Message,
                    data = new { }
                };
            }
            return new
            {
                success = true,
                message = "Success",
                data = new { }
            };
        }

        public object GetTestSummary(int testId, int resultId, string userId)
        {
            var uts = _dbEntities.UserTests.Where(f => f.UserId == userId && f.TestId == testId).Select(g => new { g.Score, g.Time, g.Timestamp, g.Id }).ToList();
            var ut = _dbEntities.UserTests.Where(f => f.TestId == testId && f.UserId == userId && f.Id == resultId).Select(g => new { g.Score, g.Time, g.Timestamp.Value, g.Id }).ToList();

            return new
            {
                success = true,
                data = new
                {
                    AllResults = uts,
                    LastResult = ut
                }
            };
        }

    }
}

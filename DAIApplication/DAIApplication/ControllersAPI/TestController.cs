using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAIApplication.Models;
using DAIApplication.Services.Answer;
using DAIApplication.Services.Question;
using DAIApplication.Services.Test;

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

        // POST api/test
        public object Post([FromBody]TestModel test)
        {

            return new {};
        }

    }
}

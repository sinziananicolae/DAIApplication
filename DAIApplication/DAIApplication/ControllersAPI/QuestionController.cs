using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAIApplication.Services.Question;

namespace DAIApplication.ControllersAPI
{
    public class QuestionController : ApiController
    {
        private QuestionService _QuestionService;

        public QuestionController()
        {
            _QuestionService = new QuestionService();
        }

        [HttpGet]
        [Route("api/qTypes")]
        public object GetQTypes()
        {
            var qTypes = _QuestionService.GetQuestionTypes();
            return new
            {
                success = true,
                message = "",
                data = qTypes
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DAIApplication.Models;
using DAIApplication.Services.Answer;
using DAIApplication.Services.Category;
using DAIApplication.Services.Question;
using DAIApplication.Services.Test;

namespace DAIApplication.ControllersAPI
{
    public class CategoryController : ApiController
    {
        private CategoryService _categoryService;

        public CategoryController()
        {
            _categoryService = new CategoryService();
        }

        // Get api/category
        public object Get()
        {
            var categories = _categoryService.GetAllCategories();
            return new
            {
                success = true,
                data = categories
            };
        }

    }
}

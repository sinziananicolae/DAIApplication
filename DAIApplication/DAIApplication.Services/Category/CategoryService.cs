using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAIApplication.Data.Database;

namespace DAIApplication.Services.Category
{
    public class CategoryService
    {
        private readonly DbEntities _dbEntities;

        public CategoryService()
        {
            _dbEntities = new DbEntities();
        }

        public List<object> GetAllCategories()
        {
            List<object> allCategoriesList = new List<object>();
            IEnumerable<Data.Database.Category> allCategories = _dbEntities.Categories.ToList();
            foreach (Data.Database.Category category in allCategories)
            {
                IEnumerable<Subcategory> currentCategorySubcategories = _dbEntities.Subcategories.ToList().Where(f => f.CategoryId == category.Id);
                List<object> subcategoriesForCategory = new List<object>();
                foreach (Subcategory subCategory in currentCategorySubcategories)
                {
                    subcategoriesForCategory.Add(new
                    {
                        subCategory.Id,
                        subCategory.Name,
                        subCategory.CategoryId
                    });
                }
                allCategoriesList.Add(new
                {
                    category.Id,
                    category.Name,
                    Subcategories = subcategoriesForCategory
                });
            }

            return allCategoriesList;
        }
    }
}

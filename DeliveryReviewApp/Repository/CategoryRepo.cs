using DeliveryReviewApp.Data;
using DeliveryReviewApp.Interfaces;
using PackageReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryReviewApp.Repository
{
    public class CategoryRepo : ICategory
    {
        private readonly DataContext _context;

        public CategoryRepo(DataContext context)
        {
            _context = context;
        }
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            //change tracker: adding, updating, modifying
            _context.Add(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Package> GetPackageByCategory(int categoryId)
        {
            return _context.PackageCategories.Where(c => c.CategoryId == categoryId).Select(c => c.Package).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }
    }
}

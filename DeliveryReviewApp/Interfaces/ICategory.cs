using PackageReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryReviewApp.Interfaces
{
    public interface ICategory
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
        ICollection<Package> GetPackageByCategory(int categoryId);
        bool CategoryExists(int id);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();

    }
}

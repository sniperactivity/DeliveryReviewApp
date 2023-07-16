using DeliveryReviewApp.Data;
using DeliveryReviewApp.Dto;
using DeliveryReviewApp.Interfaces;
using DeliveryReviewApp.Models;
using PackageReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryReviewApp.Repository
{
    public class PackageRepo:IPackage
    {
        private readonly DataContext _context;

        public PackageRepo(DataContext context)
        {
            _context = context;
        }

        public bool CreatePackage(int ownerId, int categoryId, Package package)
        {
            var packageOwnerEntity = _context.Owners.Where(a => a.Id == ownerId).FirstOrDefault();
            var Category = _context.Categories.Where(a => a.Id == categoryId).FirstOrDefault();

            var packageOwner = new PackageOwner()
            {
                Owner = packageOwnerEntity,
                Package = package,
            };
            _context.Add(packageOwner);

            var packageCatgory = new PackageCategory()
            {
                Category = Category,
                Package = package,
            };
            _context.Add(packageCatgory);
            _context.Add(package);

            return Save();
        }

        public bool DeletePackage(Package package)
        {
            _context.Remove(package);
            return Save();
        }

        public decimal GetDeliveryRating(int packId)
        {
            var review = _context.Reviews.Where(p => p.Package.Id == packId);

            if (review.Count() <= 0)
                return 0;
            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        public Package GetPackage(string name)
        {
            return _context.Packages.Where(p => p.Name == name).FirstOrDefault();
        }

        public ICollection<Package> GetPackages()
        {
            return _context.Packages.OrderBy(p => p.Id).ToList();
        }

        public Package GetPackages(int id)
        {
            return _context.Packages.Where(p => p.Id == id).FirstOrDefault();
        }

        public Package GetPackageTrimToUpper(PackageDto packageCreate)
        {
            return GetPackages().Where(c => c.Name.Trim().ToUpper() == packageCreate.Name.TrimEnd().ToUpper())
               .FirstOrDefault();
        }

        public bool PackageExists(int packId)
        {
            return _context.Packages.Any(p => p.Id == packId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePackage(int ownerId, int categoryId, Package package)
        {
            _context.Update(package);
            return Save();
        }
    }
}

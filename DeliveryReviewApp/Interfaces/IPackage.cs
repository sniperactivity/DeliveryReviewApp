using DeliveryReviewApp.Dto;
using PackageReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryReviewApp.Interfaces
{
    public interface IPackage
    {
        ICollection<Package> GetPackages();
        Package GetPackages(int id);
        Package GetPackage(string name);
        decimal GetDeliveryRating(int packId);
        bool PackageExists(int packId);
        bool CreatePackage(int ownerId, int categoryId, Package package);
        bool UpdatePackage(int ownerId, int categoryId, Package package);
        bool DeletePackage(Package package);
        Package GetPackageTrimToUpper(PackageDto packageCreate);
        bool Save();

    }
}

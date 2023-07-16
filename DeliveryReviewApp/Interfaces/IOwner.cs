using PackageReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryReviewApp.Interfaces
{
    public interface IOwner
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int ownerId);
        ICollection<Owner> GetOwnerOfPackage(int packId);
        ICollection<Package> GetPackageByOwner(int ownerId);
        bool OwnerExists(int ownerId);
        bool CreateOwner(Owner owner);
        bool UpdateOwner(Owner owner);
        bool DeleteOwner(Owner owner);
        bool Save();
    }
}

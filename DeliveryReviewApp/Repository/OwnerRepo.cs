using DeliveryReviewApp.Data;
using DeliveryReviewApp.Interfaces;
using PackageReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryReviewApp.Repository
{
    public class OwnerRepo : IOwner
    {
        private readonly DataContext _context;

        public OwnerRepo(DataContext context)
        {
            _context = context;
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _context.Remove(owner);
            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfPackage(int packId)
        {
            return _context.PackageOwners.Where(p => p.Package.Id == packId).Select(o => o.Owner).ToList();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Package> GetPackageByOwner(int ownerId)
        {
            return _context.PackageOwners.Where(p => p.Owner.Id == ownerId).Select(p => p.Package).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _context.Owners.Any(o => o.Id == ownerId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return Save();
        }
    }
}

using DeliveryReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageReviewApp.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DropDate { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<PackageOwner> PackageOwners { get; set; }
        public ICollection<PackageCategory> PackageCategories { get; set; }
    }
}

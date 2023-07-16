using DeliveryReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PackageReviewApp.Models
{
    public class Owner
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Country Country { get; set; }
        public ICollection<PackageOwner> PackageOwners { get; set; }
    }
}

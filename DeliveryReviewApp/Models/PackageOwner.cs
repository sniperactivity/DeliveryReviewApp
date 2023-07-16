using PackageReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryReviewApp.Models
{
    public class PackageOwner
    {
        public int PackageId { get; set; }
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
        public Package Package { get; set; }
    }
}

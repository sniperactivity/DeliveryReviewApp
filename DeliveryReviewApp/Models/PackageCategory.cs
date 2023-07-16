using PackageReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryReviewApp.Models
{
    public class PackageCategory
    {
        public int PackageId { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public Package Package { get; set; }
    }
}

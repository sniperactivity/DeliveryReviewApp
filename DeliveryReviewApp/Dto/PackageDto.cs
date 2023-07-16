using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryReviewApp.Dto
{
    public class PackageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DropDate { get; set; }
    }
}

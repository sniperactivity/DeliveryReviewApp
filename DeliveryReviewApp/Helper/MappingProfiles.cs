using AutoMapper;
using DeliveryReviewApp.Dto;
using DeliveryReviewApp.Models;
using PackageReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryReviewApp.Helper
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<Package, PackageDto>();
            CreateMap<PackageDto, Package>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Country, CountryDto>();
            CreateMap<CountryDto, Country>(); 
            CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerDto, Owner>(); 
            CreateMap<Review, ReviewDto>(); 
            CreateMap<ReviewDto, Review>(); 
            CreateMap<Reviewer, ReviewerDto>();
            CreateMap<ReviewerDto, Reviewer>();
        }
    }
}

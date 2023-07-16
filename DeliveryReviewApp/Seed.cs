using DeliveryReviewApp.Data;
using DeliveryReviewApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PackageReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryReviewApp
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.PackageOwners.Any())
            {
                var packageOwners = new List<PackageOwner>()
                {
                    new PackageOwner()
                    {
                        Package = new Package()
                        {
                            Name = "Phone",
                            DropDate = new DateTime(1903,1,1),
                            PackageCategories = new List<PackageCategory>()
                            {
                                new PackageCategory { Category = new Category() { Name = "Electric"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Title="Fastest Delivery",Text = "DoyleDelivery is the best delivery company ever, because it is Fast", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Sniper", LastName = "Yak" } },
                                new Review { Title="Respectful", Text = "driver was nice and respectful", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                                new Review { Title="Easy",Text = "Easiest delivery proccess ever!!!", Rating = 1,
                                Reviewer = new Reviewer(){ FirstName = "James", LastName = "Ade" } },
                            }
                        },
                        Owner = new Owner()
                        {
                            FirstName = "Jack",
                            LastName = "Daniels",
                            Country = new Country()
                            {
                                Name = "Nigeria"
                            }
                        }
                    },
                    new PackageOwner()
                    {
                        Package = new Package()
                        {
                            Name = "Soap",
                            DropDate = new DateTime(1903,1,1),
                            PackageCategories = new List<PackageCategory>()
                            {
                                new PackageCategory { Category = new Category() { Name = "Liquid"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Title="Fastest Delivery",Text = "DoyleDelivery is the best delivery company ever, because it is Fast", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Sniper", LastName = "Yak" } },
                                new Review { Title="Respectful", Text = "driver was nice and respectful", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                                new Review { Title="Easy",Text = "Easiest delivery proccess ever!!!", Rating = 1,
                                Reviewer = new Reviewer(){ FirstName = "James", LastName = "Ade" } },
                            }
                        },
                        Owner = new Owner()
                        {
                            FirstName = "Tunde",
                            LastName = "Baker",
                            Country = new Country()
                            {
                                Name = "Nigeria"
                            }
                        }
                    },
                    new PackageOwner()
                    {
                        Package = new Package()
                        {
                            Name = "Burger",
                            DropDate = new DateTime(1903,1,1),
                            PackageCategories = new List<PackageCategory>()
                            {
                                new PackageCategory { Category = new Category() { Name = "food"}}
                            },
                            Reviews = new List<Review>()
                            {
                                new Review { Title="Fastest Delivery",Text = "DoyleDelivery is the best delivery company ever, because it is Fast", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Sniper", LastName = "Yak" } },
                                new Review { Title="Respectful", Text = "driver was nice and respectful", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                                new Review { Title="Easy",Text = "Easiest delivery proccess ever!!!", Rating = 1,
                                Reviewer = new Reviewer(){ FirstName = "James", LastName = "Ade" } },
                            }
                        },
                        Owner = new Owner()
                        {
                            FirstName = "Animashaun",
                            LastName = "Femi",
                            Country = new Country()
                            {
                                Name = "Nigeria"
                            }
                        } 
                    }
                };
                dataContext.PackageOwners.AddRange(packageOwners);
                dataContext.SaveChanges();
            }
        }
        public static void SeedData(IApplicationBuilder app)
        {
            using var serviceScpoe = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScpoe.ServiceProvider.GetService<Seed>();
            context.SeedDataContext();
        }
    }
}

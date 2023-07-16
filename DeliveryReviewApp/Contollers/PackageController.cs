using AutoMapper;
using DeliveryReviewApp.Dto;
using DeliveryReviewApp.Interfaces;
using DeliveryReviewApp.Models;
using Microsoft.AspNetCore.Mvc;
using PackageReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryReviewApp.Contollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : Controller
    {
        private readonly IPackage _package;
        private readonly IMapper _mapper;
        private readonly IReview _review;

        public PackageController(IPackage package, IMapper mapper, IReview review)
        {
            _package = package;
            _mapper = mapper;
            _review = review;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Package>))]
        public IActionResult GetPackages()
        {
            var packages = _mapper.Map<List<PackageDto>>(_package.GetPackages());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(packages);
        }
        [HttpGet("{packId}")]
        [ProducesResponseType(200, Type = typeof(Package))]
        [ProducesResponseType(400)]
        public IActionResult GetPackage(int packId)
        {
            if (!_package.PackageExists(packId))
                return NotFound();

            var package = _mapper.Map<PackageDto>(_package.GetPackages(packId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(package);
        }
        [HttpGet("{packId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetDeliveryRating(int packId)
        {
            if (!_package.PackageExists(packId))
                return NotFound();

            var rating = _package.GetDeliveryRating(packId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(rating);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePackage([FromQuery] int ownerId, [FromQuery] int catId, [FromBody] PackageDto packageCreate)
        {
            if (packageCreate == null)
                return BadRequest(ModelState);

            var package = _package.GetPackageTrimToUpper(packageCreate);

            if (package != null)
            {
                ModelState.AddModelError("", "Package Already Exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var packageMap = _mapper.Map<Package>(packageCreate);

            if (!_package.CreatePackage(ownerId, catId, packageMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");
        }
        [HttpPut("{packId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int packId, [FromQuery] int ownerId, [FromQuery] int catId, [FromBody] PackageDto updatedPackage)
        {
            if (updatedPackage == null)
                return BadRequest(ModelState);

            if (packId != updatedPackage.Id)
                return BadRequest(ModelState);

            if (!_package.PackageExists(packId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var packageMap = _mapper.Map<Package>(updatedPackage);

            if (!_package.UpdatePackage(ownerId, catId, packageMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        [HttpDelete("{packId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int packId)
        {
            if (!_package.PackageExists(packId))
            {
                return NotFound();
            }

            var reviewsToDelete = _review.GetReviewsOfPackage(packId);
            var packageToDelete = _package.GetPackages(packId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_review.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting reviews");
            }

            if (!_package.DeletePackage(packageToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting package");
            }

            return NoContent();
        }
    }
}

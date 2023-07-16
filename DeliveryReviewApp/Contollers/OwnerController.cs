using AutoMapper;
using DeliveryReviewApp.Dto;
using DeliveryReviewApp.Interfaces;
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
    public class OwnerController : Controller
    {
        private readonly IOwner _owner;
        private readonly IMapper _mapper;
        private readonly ICountry _country;

        public OwnerController(IOwner owner, IMapper mapper, ICountry country)
        {
            _owner = owner;
            _mapper = mapper;
            _country = country;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_owner.GetOwners());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owners);
        }
        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int ownerId)
        {
            if (!_owner.OwnerExists(ownerId))
                return NotFound();

            var owner = _mapper.Map<OwnerDto>(_owner.GetOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owner);
        }
        [HttpGet("{ownerId}/package")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(Owner))]
        public IActionResult GetPackageByOwner(int ownerId)
        {
            if (!_owner.OwnerExists(ownerId))
            {
                return NotFound();
            }
            var package = _mapper.Map<PackageDto>(_owner.GetPackageByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(package);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int countryId ,[FromBody] OwnerDto ownerCreate)
        {
            if (ownerCreate == null)
                return BadRequest(ModelState);

            var owner = _owner.GetOwners()
                .Where(c => c.LastName.Trim().ToUpper() == ownerCreate.LastName.TrimEnd().ToUpper()).FirstOrDefault();

            if (owner != null)
            {
                ModelState.AddModelError("", "Owner Already Exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(ownerCreate);
            ownerMap.Country = _country.GetCountry(countryId);

            if (!_owner.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");
        }
        [HttpPut("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner(int ownerId, [FromBody]OwnerDto updatedOwner)
        {
            if (updatedOwner == null)
                return BadRequest(ModelState);
            if (ownerId != updatedOwner.Id)
                return BadRequest(ModelState);
            if (!_owner.OwnerExists(ownerId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();

            var ownerMap = _mapper.Map<Owner>(updatedOwner);

            if (!_owner.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something Went Wrong Updating");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int ownerId)
        {
            if (!_owner.OwnerExists(ownerId))
            {
                return NotFound();
            }

            var ownerToDelete = _owner.GetOwner(ownerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_owner.DeleteOwner(ownerToDelete))
            {
                ModelState.AddModelError("", "Error trying to delete");
            }
            return NoContent();
        }
    }
}

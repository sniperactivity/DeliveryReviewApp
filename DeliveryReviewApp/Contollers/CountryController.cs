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
    [Route("api/[Controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountry _country;
        private readonly IMapper _mapper;

        public CountryController(ICountry country, IMapper mapper)
        {
            _country = country;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_country.GetCountries());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countries);
        }

        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int countryId)
        {
            if (!_country.CountryExists(countryId))
                return NotFound();

            var country = _mapper.Map<CountryDto>(_country.GetCountry(countryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(country);
        }
        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(Country))]
        public IActionResult GetCountryByOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDto>(_country.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(country);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
        {
            if (countryCreate == null)
                return BadRequest(ModelState);

            var country = _country.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", "Country Already Exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(countryCreate);

            if (!_country.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");
        }
        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryId, [FromBody]CountryDto updatedCountry)
        {
            if (updatedCountry == null)
                return BadRequest(ModelState);
            if (countryId != updatedCountry.Id)
                return BadRequest(ModelState);
            if (!_country.CountryExists(countryId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();

            var countryMap = _mapper.Map<Country>(updatedCountry);

            if (!_country.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something Went Wrong Updating");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_country.CountryExists(countryId))
            {
                return NotFound();
            }

            var countryToDelete = _country.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_country.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting country");
            }

            return NoContent();
        }
    }
}

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
    public class CategoryController : Controller
    {
        private readonly ICategory _category;
        private readonly IMapper _mapper;

        public CategoryController(ICategory category, IMapper mapper)
        {
            _category = category;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>(_category.GetCategories());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(categories);
        }
        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_category.CategoryExists(categoryId))
                return NotFound();

            var category = _mapper.Map<CategoryDto>(_category.GetCategory(categoryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(category);
        }
        [HttpGet("package/{CategoryId}")]
        [ProducesResponseType(200, Type = typeof(Package))]
        [ProducesResponseType(400)]
        public IActionResult GetPackageByCategory(int categoryId)
        {
            var packages = _mapper.Map<List<PackageDto>>(_category.GetPackageByCategory(categoryId));
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(packages);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate)
        {
            if (categoryCreate == null)
                return BadRequest(ModelState);

            var category = _category.GetCategories()
                .Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if(category != null)
            {
                ModelState.AddModelError("", "Category Already Exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryCreate);

            if (!_category.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");
        }
        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody]CategoryDto updatedCategory)
        {
            if (updatedCategory == null)
                return BadRequest(ModelState);
            if (categoryId != updatedCategory.Id)
                return BadRequest(ModelState);
            if (!_category.CategoryExists(categoryId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();

            var categoryMap = _mapper.Map<Category>(updatedCategory);

            if (!_category.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something Went Wrong Updating");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_category.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var categoryToDelete = _category.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_category.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", "Error trying to delete");
            }
            return NoContent();
        }
    }
}

using AutoMapper;
using DeliveryReviewApp.Dto;
using DeliveryReviewApp.Interfaces;
using DeliveryReviewApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryReviewApp.Contollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewer _reviewer;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewer reviewer, IMapper mapper)
        {
            _reviewer = reviewer;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewer.GetReviewers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviewers);
        }
        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewer.ReviewerExists(reviewerId))
                return NotFound();

            var reviewer = _mapper.Map<ReviewerDto>(_reviewer.GetReviewer(reviewerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviewer);
        }
        [HttpGet("reviews/{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if (!_reviewer.ReviewerExists(reviewerId))
            {
                return NotFound();
            }
            var reviewer = _mapper.Map<List<ReviewDto>>(_reviewer.GetReviewsByReviewer(reviewerId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(reviewer);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerCreate)
        {
            if (reviewerCreate == null)
                return BadRequest(ModelState);

            var reviewer = _reviewer.GetReviewers()
                .Where(c => c.LastName.Trim().ToUpper() == reviewerCreate.LastName.TrimEnd().ToUpper()).FirstOrDefault();

            if (reviewer != null)
            {
                ModelState.AddModelError("", "Reviewer Already Exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ReviewerMap = _mapper.Map<Reviewer>(reviewerCreate);

            if (!_reviewer.CreateReviewer(ReviewerMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");
        }
        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto updatedReviewer)
        {
            if (updatedReviewer == null)
                return BadRequest(ModelState);

            if (reviewerId != updatedReviewer.Id)
                return BadRequest(ModelState);

            if (!_reviewer.ReviewerExists(reviewerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewerMap = _mapper.Map<Reviewer>(updatedReviewer);

            if (!_reviewer.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewer.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            var reviewerToDelete = _reviewer.GetReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewer.DeleteReviewer(reviewerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting reviewer");
            }

            return NoContent();
        }
    }
}

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
    public class ReviewController : Controller
    {
        private readonly IReview _review;
        private readonly IMapper _mapper;
        private readonly IPackage _package;
        private readonly IReviewer _reviewer;

        public ReviewController(IReview review, IMapper mapper, IPackage package, IReviewer reviewer)
        {
            _review = review;
            _mapper = mapper;
            _package = package;
            _reviewer = reviewer;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_review.GetReviews());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }
        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_review.ReviewExists(reviewId))
                return NotFound();

            var review = _mapper.Map<ReviewDto>(_review.GetReview(reviewId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(review);
        }
        [HttpGet("package/{packId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(Review))]
        public IActionResult GetReviewsOfPackage(int packId)
        {
            
            var reviews = _mapper.Map<List<ReviewDto>>(_review.GetReviewsOfPackage(packId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(reviews);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePackage([FromQuery] int reviewerId,[FromQuery] int packId, [FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate == null)
                return BadRequest(ModelState);

            var reviews = _review.GetReviews()
                .Where(c => c.Title.Trim().ToUpper() == reviewCreate.Title.TrimEnd().ToUpper()).FirstOrDefault();

            if (reviews != null)
            {
                ModelState.AddModelError("", "Review Already Exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewCreate);
            reviewMap.Package = _package.GetPackages(packId);
            reviewMap.Reviewer = _reviewer.GetReviewer(reviewerId);
            if (!_review.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");
        }
        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
        {
            if (updatedReview == null)
                return BadRequest(ModelState);

            if (reviewId != updatedReview.Id)
                return BadRequest(ModelState);

            if (!_review.ReviewExists(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewMap = _mapper.Map<Review>(updatedReview);

            if (!_review.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_review.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var reviewToDelete = _review.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_review.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting review");
            }

            return NoContent();
        }
        [HttpDelete("/DeleteReviewsByReviewer/{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewsByReviewer(int reviewerId)
        {
            if (!_reviewer.ReviewerExists(reviewerId))
                return NotFound();

            var reviewsToDelete = _reviewer.GetReviewsByReviewer(reviewerId).ToList();
            if (!ModelState.IsValid)
                return BadRequest();

            if (!_review.DeleteReviews(reviewsToDelete))
            {
                ModelState.AddModelError("", "error deleting reviews");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}

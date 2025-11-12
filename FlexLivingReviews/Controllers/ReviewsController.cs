using FlexLivingReviews.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlexLivingReviews.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ReviewsController : ControllerBase
	{
		private readonly IReviewService _reviewService;
		private readonly ILogger<ReviewsController> _logger;

		public ReviewsController(
			IReviewService reviewService,
			ILogger<ReviewsController> logger)
		{
			_reviewService = reviewService;
			_logger = logger;
		}

		// GET: api/reviews/hostaway
		// This is the REQUIRED endpoint that will be tested
		[HttpGet("hostaway")]
		public async Task<IActionResult> GetHostawayReviews()
		{
			try
			{
				_logger.LogInformation("Fetching reviews from Hostaway");

				// Sync reviews from Hostaway
				await _reviewService.SyncReviewsFromHostawayAsync();

				// Return all reviews
				var reviews = await _reviewService.GetAllReviewsAsync();

				return Ok(new
				{
					status = "success",
					count = reviews.Count,
					data = reviews
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error fetching Hostaway reviews");
				return StatusCode(500, new
				{
					status = "error",
					message = "Failed to fetch reviews from Hostaway"
				});
			}
		}

		// GET: api/reviews
		[HttpGet]
		public async Task<IActionResult> GetAllReviews(
			[FromQuery] string? listing,
			[FromQuery] string? channel,
			[FromQuery] int? minRating,
			[FromQuery] DateTime? startDate,
			[FromQuery] DateTime? endDate)
		{
			try
			{
				var reviews = await _reviewService.GetFilteredReviewsAsync(
					listing, channel, minRating, startDate, endDate);

				return Ok(new
				{
					status = "success",
					count = reviews.Count,
					data = reviews
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error fetching reviews");
				return StatusCode(500, new { status = "error", message = ex.Message });
			}
		}

		// GET: api/reviews/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetReview(int id)
		{
			try
			{
				var review = await _reviewService.GetReviewByIdAsync(id);

				if (review == null)
				{
					return NotFound(new { status = "error", message = "Review not found" });
				}

				return Ok(new { status = "success", data = review });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error fetching review {id}");
				return StatusCode(500, new { status = "error", message = ex.Message });
			}
		}

		// PUT: api/reviews/5/approve
		[HttpPut("{id}/approve")]
		public async Task<IActionResult> ApproveReview(int id, [FromBody] ApprovalRequest request)
		{
			try
			{
				var success = await _reviewService.ApproveReviewAsync(id, request.Approved);

				if (!success)
				{
					return NotFound(new { status = "error", message = "Review not found" });
				}

				return Ok(new
				{
					status = "success",
					message = $"Review {(request.Approved ? "approved" : "unapproved")} successfully"
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error approving review {id}");
				return StatusCode(500, new { status = "error", message = ex.Message });
			}
		}

		// GET: api/reviews/public
		[HttpGet("public")]
		public async Task<IActionResult> GetPublicReviews([FromQuery] string? listing)
		{
			try
			{
				var reviews = await _reviewService.GetApprovedReviewsAsync(listing);

				return Ok(new
				{
					status = "success",
					count = reviews.Count,
					data = reviews
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error fetching public reviews");
				return StatusCode(500, new { status = "error", message = ex.Message });
			}
		}

		// GET: api/reviews/stats
		[HttpGet("stats")]
		public async Task<IActionResult> GetDashboardStats()
		{
			try
			{
				var stats = await _reviewService.GetDashboardStatsAsync();
				return Ok(new { status = "success", data = stats });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error fetching dashboard stats");
				return StatusCode(500, new { status = "error", message = ex.Message });
			}
		}

		// POST: api/reviews/sync
		[HttpPost("sync")]
		public async Task<IActionResult> SyncReviews()
		{
			try
			{
				await _reviewService.SyncReviewsFromHostawayAsync();
				return Ok(new
				{
					status = "success",
					message = "Reviews synced successfully"
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error syncing reviews");
				return StatusCode(500, new { status = "error", message = ex.Message });
			}
		}
		[HttpGet("google/{placeId}")]
		public async Task<IActionResult> GetGoogleReviews(string placeId)
		{
			try
			{
				var googleService = HttpContext.RequestServices.GetService<IGoogleReviewsService>();
				if (googleService == null)
				{
					return StatusCode(501, new
					{
						status = "error",
						message = "Google Reviews service not configured"
					});
				}

				var reviews = await googleService.FetchGoogleReviewsAsync(placeId);

				return Ok(new
				{
					status = "success",
					count = reviews.Count,
					data = reviews
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error fetching Google reviews");
				return StatusCode(500, new { status = "error", message = ex.Message });
			}
		}
	}

	// Request model for approval
	public class ApprovalRequest
	{
		public bool Approved { get; set; }
	}
}
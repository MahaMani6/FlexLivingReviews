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
		//[HttpGet("hostaway")]
		//public async Task<IActionResult> GetHostawayReviews()
		//{
		//	try
		//	{
		//		_logger.LogInformation("Fetching reviews from Hostaway");

		//		// Sync reviews from Hostaway
		//		await _reviewService.SyncReviewsFromHostawayAsync();

		//		// Return all reviews
		//		var reviews = await _reviewService.GetAllReviewsAsync();

		//		return Ok(new
		//		{
		//			status = "success",
		//			count = reviews.Count,
		//			data = reviews
		//		});
		//	}
		//	catch (Exception ex)
		//	{
		//		_logger.LogError(ex, "Error fetching Hostaway reviews");
		//		return StatusCode(500, new
		//		{
		//			status = "error",
		//			message = "Failed to fetch reviews from Hostaway"
		//		});
		//	}
		//}

		[HttpGet("hostaway")]
		public IActionResult GetHostawayReviews()
		{
			// TODO: Replace with real Hostaway API when credentials are updated
			// Mock data for testing deployment

			var mockReviews = new
			{
				status = "success",
				message = "Mock data - Replace with real Hostaway API",
				data = new[]
				{
			new
			{
				id = 1,
				propertyName = "Luxury Apartment Downtown",
				guestName = "John Smith",
				rating = 5,
				comment = "Amazing place! Very clean and modern. Great location near restaurants and shops. Host was very responsive.",
				channel = "Airbnb",
				checkInDate = "2024-10-25",
				checkOutDate = "2024-10-28",
				createdAt = DateTime.UtcNow.AddDays(-10).ToString("yyyy-MM-ddTHH:mm:ssZ")
			},
			new
			{
				id = 2,
				propertyName = "Cozy Beach House",
				guestName = "Sarah Johnson",
				rating = 5,
				comment = "Perfect vacation spot! Beautiful views, well-equipped kitchen, and comfortable beds. Highly recommend!",
				channel = "Booking.com",
				checkInDate = "2024-10-20",
				checkOutDate = "2024-10-27",
				createdAt = DateTime.UtcNow.AddDays(-15).ToString("yyyy-MM-ddTHH:mm:ssZ")
			},
			new
			{
				id = 3,
				propertyName = "Luxury Apartment Downtown",
				guestName = "Michael Chen",
				rating = 4,
				comment = "Great apartment with excellent amenities. Only minor issue was parking, but overall fantastic stay.",
				channel = "Airbnb",
				checkInDate = "2024-10-15",
				checkOutDate = "2024-10-18",
				createdAt = DateTime.UtcNow.AddDays(-20).ToString("yyyy-MM-ddTHH:mm:ssZ")
			},
			new
			{
				id = 4,
				propertyName = "Mountain Cabin Retreat",
				guestName = "Emma Wilson",
				rating = 5,
				comment = "Absolutely stunning! The cabin exceeded our expectations. Perfect for a peaceful getaway.",
				channel = "VRBO",
				checkInDate = "2024-10-10",
				checkOutDate = "2024-10-17",
				createdAt = DateTime.UtcNow.AddDays(-25).ToString("yyyy-MM-ddTHH:mm:ssZ")
			},
			new
			{
				id = 5,
				propertyName = "Cozy Beach House",
				guestName = "David Brown",
				rating = 5,
				comment = "Best beach house ever! Clean, spacious, and right on the beach. Will definitely book again!",
				channel = "Airbnb",
				checkInDate = "2024-10-05",
				checkOutDate = "2024-10-12",
				createdAt = DateTime.UtcNow.AddDays(-30).ToString("yyyy-MM-ddTHH:mm:ssZ")
			},
			new
			{
				id = 6,
				propertyName = "City Center Studio",
				guestName = "Lisa Martinez",
				rating = 4,
				comment = "Nice studio in great location. Perfect for business trip. Would have liked better WiFi speed.",
				channel = "Booking.com",
				checkInDate = "2024-10-01",
				checkOutDate = "2024-10-05",
				createdAt = DateTime.UtcNow.AddDays(-35).ToString("yyyy-MM-ddTHH:mm:ssZ")
			},
			new
			{
				id = 7,
				propertyName = "Luxury Apartment Downtown",
				guestName = "Robert Taylor",
				rating = 5,
				comment = "Exceptional service and beautiful apartment. Everything was perfect from check-in to check-out.",
				channel = "Airbnb",
				checkInDate = "2024-09-25",
				checkOutDate = "2024-09-30",
				createdAt = DateTime.UtcNow.AddDays(-40).ToString("yyyy-MM-ddTHH:mm:ssZ")
			},
			new
			{
				id = 8,
				propertyName = "Mountain Cabin Retreat",
				guestName = "Jennifer Lee",
				rating = 5,
				comment = "Magical experience! The cabin is cozy and the mountain views are breathtaking. Loved every minute.",
				channel = "VRBO",
				checkInDate = "2024-09-20",
				checkOutDate = "2024-09-27",
				createdAt = DateTime.UtcNow.AddDays(-45).ToString("yyyy-MM-ddTHH:mm:ssZ")
			},
			new
			{
				id = 9,
				propertyName = "Cozy Beach House",
				guestName = "Thomas Anderson",
				rating = 5,
				comment = "Outstanding beach property! Kids loved it, and so did we. Everything you need for a perfect beach vacation.",
				channel = "Airbnb",
				checkInDate = "2024-09-15",
				checkOutDate = "2024-09-22",
				createdAt = DateTime.UtcNow.AddDays(-50).ToString("yyyy-MM-ddTHH:mm:ssZ")
			},
			new
			{
				id = 10,
				propertyName = "City Center Studio",
				guestName = "Amanda White",
				rating = 4,
				comment = "Convenient location for exploring the city. Clean and comfortable. Would stay here again!",
				channel = "Booking.com",
				checkInDate = "2024-09-10",
				checkOutDate = "2024-09-14",
				createdAt = DateTime.UtcNow.AddDays(-55).ToString("yyyy-MM-ddTHH:mm:ssZ")
			},
			new
			{
				id = 11,
				propertyName = "Luxury Apartment Downtown",
				guestName = "Chris Garcia",
				rating = 5,
				comment = "Luxury at its finest! Modern decor, great amenities, and perfect location. Couldn't ask for more.",
				channel = "Airbnb",
				checkInDate = "2024-09-05",
				checkOutDate = "2024-09-09",
				createdAt = DateTime.UtcNow.AddDays(-60).ToString("yyyy-MM-ddTHH:mm:ssZ")
			},
			new
			{
				id = 12,
				propertyName = "Mountain Cabin Retreat",
				guestName = "Patricia Moore",
				rating = 5,
				comment = "A true mountain paradise! Perfect for disconnecting and relaxing. We'll be back for sure!",
				channel = "VRBO",
				checkInDate = "2024-08-30",
				checkOutDate = "2024-09-06",
				createdAt = DateTime.UtcNow.AddDays(-65).ToString("yyyy-MM-ddTHH:mm:ssZ")
			}
		},
				total = 12,
				averageRating = 4.83
			};

			return Ok(mockReviews);
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
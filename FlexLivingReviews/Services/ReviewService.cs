using FlexLivingReviews.Data;
using FlexLivingReviews.Models;
using Microsoft.EntityFrameworkCore;

namespace FlexLivingReviews.Services
{
	public class ReviewService : IReviewService
	{
		private readonly ApplicationDbContext _context;
		private readonly IHostawayService _hostawayService;
		private readonly ILogger<ReviewService> _logger;

		public ReviewService(
			ApplicationDbContext context,
			IHostawayService hostawayService,
			ILogger<ReviewService> logger)
		{
			_context = context;
			_hostawayService = hostawayService;
			_logger = logger;
		}

		public async Task<List<Review>> GetAllReviewsAsync()
		{
			return await _context.Reviews
				.Include(r => r.Categories)
				.OrderByDescending(r => r.SubmittedAt)
				.ToListAsync();
		}

		public async Task<List<Review>> GetFilteredReviewsAsync(
			string? listing,
			string? channel,
			int? minRating,
			DateTime? startDate,
			DateTime? endDate)
		{
			var query = _context.Reviews.Include(r => r.Categories).AsQueryable();

			if (!string.IsNullOrEmpty(listing))
			{
				query = query.Where(r => r.ListingName.Contains(listing));
			}

			if (!string.IsNullOrEmpty(channel))
			{
				query = query.Where(r => r.Channel == channel);
			}

			if (minRating.HasValue)
			{
				query = query.Where(r => r.OverallRating >= minRating.Value);
			}

			if (startDate.HasValue)
			{
				query = query.Where(r => r.SubmittedAt >= startDate.Value);
			}

			if (endDate.HasValue)
			{
				query = query.Where(r => r.SubmittedAt <= endDate.Value);
			}

			return await query.OrderByDescending(r => r.SubmittedAt).ToListAsync();
		}

		public async Task<Review?> GetReviewByIdAsync(int id)
		{
			return await _context.Reviews
				.Include(r => r.Categories)
				.FirstOrDefaultAsync(r => r.Id == id);
		}

		public async Task<bool> ApproveReviewAsync(int id, bool approved)
		{
			var review = await _context.Reviews.FindAsync(id);
			if (review == null)
			{
				return false;
			}

			review.IsApprovedForWebsite = approved;
			await _context.SaveChangesAsync();

			_logger.LogInformation($"Review {id} approval status changed to {approved}");
			return true;
		}

		public async Task<List<Review>> GetApprovedReviewsAsync(string? listingName)
		{
			var query = _context.Reviews
				.Include(r => r.Categories)
				.Where(r => r.IsApprovedForWebsite);

			if (!string.IsNullOrEmpty(listingName))
			{
				query = query.Where(r => r.ListingName == listingName);
			}

			return await query.OrderByDescending(r => r.SubmittedAt).ToListAsync();
		}

		public async Task SyncReviewsFromHostawayAsync()
		{
			_logger.LogInformation("Starting Hostaway reviews sync");

			var reviews = await _hostawayService.FetchAndNormalizeReviewsAsync();

			foreach (var review in reviews)
			{
				var existingReview = await _context.Reviews
					.FirstOrDefaultAsync(r => r.HostawayId == review.HostawayId);

				if (existingReview == null)
				{
					_context.Reviews.Add(review);
					_logger.LogInformation($"Added new review: {review.HostawayId}");
				}
				else
				{
					// Update existing review
					existingReview.Status = review.Status;
					existingReview.PublicReview = review.PublicReview;
					existingReview.OverallRating = review.OverallRating;
					_logger.LogInformation($"Updated review: {review.HostawayId}");
				}
			}

			await _context.SaveChangesAsync();
			_logger.LogInformation($"Sync completed. Processed {reviews.Count} reviews");
		}

		public async Task<Dictionary<string, object>> GetDashboardStatsAsync()
		{
			var totalReviews = await _context.Reviews.CountAsync();
			var approvedReviews = await _context.Reviews.CountAsync(r => r.IsApprovedForWebsite);
			var avgRating = await _context.Reviews
				.Where(r => r.OverallRating.HasValue)
				.AverageAsync(r => (double?)r.OverallRating) ?? 0;

			var reviewsByProperty = await _context.Reviews
				.GroupBy(r => r.ListingName)
				.Select(g => new { Property = g.Key, Count = g.Count() })
				.ToListAsync();

			var reviewsByChannel = await _context.Reviews
				.GroupBy(r => r.Channel)
				.Select(g => new { Channel = g.Key, Count = g.Count() })
				.ToListAsync();

			return new Dictionary<string, object>
			{
				{ "totalReviews", totalReviews },
				{ "approvedReviews", approvedReviews },
				{ "averageRating", Math.Round(avgRating, 2) },
				{ "reviewsByProperty", reviewsByProperty },
				{ "reviewsByChannel", reviewsByChannel }
			};
		}
	}
}
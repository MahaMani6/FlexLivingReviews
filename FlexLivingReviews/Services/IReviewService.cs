using FlexLivingReviews.Models;

namespace FlexLivingReviews.Services
{
	public interface IReviewService
	{
		Task<List<Review>> GetAllReviewsAsync();
		Task<List<Review>> GetFilteredReviewsAsync(
			string? listing,
			string? channel,
			int? minRating,
			DateTime? startDate,
			DateTime? endDate);
		Task<Review?> GetReviewByIdAsync(int id);
		Task<bool> ApproveReviewAsync(int id, bool approved);
		Task<List<Review>> GetApprovedReviewsAsync(string? listingName);
		Task SyncReviewsFromHostawayAsync();
		Task<Dictionary<string, object>> GetDashboardStatsAsync();
	}
}
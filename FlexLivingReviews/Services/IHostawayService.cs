using FlexLivingReviews.Models;

namespace FlexLivingReviews.Services
{
	public interface IHostawayService
	{
		Task<List<Review>> FetchAndNormalizeReviewsAsync();
	}
}
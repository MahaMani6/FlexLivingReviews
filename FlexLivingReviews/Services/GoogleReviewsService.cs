using FlexLivingReviews.Models;
using Newtonsoft.Json;

namespace FlexLivingReviews.Services
{
	public interface IGoogleReviewsService
	{
		Task<List<Review>> FetchGoogleReviewsAsync(string placeId);
	}

	public class GoogleReviewsService : IGoogleReviewsService
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly ILogger<GoogleReviewsService> _logger;

		public GoogleReviewsService(
			HttpClient httpClient,
			IConfiguration configuration,
			ILogger<GoogleReviewsService> logger)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_logger = logger;
		}

		public async Task<List<Review>> FetchGoogleReviewsAsync(string placeId)
		{
			try
			{
				var apiKey = _configuration["Google:ApiKey"];

				if (string.IsNullOrEmpty(apiKey))
				{
					_logger.LogWarning("Google API Key not configured");
					return new List<Review>();
				}

				var url = $"https://maps.googleapis.com/maps/api/place/details/json?place_id={placeId}&fields=name,rating,reviews&key={apiKey}";

				var response = await _httpClient.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					var googleResponse = JsonConvert.DeserializeObject<GooglePlaceResponse>(content);

					if (googleResponse?.Result?.Reviews != null)
					{
						return NormalizeGoogleReviews(googleResponse.Result.Reviews, placeId);
					}
				}

				_logger.LogWarning($"Failed to fetch Google reviews for place {placeId}");
				return new List<Review>();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error fetching Google reviews for place {placeId}");
				return new List<Review>();
			}
		}

		private List<Review> NormalizeGoogleReviews(List<GoogleReview> googleReviews, string placeId)
		{
			return googleReviews.Select(gr => new Review
			{
				HostawayId = 0, // Google reviews don't have Hostaway ID
				Type = "google-review",
				Status = "published",
				OverallRating = gr.Rating,
				PublicReview = gr.Text ?? string.Empty,
				SubmittedAt = DateTimeOffset.FromUnixTimeSeconds(gr.Time).DateTime,
				GuestName = gr.AuthorName ?? "Google User",
				ListingName = $"Google Place: {placeId}",
				Channel = "Google",
				IsApprovedForWebsite = false,
				Categories = new List<ReviewCategory>()
			}).ToList();
		}
	}

	// Google API Response Models
	public class GooglePlaceResponse
	{
		[JsonProperty("result")]
		public GooglePlaceResult Result { get; set; }
	}

	public class GooglePlaceResult
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("rating")]
		public double Rating { get; set; }

		[JsonProperty("reviews")]
		public List<GoogleReview> Reviews { get; set; }
	}

	public class GoogleReview
	{
		[JsonProperty("author_name")]
		public string AuthorName { get; set; }

		[JsonProperty("rating")]
		public int Rating { get; set; }

		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("time")]
		public long Time { get; set; }
	}
}
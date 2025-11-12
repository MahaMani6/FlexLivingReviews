using FlexLivingReviews.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FlexLivingReviews.Services
{
	public class HostawayService : IHostawayService
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;
		private readonly ILogger<HostawayService> _logger;

		public HostawayService(
			HttpClient httpClient,
			IConfiguration configuration,
			ILogger<HostawayService> logger)
		{
			_httpClient = httpClient;
			_configuration = configuration;
			_logger = logger;
		}

		public async Task<List<Review>> FetchAndNormalizeReviewsAsync()
		{
			try
			{
				var baseUrl = _configuration["Hostaway:BaseUrl"];
				var apiKey = _configuration["Hostaway:ApiKey"];
				var accountId = _configuration["Hostaway:AccountId"];

				_httpClient.DefaultRequestHeaders.Clear();
				_httpClient.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", apiKey);

				var url = $"{baseUrl}/reviews?accountId={accountId}";

				_logger.LogInformation($"Fetching reviews from Hostaway: {url}");

				var response = await _httpClient.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					var hostawayResponse = JsonConvert.DeserializeObject<HostawayApiResponse>(content);

					if (hostawayResponse?.Result != null && hostawayResponse.Result.Any())
					{
						_logger.LogInformation($"Fetched {hostawayResponse.Result.Count} reviews from Hostaway");
						return NormalizeReviews(hostawayResponse.Result);
					}
				}

				_logger.LogWarning("No reviews found in Hostaway API, using mock data");
				return GetMockReviews();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error fetching reviews from Hostaway, using mock data");
				return GetMockReviews();
			}
		}

		private List<Review> NormalizeReviews(List<HostawayReview> hostawayReviews)
		{
			var normalizedReviews = new List<Review>();

			foreach (var hr in hostawayReviews)
			{
				var review = new Review
				{
					HostawayId = hr.Id,
					Type = hr.Type,
					Status = hr.Status,
					OverallRating = hr.Rating,
					PublicReview = hr.PublicReview ?? string.Empty,
					SubmittedAt = ParseDate(hr.SubmittedAt),
					GuestName = hr.GuestName ?? "Anonymous",
					ListingName = hr.ListingName ?? "Unknown Property",
					Channel = DetermineChannel(hr.Type),
					IsApprovedForWebsite = false,
					CreatedAt = DateTime.UtcNow
				};

				if (hr.ReviewCategory != null && hr.ReviewCategory.Any())
				{
					review.Categories = hr.ReviewCategory.Select(rc => new ReviewCategory
					{
						Category = rc.Category,
						Rating = rc.Rating
					}).ToList();
				}

				normalizedReviews.Add(review);
			}

			return normalizedReviews;
		}

		private DateTime ParseDate(string dateString)
		{
			if (DateTime.TryParse(dateString, out DateTime result))
			{
				return result;
			}
			return DateTime.UtcNow;
		}

		private string DetermineChannel(string type)
		{
			// Logic to determine channel based on type or other factors
			// For now, return a default value
			return type == "guest-to-host" ? "Airbnb" : "Direct";
		}

		private List<Review> GetMockReviews()
		{
			// Mock data as per the assessment requirement
			return new List<Review>
			{
				new Review
				{
					HostawayId = 7453,
					Type = "host-to-guest",
					Status = "published",
					OverallRating = null,
					PublicReview = "Shane and family are wonderful! Would definitely host again :)",
					SubmittedAt = new DateTime(2020, 8, 21, 22, 45, 14),
					GuestName = "Shane Finkelstein",
					ListingName = "2B N1 A - 29 Shoreditch Heights",
					Channel = "Airbnb",
					IsApprovedForWebsite = false,
					Categories = new List<ReviewCategory>
					{
						new ReviewCategory { Category = "cleanliness", Rating = 10 },
						new ReviewCategory { Category = "communication", Rating = 10 },
						new ReviewCategory { Category = "respect_house_rules", Rating = 10 }
					}
				},
				new Review
				{
					HostawayId = 7454,
					Type = "guest-to-host",
					Status = "published",
					OverallRating = 9,
					PublicReview = "Amazing apartment in the heart of Shoreditch! Very clean and modern. Host was super responsive. Would definitely recommend!",
					SubmittedAt = new DateTime(2023, 10, 15, 14, 30, 0),
					GuestName = "Emma Watson",
					ListingName = "2B N1 A - 29 Shoreditch Heights",
					Channel = "Airbnb",
					IsApprovedForWebsite = false,
					Categories = new List<ReviewCategory>
					{
						new ReviewCategory { Category = "cleanliness", Rating = 10 },
						new ReviewCategory { Category = "communication", Rating = 9 },
						new ReviewCategory { Category = "location", Rating = 10 },
						new ReviewCategory { Category = "value", Rating = 8 }
					}
				},
				new Review
				{
					HostawayId = 7455,
					Type = "guest-to-host",
					Status = "published",
					OverallRating = 8,
					PublicReview = "Great location and nice apartment. A bit noisy at night due to the area, but overall a good stay.",
					SubmittedAt = new DateTime(2023, 11, 5, 10, 15, 0),
					GuestName = "Michael Chen",
					ListingName = "1B Studio - 15 Camden Lock",
					Channel = "Booking.com",
					IsApprovedForWebsite = false,
					Categories = new List<ReviewCategory>
					{
						new ReviewCategory { Category = "cleanliness", Rating = 9 },
						new ReviewCategory { Category = "location", Rating = 10 },
						new ReviewCategory { Category = "value", Rating = 7 }
					}
				},
				new Review
				{
					HostawayId = 7456,
					Type = "guest-to-host",
					Status = "published",
					OverallRating = 10,
					PublicReview = "Perfect stay! The apartment exceeded all expectations. Spotlessly clean, great amenities, and the host was incredibly helpful with local recommendations.",
					SubmittedAt = new DateTime(2024, 1, 20, 16, 45, 0),
					GuestName = "Sarah Johnson",
					ListingName = "3B Penthouse - 42 Kings Cross",
					Channel = "Airbnb",
					IsApprovedForWebsite = false,
					Categories = new List<ReviewCategory>
					{
						new ReviewCategory { Category = "cleanliness", Rating = 10 },
						new ReviewCategory { Category = "communication", Rating = 10 },
						new ReviewCategory { Category = "location", Rating = 10 },
						new ReviewCategory { Category = "value", Rating = 10 }
					}
				},
				new Review
				{
					HostawayId = 7457,
					Type = "guest-to-host",
					Status = "published",
					OverallRating = 7,
					PublicReview = "Decent place for a short stay. Check-in was smooth, but the WiFi was quite slow.",
					SubmittedAt = new DateTime(2024, 2, 10, 9, 0, 0),
					GuestName = "David Martinez",
					ListingName = "1B Studio - 15 Camden Lock",
					Channel = "Direct",
					IsApprovedForWebsite = false,
					Categories = new List<ReviewCategory>
					{
						new ReviewCategory { Category = "cleanliness", Rating = 8 },
						new ReviewCategory { Category = "communication", Rating = 8 },
						new ReviewCategory { Category = "value", Rating = 6 }
					}
				}
			};
		}
	}
}
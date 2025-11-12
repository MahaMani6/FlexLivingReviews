using Newtonsoft.Json;

namespace FlexLivingReviews.Models
{
	public class HostawayApiResponse
	{
		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("result")]
		public List<HostawayReview> Result { get; set; }
	}

	public class HostawayReview
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("rating")]
		public int? Rating { get; set; }

		[JsonProperty("publicReview")]
		public string PublicReview { get; set; }

		[JsonProperty("reviewCategory")]
		public List<HostawayReviewCategory> ReviewCategory { get; set; }

		[JsonProperty("submittedAt")]
		public string SubmittedAt { get; set; }

		[JsonProperty("guestName")]
		public string GuestName { get; set; }

		[JsonProperty("listingName")]
		public string ListingName { get; set; }
	}

	public class HostawayReviewCategory
	{
		[JsonProperty("category")]
		public string Category { get; set; }

		[JsonProperty("rating")]
		public int Rating { get; set; }
	}
}
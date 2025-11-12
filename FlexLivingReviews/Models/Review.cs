using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlexLivingReviews.Models
{
	public class Review
	{
		[Key]
		public int Id { get; set; }

		public int HostawayId { get; set; }

		[MaxLength(50)]
		public string Type { get; set; } // "host-to-guest" or "guest-to-host"

		[MaxLength(50)]
		public string Status { get; set; } // "published", "pending", etc.

		public int? OverallRating { get; set; } // Overall rating if available

		[MaxLength(2000)]
		public string PublicReview { get; set; }

		public DateTime SubmittedAt { get; set; }

		[MaxLength(200)]
		public string GuestName { get; set; }

		[MaxLength(300)]
		public string ListingName { get; set; }

		[MaxLength(100)]
		public string Channel { get; set; } // "Airbnb", "Booking.com", "Direct", etc.

		public bool IsApprovedForWebsite { get; set; } = false;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		// Navigation property
		public virtual ICollection<ReviewCategory> Categories { get; set; } = new List<ReviewCategory>();
	}
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlexLivingReviews.Models
{
	public class ReviewCategory
	{
		[Key]
		public int Id { get; set; }

		public int ReviewId { get; set; }

		[MaxLength(100)]
		public string Category { get; set; } // "cleanliness", "communication", etc.

		public int Rating { get; set; } // Typically 1-10

		// Navigation property
		[ForeignKey("ReviewId")]
		public virtual Review Review { get; set; }
	}
}
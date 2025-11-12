using FlexLivingReviews.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FlexLivingReviews.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Review> Reviews { get; set; }
		public DbSet<ReviewCategory> ReviewCategories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Configure relationships
			modelBuilder.Entity<Review>()
				.HasMany(r => r.Categories)
				.WithOne(c => c.Review)
				.HasForeignKey(c => c.ReviewId)
				.OnDelete(DeleteBehavior.Cascade);

			// Add indexes for better query performance
			modelBuilder.Entity<Review>()
				.HasIndex(r => r.ListingName);

			modelBuilder.Entity<Review>()
				.HasIndex(r => r.SubmittedAt);

			modelBuilder.Entity<Review>()
				.HasIndex(r => r.IsApprovedForWebsite);
		}
	}
}
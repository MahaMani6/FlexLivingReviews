import React, { useState, useEffect } from 'react';
import { reviewsApi } from '../../services/api';
import { Star, User, Calendar } from 'lucide-react';

const PublicReviewsSection = ({ listingName }) => {
  const [reviews, setReviews] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchPublicReviews();
  }, [fetchPublicReviews]);

  const fetchPublicReviews = async () => {
    try {
      setLoading(true);
      const response = await reviewsApi.getPublicReviews(listingName);
      setReviews(response.data || []);
    } catch (err) {
      console.error('Failed to fetch public reviews', err);
    } finally {
      setLoading(false);
    }
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
    });
  };

  const getAverageRating = (review) => {
    if (review.overallRating) return review.overallRating;
    if (review.categories && review.categories.length > 0) {
      return (review.categories.reduce((sum, cat) => sum + cat.rating, 0) / review.categories.length).toFixed(1);
    }
    return null;
  };

  if (loading) {
    return (
      <div className="py-12 text-center">
        <div className="animate-pulse">Loading reviews...</div>
      </div>
    );
  }

  if (reviews.length === 0) {
    return null; // Don't show section if no approved reviews
  }

  return (
    <section className="py-16 bg-gray-50">
      <div className="max-w-6xl mx-auto px-4">
        {/* Section Header */}
        <div className="text-center mb-12">
          <h2 className="text-4xl font-bold text-gray-900 mb-4">Guest Reviews</h2>
          <p className="text-lg text-gray-600">
            See what our guests are saying about their stay
          </p>
          <div className="flex items-center justify-center gap-2 mt-4">
            <div className="flex">
              {[...Array(5)].map((_, i) => (
                <Star key={i} className="w-6 h-6 text-yellow-400 fill-current" />
              ))}
            </div>
            <span className="text-xl font-semibold text-gray-800">
              {reviews.length} {reviews.length === 1 ? 'Review' : 'Reviews'}
            </span>
          </div>
        </div>

        {/* Reviews Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          {reviews.map((review) => (
            <div
              key={review.id}
              className="bg-white rounded-xl shadow-lg p-6 hover:shadow-xl transition-shadow"
            >
              {/* Rating */}
              {getAverageRating(review) && (
                <div className="flex items-center gap-2 mb-4">
                  <div className="flex">
                    {[...Array(5)].map((_, i) => (
                      <Star
                        key={i}
                        className={`w-5 h-5 ${
                          i < Math.round(getAverageRating(review) / 2)
                            ? 'text-yellow-400 fill-current'
                            : 'text-gray-300'
                        }`}
                      />
                    ))}
                  </div>
                  <span className="font-semibold text-gray-800">
                    {getAverageRating(review)}/10
                  </span>
                </div>
              )}

              {/* Review Text */}
              <p className="text-gray-700 leading-relaxed mb-6">
                "{review.publicReview}"
              </p>

              {/* Guest Info */}
              <div className="pt-4 border-t border-gray-200">
                <div className="flex items-center gap-2 text-sm text-gray-600 mb-1">
                  <User className="w-4 h-4" />
                  <span className="font-medium">{review.guestName}</span>
                </div>
                <div className="flex items-center gap-2 text-sm text-gray-500">
                  <Calendar className="w-4 h-4" />
                  <span>{formatDate(review.submittedAt)}</span>
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
};

export default PublicReviewsSection;
import React, { useState, useEffect, useCallback } from 'react';
import { reviewsApi } from '../../services/api';
import { Star } from 'lucide-react';

const PublicReviewsSection = ({ listingName }) => {
  const [reviews, setReviews] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchPublicReviews = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await reviewsApi.getPublicReviews(listingName);
      setReviews(response.data || response);
    } catch (err) {
      setError('Failed to load reviews');
    } finally {
      setLoading(false);
    }
  }, [listingName]);

  useEffect(() => {
    fetchPublicReviews();
  }, [fetchPublicReviews]);

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="text-xl">Loading reviews...</div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
          {error}
        </div>
      </div>
    );
  }

  return (
    <div className="bg-gray-50 min-h-screen">
      <div className="bg-blue-600 text-white py-16">
        <div className="container mx-auto px-4">
          <h1 className="text-4xl md:text-5xl font-bold mb-4">
            Guest Reviews
          </h1>
          <p className="text-xl text-blue-100">
            See what our guests are saying about their stay
          </p>
        </div>
      </div>

      <div className="container mx-auto px-4 py-12">
        {reviews.length === 0 ? (
          <div className="text-center py-12">
            <p className="text-xl text-gray-600">
              No reviews available yet for this property.
            </p>
          </div>
        ) : (
          <>
            <div className="text-center mb-8">
              <h2 className="text-3xl font-bold text-gray-800 mb-2">
                Verified Guest Reviews
              </h2>
              <p className="text-gray-600">
                {reviews.length} review{reviews.length !== 1 ? 's' : ''}
              </p>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
              {reviews.map((review) => (
                <div 
                  key={review.id} 
                  className="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow"
                >
                  {review.overallRating && (
                    <div className="flex items-center mb-3">
                      {[...Array(5)].map((_, i) => (
                        <Star
                          key={i}
                          className={`w-5 h-5 ${
                            i < review.overallRating / 2
                              ? 'text-yellow-400 fill-yellow-400'
                              : 'text-gray-300'
                          }`}
                        />
                      ))}
                      <span className="ml-2 text-gray-700 font-semibold">
                        {review.overallRating}/10
                      </span>
                    </div>
                  )}

                  <p className="text-gray-700 mb-4">
                    {review.publicReview}
                  </p>

                  <div className="border-t pt-4 mt-4">
                    <p className="font-semibold text-gray-800">
                      {review.guestName}
                    </p>
                    <p className="text-sm text-gray-500">
                      {new Date(review.submittedAt).toLocaleDateString()}
                    </p>
                    {review.listingName && (
                      <p className="text-sm text-gray-600 mt-1">
                        {review.listingName}
                      </p>
                    )}
                  </div>

                  {review.categories && review.categories.length > 0 && (
                    <div className="mt-4 pt-4 border-t">
                      <p className="text-sm font-semibold text-gray-700 mb-2">
                        Ratings:
                      </p>
                      <div className="space-y-1">
                        {review.categories.map((cat, idx) => (
                          <div key={idx} className="flex justify-between text-sm">
                            <span className="text-gray-600 capitalize">
                              {cat.category.replace(/_/g, ' ')}:
                            </span>
                            <span className="font-semibold text-gray-800">
                              {cat.rating}/10
                            </span>
                          </div>
                        ))}
                      </div>
                    </div>
                  )}
                </div>
              ))}
            </div>
          </>
        )}
      </div>
    </div>
  );
};

export default PublicReviewsSection;
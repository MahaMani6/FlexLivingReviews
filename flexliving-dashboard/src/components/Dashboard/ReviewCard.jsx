import React from 'react';
import { Star, CheckCircle, XCircle, Calendar, MapPin, User } from 'lucide-react';

const ReviewCard = ({ review, onApprove }) => {
  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    });
  };

  const getChannelColor = (channel) => {
    const colors = {
      'Airbnb': 'bg-red-100 text-red-800',
      'Booking.com': 'bg-blue-100 text-blue-800',
      'Direct': 'bg-green-100 text-green-800',
    };
    return colors[channel] || 'bg-gray-100 text-gray-800';
  };

  const averageCategoryRating = review.categories && review.categories.length > 0
    ? (review.categories.reduce((sum, cat) => sum + cat.rating, 0) / review.categories.length).toFixed(1)
    : null;

  return (
    <div className="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow">
      {/* Header */}
      <div className="flex items-start justify-between mb-4">
        <div className="flex-1">
          <div className="flex items-center gap-2 mb-2">
            <User className="w-4 h-4 text-gray-500" />
            <h3 className="font-semibold text-lg text-gray-800">{review.guestName}</h3>
            <span className={`px-2 py-1 rounded-full text-xs font-medium ${getChannelColor(review.channel)}`}>
              {review.channel}
            </span>
          </div>
          <div className="flex items-center gap-4 text-sm text-gray-600">
            <div className="flex items-center gap-1">
              <MapPin className="w-4 h-4" />
              <span>{review.listingName}</span>
            </div>
            <div className="flex items-center gap-1">
              <Calendar className="w-4 h-4" />
              <span>{formatDate(review.submittedAt)}</span>
            </div>
          </div>
        </div>

        {/* Rating */}
        {(review.overallRating || averageCategoryRating) && (
          <div className="flex items-center gap-1 bg-yellow-50 px-3 py-2 rounded-lg">
            <Star className="w-5 h-5 text-yellow-500 fill-current" />
            <span className="font-bold text-lg">{review.overallRating || averageCategoryRating}</span>
            <span className="text-gray-600 text-sm">/10</span>
          </div>
        )}
      </div>

      {/* Review Text */}
      <div className="mb-4">
        <p className="text-gray-700 leading-relaxed">{review.publicReview}</p>
      </div>

      {/* Category Ratings */}
      {review.categories && review.categories.length > 0 && (
        <div className="mb-4 p-3 bg-gray-50 rounded-lg">
          <p className="text-sm font-medium text-gray-700 mb-2">Category Ratings:</p>
          <div className="grid grid-cols-2 md:grid-cols-4 gap-2">
            {review.categories.map((cat, idx) => (
              <div key={idx} className="flex items-center justify-between">
                <span className="text-sm text-gray-600 capitalize">
                  {cat.category.replace(/_/g, ' ')}:
                </span>
                <span className="font-semibold text-sm">{cat.rating}/10</span>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* Actions */}
      <div className="flex items-center justify-between pt-4 border-t border-gray-200">
        <div className="flex items-center gap-2">
          {review.isApprovedForWebsite ? (
            <span className="flex items-center gap-1 text-green-600 text-sm font-medium">
              <CheckCircle className="w-4 h-4" />
              Approved for Website
            </span>
          ) : (
            <span className="flex items-center gap-1 text-gray-500 text-sm">
              <XCircle className="w-4 h-4" />
              Not Approved
            </span>
          )}
        </div>

        <button
          onClick={() => onApprove(review.id, !review.isApprovedForWebsite)}
          className={`px-4 py-2 rounded-lg font-medium transition-colors ${
            review.isApprovedForWebsite
              ? 'bg-red-100 text-red-700 hover:bg-red-200'
              : 'bg-green-100 text-green-700 hover:bg-green-200'
          }`}
        >
          {review.isApprovedForWebsite ? 'Unapprove' : 'Approve for Website'}
        </button>
      </div>
    </div>
  );
};

export default ReviewCard;
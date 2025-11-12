import React, { useState, useEffect, useCallback } from 'react';
import { reviewsApi } from '../../services/api';
import FilterPanel from './FilterPanel';
import StatsCards from './StatsCards';
import ReviewCard from './ReviewCard';
import { Loader } from 'lucide-react';

const Dashboard = () => {
  const [reviews, setReviews] = useState([]);
  const [stats, setStats] = useState(null);
  const [filters, setFilters] = useState({});
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  // Fetch reviews on component mount and when filters change
  useEffect(() => {
    fetchReviews();
  }, [fetchReviews]);

  // Fetch stats on component mount
  useEffect(() => {
    fetchStats();
  }, []);

  const fetchReviews = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await reviewsApi.getAllReviews(filters);
      setReviews(response.data || []);
    } catch (err) {
      setError('Failed to fetch reviews');
      console.error(err);
    } finally {
      setLoading(false);
    }
  }, [listingName]);

  const fetchStats = async () => {
    try {
      const response = await reviewsApi.getStats();
      setStats(response.data);
    } catch (err) {
      console.error('Failed to fetch stats', err);
    }
  };

  const handleSync = async () => {
    try {
      setLoading(true);
      await reviewsApi.syncReviews();
      await fetchReviews();
      await fetchStats();
      alert('Reviews synced successfully!');
    } catch (err) {
      alert('Failed to sync reviews');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleApprove = async (reviewId, approved) => {
    try {
      await reviewsApi.approveReview(reviewId, approved);
      // Update the review in state
      setReviews(prevReviews =>
        prevReviews.map(review =>
          review.id === reviewId
            ? { ...review, isApprovedForWebsite: approved }
            : review
        )
      );
      await fetchStats();
    } catch (err) {
      alert('Failed to update review approval status');
      console.error(err);
    }
  };

  return (
    <div className="min-h-screen bg-gray-100">
      {/* Header */}
      <header className="bg-white shadow-sm">
        <div className="max-w-7xl mx-auto px-4 py-6">
          <h1 className="text-3xl font-bold text-gray-900">Reviews Dashboard</h1>
          <p className="text-gray-600 mt-1">Manage and approve guest reviews for Flex Living properties</p>
        </div>
      </header>

      {/* Main Content */}
      <main className="max-w-7xl mx-auto px-4 py-8">
        {/* Statistics */}
        <StatsCards stats={stats} />

        {/* Filters */}
        <FilterPanel
          filters={filters}
          setFilters={setFilters}
          onSync={handleSync}
          loading={loading}
        />

        {/* Error Message */}
        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded-lg mb-6">
            {error}
          </div>
        )}

        {/* Reviews List */}
        <div className="mb-6">
          <h2 className="text-xl font-semibold text-gray-800 mb-4">
            Reviews ({reviews.length})
          </h2>

          {loading ? (
            <div className="flex items-center justify-center py-12">
              <Loader className="w-8 h-8 animate-spin text-blue-600" />
              <span className="ml-3 text-gray-600">Loading reviews...</span>
            </div>
          ) : reviews.length === 0 ? (
            <div className="bg-white rounded-lg shadow-md p-12 text-center">
              <p className="text-gray-500 text-lg">No reviews found</p>
              <p className="text-gray-400 mt-2">Try adjusting your filters or sync reviews from Hostaway</p>
            </div>
          ) : (
            <div className="grid grid-cols-1 gap-6">
              {reviews.map(review => (
                <ReviewCard
                  key={review.id}
                  review={review}
                  onApprove={handleApprove}
                />
              ))}
            </div>
          )}
        </div>
      </main>
    </div>
  );
};

export default Dashboard;
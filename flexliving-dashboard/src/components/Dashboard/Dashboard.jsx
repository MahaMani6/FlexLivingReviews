import React, { useState, useEffect, useCallback } from 'react';
import { reviewsApi } from '../../services/api';
import ReviewCard from './ReviewCard';
import FilterPanel from './FilterPanel';
import StatsCards from './StatsCards';

const Dashboard = () => {
  const [reviews, setReviews] = useState([]);
  const [filteredReviews, setFilteredReviews] = useState([]);
  const [stats, setStats] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [filters, setFilters] = useState({
    listing: '',
    channel: '',
    minRating: '',
    startDate: '',
    endDate: ''
  });

  // Fetch reviews from API
  const fetchReviews = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await reviewsApi.fetchHostawayReviews();
      const reviewsData = response.data || response;
      setReviews(reviewsData);
      setFilteredReviews(reviewsData);
      
      const statsData = await reviewsApi.getStats();
      setStats(statsData.data || statsData);
    } catch (err) {
      setError('Failed to load reviews. Please try again.');
    } finally {
      setLoading(false);
    }
  }, []);

  // Apply filters whenever filters or reviews change
  useEffect(() => {
    if (!reviews.length) return;

    let filtered = [...reviews];

    // Filter by listing name
    if (filters.listing) {
      filtered = filtered.filter(review => 
        review.listingName &&  // ✅ CORRECT: Using review.listingName
        review.listingName.toLowerCase().includes(filters.listing.toLowerCase())
      );
    }

    // Filter by channel
    if (filters.channel) {
      filtered = filtered.filter(review => 
        review.channel &&
        review.channel.toLowerCase() === filters.channel.toLowerCase()
      );
    }

    // Filter by minimum rating
    if (filters.minRating) {
      const minRating = parseInt(filters.minRating, 10);
      filtered = filtered.filter(review => 
        review.overallRating && review.overallRating >= minRating
      );
    }

    // Filter by date range
    if (filters.startDate) {
      filtered = filtered.filter(review => 
        new Date(review.submittedAt) >= new Date(filters.startDate)
      );
    }

    if (filters.endDate) {
      filtered = filtered.filter(review => 
        new Date(review.submittedAt) <= new Date(filters.endDate)
      );
    }

    setFilteredReviews(filtered);
  }, [filters, reviews]);

  // Fetch reviews on component mount
  useEffect(() => {
    fetchReviews();
  }, [fetchReviews]);

  // Handle approve/disapprove action
  const handleApprove = async (id, approved) => {
    try {
      await reviewsApi.approveReview(id, approved);
      
      // Update local state optimistically
      setReviews(prevReviews =>
        prevReviews.map(review =>
          review.id === id ? { ...review, isApprovedForWebsite: approved } : review
        )
      );
      
      setFilteredReviews(prevFiltered =>
        prevFiltered.map(review =>
          review.id === id ? { ...review, isApprovedForWebsite: approved } : review
        )
      );
    } catch (err) {
      setError('Failed to update review status. Please try again.');
    }
  };

  // Handle filter changes
  const handleFilterChange = (newFilters) => {
    setFilters(newFilters);
  };

  // Handle sync button click
  const handleSync = async () => {
    await fetchReviews();
  };

  if (loading && !reviews.length) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="text-xl">Loading reviews...</div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
          {error}
        </div>
      )}

      {stats && <StatsCards stats={stats} />}
      
      <FilterPanel 
        filters={filters}
        onFilterChange={handleFilterChange}
        onSync={handleSync}
      />
      
      <div className="mt-8">
        <h2 className="text-2xl font-bold mb-4">
          Reviews ({filteredReviews.length})
        </h2>
        
        {filteredReviews.length === 0 ? (
          <div className="text-center py-12 text-gray-500">
            No reviews found matching your filters.
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {filteredReviews.map((review) => (
              <ReviewCard 
                key={review.id}
                review={review}
                onApprove={handleApprove}
              />
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default Dashboard;
import axios from 'axios';

const API_BASE_URL = 'https://localhost:7002/api'; // Update with your port

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Reviews API
export const reviewsApi = {
  // Fetch from Hostaway and sync
  fetchHostawayReviews: async () => {
    const response = await api.get('/reviews/hostaway');
    return response.data;
  },

  // Get all reviews with filters
  getAllReviews: async (filters = {}) => {
    const params = new URLSearchParams();
    if (filters.listing) params.append('listing', filters.listing);
    if (filters.channel) params.append('channel', filters.channel);
    if (filters.minRating) params.append('minRating', filters.minRating);
    if (filters.startDate) params.append('startDate', filters.startDate);
    if (filters.endDate) params.append('endDate', filters.endDate);

    const response = await api.get(`/reviews?${params.toString()}`);
    return response.data;
  },

  // Get single review
  getReview: async (id) => {
    const response = await api.get(`/reviews/${id}`);
    return response.data;
  },

  // Approve/Unapprove review
  approveReview: async (id, approved) => {
    const response = await api.put(`/reviews/${id}/approve`, { approved });
    return response.data;
  },

  // Get approved reviews for public display
  getPublicReviews: async (listing = '') => {
    const params = listing ? `?listing=${listing}` : '';
    const response = await api.get(`/reviews/public${params}`);
    return response.data;
  },

  // Get dashboard statistics
  getStats: async () => {
    const response = await api.get('/reviews/stats');
    return response.data;
  },

  // Sync reviews
  syncReviews: async () => {
    const response = await api.post('/reviews/sync');
    return response.data;
  },
};

export default api;
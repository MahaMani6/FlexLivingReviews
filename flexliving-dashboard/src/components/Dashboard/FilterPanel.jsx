import React from 'react';
import { Search, Filter, RefreshCw } from 'lucide-react';

const FilterPanel = ({ 
  filters = {}, 
  setFilters = () => console.warn('setFilters function not provided'), 
  onSync = () => console.warn('onSync function not provided'), 
  loading = false 
}) => {
  const handleFilterChange = (key, value) => {
    if (typeof setFilters !== 'function') {
      console.error('setFilters is not a function');
      return;
    }
    setFilters(prev => ({ ...prev, [key]: value }));
  };

  return (
    <div className="bg-white rounded-lg shadow-md p-6 mb-6">
      <div className="flex items-center justify-between mb-4">
        <div className="flex items-center gap-2">
          <Filter className="w-5 h-5 text-gray-600" />
          <h2 className="text-lg font-semibold">Filters</h2>
        </div>
        <button
          onClick={onSync}
          disabled={loading}
          className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50"
        >
          <RefreshCw className={`w-4 h-4 ${loading ? 'animate-spin' : ''}`} />
          Sync Reviews
        </button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
        {/* Search by listing */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Property Name
          </label>
          <div className="relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 w-4 h-4 text-gray-400" />
            <input
              type="text"
              value={filters.listing || ''}
              onChange={(e) => handleFilterChange('listing', e.target.value)}
              placeholder="Search property..."
              className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
          </div>
        </div>

        {/* Channel filter */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Channel
          </label>
          <select
            value={filters.channel || ''}
            onChange={(e) => handleFilterChange('channel', e.target.value)}
            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          >
            <option value="">All Channels</option>
            <option value="Airbnb">Airbnb</option>
            <option value="Booking.com">Booking.com</option>
            <option value="Direct">Direct</option>
          </select>
        </div>

        {/* Rating filter */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Min Rating
          </label>
          <select
            value={filters.minRating || ''}
            onChange={(e) => handleFilterChange('minRating', e.target.value)}
            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          >
            <option value="">All Ratings</option>
            <option value="7">7+</option>
            <option value="8">8+</option>
            <option value="9">9+</option>
            <option value="10">10 only</option>
          </select>
        </div>

        {/* Date filter */}
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Date Range
          </label>
          <select
            onChange={(e) => {
              const value = e.target.value;
              const now = new Date();
              let startDate = '';

              if (value === '7days') {
                startDate = new Date(now.setDate(now.getDate() - 7)).toISOString().split('T')[0];
              } else if (value === '30days') {
                startDate = new Date(now.setDate(now.getDate() - 30)).toISOString().split('T')[0];
              } else if (value === '90days') {
                startDate = new Date(now.setDate(now.getDate() - 90)).toISOString().split('T')[0];
              }

              handleFilterChange('startDate', startDate);
            }}
            className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          >
            <option value="">All Time</option>
            <option value="7days">Last 7 Days</option>
            <option value="30days">Last 30 Days</option>
            <option value="90days">Last 90 Days</option>
          </select>
        </div>
      </div>
    </div>
  );
};

export default FilterPanel;
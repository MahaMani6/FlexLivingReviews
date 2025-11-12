import React, { useState } from 'react';
import Dashboard from './components/Dashboard/Dashboard';
import PublicReviewsSection from './components/PublicReviews/PublicReviewsSection';
import './App.css';

function App() {
  const [currentView, setCurrentView] = useState('dashboard');
  const [selectedProperty, setSelectedProperty] = useState('');

  return (
    <div className="App">
      {/* Navigation Bar */}
      <nav className="bg-white shadow-md">
        <div className="max-w-7xl mx-auto px-4">
          <div className="flex items-center justify-between h-16">
            <div className="flex items-center gap-8">
              <h1 className="text-xl font-bold text-blue-600">Flex Living</h1>
              <div className="flex gap-4">
                <button
                  onClick={() => setCurrentView('dashboard')}
                  className={`px-4 py-2 rounded-lg font-medium transition-colors ${
                    currentView === 'dashboard'
                      ? 'bg-blue-600 text-white'
                      : 'text-gray-600 hover:bg-gray-100'
                  }`}
                >
                  Manager Dashboard
                </button>
                <button
                  onClick={() => setCurrentView('public')}
                  className={`px-4 py-2 rounded-lg font-medium transition-colors ${
                    currentView === 'public'
                      ? 'bg-blue-600 text-white'
                      : 'text-gray-600 hover:bg-gray-100'
                  }`}
                >
                  Public View
                </button>
              </div>
            </div>
          </div>
        </div>
      </nav>

      {/* Main Content Area */}
      {currentView === 'dashboard' ? (
        <Dashboard />
      ) : (
        <div className="min-h-screen bg-white">
          {/* Property Selector for Public View */}
          <div className="bg-gray-100 py-4">
            <div className="max-w-6xl mx-auto px-4">
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Select Property (Optional):
              </label>
              <select
                value={selectedProperty}
                onChange={(e) => setSelectedProperty(e.target.value)}
                className="px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500"
              >
                <option value="">All Properties</option>
                <option value="2B N1 A - 29 Shoreditch Heights">
                  2B N1 A - 29 Shoreditch Heights
                </option>
                <option value="1B Studio - 15 Camden Lock">
                  1B Studio - 15 Camden Lock
                </option>
                <option value="3B Penthouse - 42 Kings Cross">
                  3B Penthouse - 42 Kings Cross
                </option>
              </select>
            </div>
          </div>

          {/* Hero Section */}
          <div className="bg-gradient-to-r from-blue-600 to-blue-800 text-white py-20">
            <div className="max-w-6xl mx-auto px-4 text-center">
              <h1 className="text-5xl font-bold mb-4">
                {selectedProperty || 'Premium Serviced Apartments'}
              </h1>
              <p className="text-xl text-blue-100">
                Flexible living spaces in the heart of London
              </p>
            </div>
          </div>

          {/* Display Public Reviews */}
          <PublicReviewsSection listingName={selectedProperty} />
        </div>
      )}
    </div>
  );
}

export default App;
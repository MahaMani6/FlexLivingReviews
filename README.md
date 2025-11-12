# Flex Living Reviews Dashboard

A comprehensive reviews management system for Flex Living properties, allowing managers to view, filter, approve, and display guest reviews from multiple channels.

## 🚀 Features

- ✅ Hostaway API Integration with mock data fallback
- ✅ Manager Dashboard with advanced filtering
- ✅ Review approval workflow
- ✅ Public-facing review display (approved reviews only)
- ✅ Dashboard statistics and analytics
- ✅ Responsive design (mobile, tablet, desktop)
- ✅ RESTful API with Swagger documentation

## 💻 Tech Stack

**Backend:**
- ASP.NET Core 8.0 Web API
- C# 12
- SQL Server with Entity Framework Core
- RESTful API Architecture

**Frontend:**
- React 18
- Tailwind CSS
- Axios for API calls

## 📋 Prerequisites

- Visual Studio 2022
- .NET 8.0 SDK
- Node.js 18+
- SQL Server LocalDB

## ⚡ Quick Start

### Backend Setup
```bash
1. Open FlexLivingReviews.sln in Visual Studio 2022
2. Open Package Manager Console
3. Run: Update-Database
4. Press F5 to launch API
5. Swagger UI opens at https://localhost:XXXX/swagger
```

### Frontend Setup
```bash
1. Navigate to flexliving-dashboard folder
2. Run: npm install
3. Update API URL in src/services/api.js
4. Run: npm start
5. Dashboard opens at http://localhost:3000
```

### First Use

1. Click "Sync Reviews" to load sample data
2. Test filtering and approval features
3. Switch to "Public View" to see customer-facing display

## 🔍 Key Endpoint (Required)
```
GET /api/reviews/hostaway
```

Fetches reviews from Hostaway API, normalizes data, and returns structured JSON.

## 📂 Project Structure
```
FlexLivingReviews/
├── Controllers/          # API endpoints
├── Services/            # Business logic
├── Models/              # Data models
├── Data/                # Database context
└── flexliving-dashboard/
    ├── src/components/  # React components
    └── src/services/    # API integration
```

## 🗄️ Database Schema

- **Reviews**: Main review data with approval flags
- **ReviewCategories**: Category-specific ratings (cleanliness, communication, etc.)

Relationship: One Review → Many Categories

## 📊 API Endpoints

- `GET /api/reviews/hostaway` - Sync and fetch from Hostaway
- `GET /api/reviews` - Get all reviews with filters
- `PUT /api/reviews/{id}/approve` - Approve/unapprove review
- `GET /api/reviews/public` - Get approved reviews only
- `GET /api/reviews/stats` - Dashboard statistics

## 🔬 Google Reviews Integration

Research completed and documented. Integration is feasible with limitations (read-only, 5 reviews max, rate limits). Recommended for display purposes only.

## 🤖 Development Notes

This project was developed with assistance from Claude AI for technical guidance and best practices consultation. All implementation, design decisions, and problem-solving were completed independently.

## 👤 Author

Mahalakshmi C M

## 📄 License

Private - Assessment Project for Flex Living
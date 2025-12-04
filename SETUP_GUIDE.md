# FarmSync - Complete Setup Guide

## 🎯 Overview

FarmSync is a comprehensive farm management platform with role-based access control, built with:
- **Backend**: .NET 8 WebAPI with Clean Architecture
- **Frontend**: Angular 18 with Material Design
- **Database**: PostgreSQL 16

## 📁 Project Structure

```
Makhasaneni/
├── src/
│   ├── FarmSync.API/              # API Layer
│   ├── FarmSync.Application/      # Business Logic
│   ├── FarmSync.Domain/           # Domain Models
│   └── FarmSync.Infrastructure/   # Data Access
├── farmsync-ui/                   # Angular Frontend
└── FarmSync.sln                   # Solution File
```

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 18+
- PostgreSQL 16
- Angular CLI

### Backend Setup

1. **Database Configuration**
   - Connection string in `src/FarmSync.API/appsettings.json`
   - Database: `FarmSyncDb`
   - Already migrated and seeded

2. **Run the API**
   ```bash
   cd /Users/nhlakanipho/Dev/Makhasaneni
   dotnet run --project src/FarmSync.API/FarmSync.API.csproj
   ```
   - API runs on: http://localhost:5201
   - Swagger UI: http://localhost:5201/swagger

### Frontend Setup

1. **Install Dependencies** (Already done)
   ```bash
   cd /Users/nhlakanipho/Dev/Makhasaneni/farmsync-ui
   npm install
   ```

2. **Run the Angular App**
   ```bash
   npm start
   ```
   - App runs on: http://localhost:4200
   - Uses proxy to avoid CORS issues

## 👥 User Roles & Access

### System Admin
- **Username**: admin
- **Password**: Admin@123
- **Access**: Full system access
- **Route**: `/admin`
- **Features**:
  - Dashboard with system overview
  - Full inventory management (CRUD)
  - User management (future)
  - System settings (future)

### Accountant
- **Route**: `/accountant`
- **Features**:
  - Financial dashboard
  - Reports and expenses (future)

### Operations Manager
- **Route**: `/operations`
- **Features**:
  - Operations dashboard
  - Task management (future)
  - Vehicle & equipment tracking (future)

### HR Manager
- **Route**: `/hr`
- **Features**:
  - HR dashboard
  - Employee management (future)
  - Attendance & payroll (future)

## 🔐 Authentication Flow

1. User logs in at `/login`
2. Backend validates credentials and returns JWT token
3. Token stored in localStorage
4. All API requests include `Authorization: Bearer {token}` header
5. Routes protected by `authGuard` and `roleGuard`
6. Users redirected to appropriate dashboard based on role

## 📊 Current Features

### ✅ Implemented
- JWT Authentication with BCrypt password hashing
- Role-based routing and guards
- Inventory Management:
  - List all items
  - Create new item
  - Edit existing item
  - Delete item
  - Low stock tracking
- Reference Data:
  - Categories (Seeds, Fertilizers, Pesticides, Feed, Equipment)
  - Types (Consumable, Durable, Equipment)
  - Units (Kilogram, Liter, Piece, Bag, Ton)
  - Locations (Main Warehouse, Field Storage, Processing Plant)
- Material Design UI
- Responsive layout

### 🔄 Phase 1 Complete
- Backend API with Clean Architecture
- Database schema with migrations
- Angular frontend with role-based views
- Authentication & authorization

### 📋 Future Phases
- Phase 2: Employee & Team Management
- Phase 3: Task & Schedule Management
- Phase 4: Vehicle & Equipment Management
- Phase 5: Financial Management
- Phase 6: Reporting & Analytics
- Phase 7: Hardware Integration (IoT devices)

## 🛠️ API Endpoints

### Authentication
- `POST /api/auth/login` - Login
- `POST /api/auth/register` - Register new user

### Inventory
- `GET /api/inventoryitems` - Get all items
- `GET /api/inventoryitems/{id}` - Get item by ID
- `POST /api/inventoryitems` - Create item
- `PUT /api/inventoryitems/{id}` - Update item
- `DELETE /api/inventoryitems/{id}` - Delete item
- `GET /api/inventoryitems/low-stock` - Get low stock items

### Reference Data
- `GET /api/referencedata/categories` - Get all categories
- `GET /api/referencedata/types` - Get all types
- `GET /api/referencedata/units` - Get all units
- `GET /api/referencedata/locations` - Get all locations
- `GET /api/referencedata/statuses` - Get all statuses
- `GET /api/referencedata/conditions` - Get all conditions
- `GET /api/referencedata/maintenance-types` - Get all maintenance types

## 🧪 Testing

### Using Swagger
1. Navigate to http://localhost:5201/swagger
2. Click "Authorize" button
3. Login via `/api/auth/login` endpoint
4. Copy the token from response
5. Click "Authorize" and enter: `Bearer {your-token}`
6. Test other endpoints

### Using the UI
1. Navigate to http://localhost:4200
2. Login with admin/Admin@123
3. Explore the dashboard
4. Navigate to Inventory
5. Create/Edit/Delete items

### Using API-Tests.http
Open `API-Tests.http` in VS Code and use REST Client extension.

## 🔧 Configuration Files

### Backend
- `appsettings.json` - App configuration
- `appsettings.Development.json` - Dev settings
- `Program.cs` - Startup configuration

### Frontend
- `angular.json` - Angular configuration
- `tsconfig.json` - TypeScript configuration
- `proxy.conf.json` - Development proxy
- `environment.ts` - Environment variables

## 📦 Database Schema

### Users & Auth
- Users
- Roles (Admin, Manager, User)
- UserRoles (many-to-many)

### Inventory
- InventoryItems
- InventoryLocations
- StockLevels
- InventoryTransactions
- Equipment
- EquipmentMaintenanceRecords

### Reference Data
- InventoryCategories
- InventoryTypes
- UnitsOfMeasure
- TransactionStatuses
- EquipmentConditions
- MaintenanceTypes

## 🎨 UI Components

### Shared Components
- Header (with user menu)
- Sidebar (with role-based navigation)
- Layout components for each role

### Feature Components
- Login page
- Admin dashboard
- Inventory list
- Inventory form (create/edit)
- Accountant dashboard
- Operations dashboard
- HR dashboard

## 📝 Next Steps

1. **Start Both Servers**:
   ```bash
   # Terminal 1 - API
   cd /Users/nhlakanipho/Dev/Makhasaneni
   dotnet run --project src/FarmSync.API/FarmSync.API.csproj

   # Terminal 2 - Angular
   cd /Users/nhlakanipho/Dev/Makhasaneni/farmsync-ui
   npm start
   ```

2. **Test the Application**:
   - Visit http://localhost:4200
   - Login with admin/Admin@123
   - Create inventory items
   - Test role-based navigation

3. **Continue Development**:
   - Add more features to admin panel
   - Implement accountant features
   - Build operations management
   - Add HR functionality

## 🐛 Troubleshooting

### CORS Issues
- Proxy configuration in `proxy.conf.json` handles this
- API has CORS enabled for http://localhost:4200

### JWT Token Expired
- Tokens expire after 7 days
- Logout and login again

### Database Connection
- Check PostgreSQL is running
- Verify connection string in appsettings.json
- Ensure database `FarmSyncDb` exists

## 📚 Technologies Used

### Backend
- .NET 8.0
- Entity Framework Core 8.0.11
- PostgreSQL (Npgsql 8.0.11)
- JWT Authentication
- BCrypt for password hashing
- Swagger/OpenAPI

### Frontend
- Angular 18
- Angular Material 18
- RxJS 7.8
- TypeScript 5.4
- SCSS

## 📄 License
Proprietary - FarmSync © 2024

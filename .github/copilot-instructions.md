# FarmSync AI Agent Guidelines

## Architecture Overview

**FarmSync** is a comprehensive farm management platform using Clean Architecture with .NET 8 backend and Angular 18 frontend.

### Backend Structure (.NET 8 Clean Architecture)
```
src/
├── FarmSync.Domain/         # Entities, Enums, Interfaces (no dependencies)
├── FarmSync.Application/    # Business logic, DTOs, Service interfaces
├── FarmSync.Infrastructure/ # Data access, EF Core, external services
└── FarmSync.API/           # Controllers, SignalR hubs, Program.cs
```

**Dependency Flow**: API → Application → Infrastructure → Domain (Domain has zero dependencies)

### Frontend Structure (Angular 18)
```
material-dashboard-angular2-master/src/app/
├── core/          # Guards, interceptors, services, constants
├── layouts/       # Admin layout with role-based routing
├── [module]/      # Feature modules: procurement, fleet, hr, inventory, reports
└── app.routing.ts # Root routes with AuthGuard protection
```

## Critical Patterns

### 1. Role-Based Authorization

**Backend**: Use full role names with spaces in `[Authorize]` attributes:
```csharp
[Authorize(Roles = "Accounting Manager,Operations Manager,Admin")]
public async Task<ActionResult<PurchaseOrderDto>> ApprovePurchaseOrder(Guid id)
```

**Roles**: `Admin`, `Operations Manager`, `Accounting Manager`, `Operations Clerk`, `Accountant`, `Team Leader`, `Driver`, `General Worker`

**Frontend**: Permission system in `core/constants/permission.constants.ts`:
```typescript
// Check permissions via PermissionService
MODULE_PERMISSIONS.procurement.purchase_orders.approve 
// Returns: ['Accounting Manager', 'Operations Manager']
```

### 2. Service Layer Pattern

**All business logic in Application layer services**, not controllers:
- Controllers are thin, only handle HTTP concerns
- Services implement interfaces from `Application/Interfaces/`
- Repositories accessed via `IUnitOfWork` or injected directly
- Example: `PurchaseOrderService.ApproveAsync()` handles approval logic, status changes, notifications

### 3. Notification & Real-Time Updates

**SignalR architecture**:
- Backend: `NotificationHub.cs` groups users by `user_{userId}` 
- Service: `INotificationService.SendNotificationAsync()` creates DB record + SignalR push + optional email
- Frontend: `NotificationService` connects with JWT token, subscribes to `ReceiveNotification` event
- **Always call notification service after state changes** (approvals, status updates, assignments)

```csharp
// Backend - inject and call after approval/state change
await _notificationService.SendNotificationAsync(
    userId, 
    NotificationType.PurchaseOrderStatusChanged, 
    "PO {PONumber} approved", 
    relatedEntityId
);
```

### 4. Entity Framework Patterns

**DbContext**: `FarmSyncDbContext` with all DbSets, OnModelCreating() for configurations
**Migrations**: Run from solution root: `dotnet ef migrations add MigrationName --project src/FarmSync.Infrastructure --startup-project src/FarmSync.API`
**BaseEntity**: All domain entities inherit from `BaseEntity` (Id, CreatedAt, UpdatedAt, CreatedBy, UpdatedBy)

### 5. Frontend Service Communication

**API calls via services**:
```typescript
// Services use environment.apiUrl
private apiUrl = `${environment.apiUrl}/PurchaseOrders`;

// All HTTP calls return Observables
getPurchaseOrders(): Observable<PurchaseOrder[]> {
  return this.http.get<PurchaseOrder[]>(this.apiUrl);
}
```

**Proxy config**: `proxy.conf.json` routes `/api` to `http://localhost:5201` (avoid CORS)

### 6. Document Upload Pattern

- Backend: `IDocumentStorageService` (LocalFileStorageService implementation)
- Storage: `wwwroot/uploads` (configured in appsettings.json)
- Upload: MultipartFormData to dedicated document endpoints
- Association: Documents linked via `relatedEntityType` and `relatedEntityId`

## Development Workflows

### Running the Application

**Backend** (from `/Users/nhlakanipho/Dev/Makhasaneni`):
```bash
dotnet run --project src/FarmSync.API/FarmSync.API.csproj
# Runs on http://localhost:5201
# Swagger: http://localhost:5201/swagger
```

**Frontend** (from `material-dashboard-angular2-master`):
```bash
npm start  # Runs on http://localhost:4200
```

### Database Changes

1. Update entity in `FarmSync.Domain/Entities/`
2. Update DbContext if adding new DbSet
3. Create migration: `dotnet ef migrations add MigrationName --project src/FarmSync.Infrastructure --startup-project src/FarmSync.API`
4. Apply: `dotnet ef database update --project src/FarmSync.API`

**Connection string**: `appsettings.json` → PostgreSQL on localhost:5432

### Adding New Features

**Backend flow**:
1. Create/update entity in `Domain/Entities/`
2. Create DTO in `Application/DTOs/`
3. Create service interface in `Application/Interfaces/`
4. Implement service in `Application/Services/`
5. Create repository interface in `Domain/Interfaces/`
6. Implement repository in `Infrastructure/Repositories/`
7. Register in `Program.cs` dependency injection
8. Create controller in `API/Controllers/`

**Frontend flow**:
1. Create model in `[module]/models/`
2. Create service in `[module]/services/`
3. Add routes to module routing
4. Create components (list, form, detail)
5. Update permissions in `permission.constants.ts`

## Common Issues & Solutions

### Authorization Failures
- **Problem**: 403 Forbidden despite correct role
- **Cause**: Role name mismatch (using "Manager" instead of "Accounting Manager")
- **Fix**: Use full role names with spaces in `[Authorize(Roles = "")]`

### Notifications Not Working
- **Problem**: No real-time updates after approval/changes
- **Cause**: Service methods not calling `INotificationService`
- **Fix**: Inject `INotificationService` and call `SendNotificationAsync()` after state changes

### Frontend Permission Check Fails
- **Problem**: UI shows buttons but backend rejects
- **Cause**: Frontend permissions in `permission.constants.ts` don't match backend `[Authorize]` roles
- **Fix**: Ensure role arrays match between frontend constants and backend attributes

### Unit of Measure Display
- **Pattern**: `InventoryItem` has `unitOfMeasureName` - always display next to quantity fields
- **Example**: "Quantity (kg)" instead of just "Quantity"

## Module-Specific Notes

### Procurement Workflows
- PO approval requires `Accounting Manager` or `Operations Manager`
- After approval: notify creator + operations clerk
- Document types: Quotation (on create), POP + Invoice (after approval)
- Expected delivery hidden on create, shown on edit for specific roles

### HR/Workforce
- Clock events use `ClockEventType`: ClockIn, ClockOut, BreakStart, BreakEnd
- Tasks assigned via `WorkTask` with `TeamId` and `AssignedToId`
- Attendance exports via `AttendanceService.ExportPayrollData()`

### Fleet Management
- Vehicle assignments via `DriverAssignment` (one active per vehicle)
- GPS tracking in `GPSLocation` entity with lat/long/timestamp
- Maintenance records linked to `Vehicle` with `MaintenanceType` reference data

### Reports
- All reports use POST with filter DTOs (date ranges, status filters)
- Export methods return file streams with `FileStreamResult`
- Report types: PurchaseOrders, GoodsReceived, InventoryValuation, SupplierTransactions, Expenses

## Testing Credentials

**Admin**: `admin` / `Admin@123` (full access)
**Test users**: Check seeded data in `FarmSyncDbContext` seed methods

## Key Files Reference

- **DI Registration**: `src/FarmSync.API/Program.cs`
- **Permissions**: `material-dashboard-angular2-master/src/app/core/constants/permission.constants.ts`
- **API URLs**: `material-dashboard-angular2-master/src/environments/environment.ts`
- **DB Context**: `src/FarmSync.Infrastructure/Data/FarmSyncDbContext.cs`
- **Auth Flow**: `material-dashboard-angular2-master/src/app/core/services/auth.service.ts`

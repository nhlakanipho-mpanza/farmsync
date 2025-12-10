using Microsoft.EntityFrameworkCore;
using FarmSync.Infrastructure.Data;
using FarmSync.Infrastructure.Repositories;
using FarmSync.Application.Interfaces;
using FarmSync.Application.Services;
using FarmSync.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SignalR
builder.Services.AddSignalR();

// Configure Database
builder.Services.AddDbContext<FarmSyncDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRepository<FarmSync.Domain.Entities.Inventory.InventoryItem>, InventoryItemRepository>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
builder.Services.AddScoped<IGoodsReceivedRepository, GoodsReceivedRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

// Register HR Repositories
builder.Services.AddScoped<FarmSync.Domain.Interfaces.HR.IEmployeeRepository, FarmSync.Infrastructure.Repositories.HR.EmployeeRepository>();
builder.Services.AddScoped<FarmSync.Domain.Interfaces.HR.ITeamRepository, FarmSync.Infrastructure.Repositories.HR.TeamRepository>();
builder.Services.AddScoped<FarmSync.Domain.Interfaces.HR.IWorkTaskRepository, FarmSync.Infrastructure.Repositories.HR.WorkTaskRepository>();
builder.Services.AddScoped<FarmSync.Domain.Interfaces.HR.IClockEventRepository, FarmSync.Infrastructure.Repositories.HR.ClockEventRepository>();
builder.Services.AddScoped<FarmSync.Domain.Interfaces.HR.IInventoryIssueRepository, FarmSync.Infrastructure.Repositories.HR.InventoryIssueRepository>();
builder.Services.AddScoped<FarmSync.Domain.Interfaces.HR.IEquipmentIssueRepository, FarmSync.Infrastructure.Repositories.HR.EquipmentIssueRepository>();

// Register Generic Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
// Register Services
builder.Services.AddScoped<IInventoryItemService, InventoryItemService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<IGoodsReceivedService, GoodsReceivedService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();ervice>();
builder.Services.AddScoped<ISupplierService, SupplierService>();

// Register HR Services
builder.Services.AddScoped<FarmSync.Application.Interfaces.HR.IEmployeeService, FarmSync.Application.Services.HR.EmployeeService>();
builder.Services.AddScoped<FarmSync.Application.Interfaces.HR.ITeamService, FarmSync.Application.Services.HR.TeamService>();
builder.Services.AddScoped<FarmSync.Application.Interfaces.HR.IWorkTaskService, FarmSync.Application.Services.HR.WorkTaskService>();
builder.Services.AddScoped<FarmSync.Application.Interfaces.HR.ITaskTemplateService, FarmSync.Application.Services.HR.TaskTemplateService>();
builder.Services.AddScoped<FarmSync.Application.Interfaces.HR.IAttendanceService, FarmSync.Application.Services.HR.AttendanceService>();
builder.Services.AddScoped<FarmSync.Application.Interfaces.HR.IIssuingService, FarmSync.Application.Services.HR.IssuingService>();

// Register HR Repositories
builder.Services.AddScoped<FarmSync.Domain.Interfaces.HR.ITaskTemplateRepository, FarmSync.Infrastructure.Repositories.HR.TaskTemplateRepository>();

// Register Fleet Repositories
builder.Services.AddScoped<FarmSync.Domain.Interfaces.Fleet.IVehicleRepository, FarmSync.Infrastructure.Repositories.Fleet.VehicleRepository>();
builder.Services.AddScoped<FarmSync.Domain.Interfaces.Fleet.IDriverAssignmentRepository, FarmSync.Infrastructure.Repositories.Fleet.DriverAssignmentRepository>();
builder.Services.AddScoped<FarmSync.Domain.Interfaces.Fleet.ITripLogRepository, FarmSync.Infrastructure.Repositories.Fleet.TripLogRepository>();
builder.Services.AddScoped<FarmSync.Domain.Interfaces.Fleet.IMaintenanceRecordRepository, FarmSync.Infrastructure.Repositories.Fleet.MaintenanceRecordRepository>();
builder.Services.AddScoped<FarmSync.Domain.Interfaces.Fleet.IFuelLogRepository, FarmSync.Infrastructure.Repositories.Fleet.FuelLogRepository>();
builder.Services.AddScoped<FarmSync.Domain.Interfaces.Fleet.IIncidentReportRepository, FarmSync.Infrastructure.Repositories.Fleet.IncidentReportRepository>();
builder.Services.AddScoped<FarmSync.Domain.Interfaces.Fleet.IGPSLocationRepository, FarmSync.Infrastructure.Repositories.Fleet.GPSLocationRepository>();

// Register Fleet Services
builder.Services.AddScoped<FarmSync.Application.Interfaces.Fleet.IVehicleService, FarmSync.Application.Services.Fleet.VehicleService>();
builder.Services.AddScoped<FarmSync.Application.Interfaces.Fleet.IDriverAssignmentService, FarmSync.Application.Services.Fleet.DriverAssignmentService>();
builder.Services.AddScoped<FarmSync.Application.Interfaces.Fleet.ITripLogService, FarmSync.Application.Services.Fleet.TripLogService>();
builder.Services.AddScoped<FarmSync.Application.Interfaces.Fleet.IMaintenanceService, FarmSync.Application.Services.Fleet.MaintenanceService>();
builder.Services.AddScoped<FarmSync.Application.Interfaces.Fleet.IFuelService, FarmSync.Application.Services.Fleet.FuelService>();
builder.Services.AddScoped<FarmSync.Application.Interfaces.Fleet.IGPSTrackingService, FarmSync.Application.Services.Fleet.GPSTrackingService>();

// Register Report Services
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IExportService, ExportService>();

// Register Document and Notification Services
builder.Services.AddScoped<IEmailService, FarmSync.Infrastructure.Services.EmailService>();
builder.Services.AddScoped<IDocumentStorageService, FarmSync.Infrastructure.Services.LocalFileStorageService>();
builder.Services.AddScoped<INotificationService>(provider =>
{
    var context = provider.GetRequiredService<FarmSyncDbContext>();
    var emailService = provider.GetRequiredService<IEmailService>();
    var logger = provider.GetRequiredService<ILogger<FarmSync.Infrastructure.Services.NotificationService>>();
    var hubService = provider.GetRequiredService<INotificationHubService>();
    return new FarmSync.Infrastructure.Services.NotificationService(context, emailService, logger, hubService);
});

// Register SignalR hub service
builder.Services.AddScoped<INotificationHubService, FarmSync.API.Services.NotificationHubService>();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization();

// Configure CORS - Allow any origin for testing notifications on multiple ports
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FarmSyncDbContext>();
    await DbSeeder.SeedAsync(context);
    await FarmSync.API.Data.GPSDataSeeder.SeedGPSData(context);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<FarmSync.API.Hubs.NotificationHub>("/hubs/notifications");

app.Run();


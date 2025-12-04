using Microsoft.EntityFrameworkCore;
using WhatsappBackend.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument();

// Database provider selection
var provider = Environment.GetEnvironmentVariable("DATABASE_PROVIDER") ?? "SQLite";
if (string.Equals(provider, "Postgres", StringComparison.OrdinalIgnoreCase))
{
    var pgUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    if (string.IsNullOrWhiteSpace(pgUrl))
    {
        throw new InvalidOperationException("DATABASE_PROVIDER=Postgres but DATABASE_URL is not set in environment.");
    }
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(pgUrl));
}
else
{
    // Default to SQLite in local file
    var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
    Directory.CreateDirectory(dataDir);
    var dbPath = Path.Combine(dataDir, "app.db");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite($"Data Source={dbPath}"));
}

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowCredentials()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Use CORS
app.UseCors("AllowAll");

// Configure OpenAPI/Swagger
app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.Path = "/docs";
});

// Apply migrations/ensure database and seed in Development
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var isSqlite = db.Database.ProviderName?.Contains("Sqlite", StringComparison.OrdinalIgnoreCase) == true;
    if (!isSqlite)
    {
        // For Postgres, attempt migration
        await db.Database.MigrateAsync();
    }
    else
    {
        // For SQLite in dev, ensure created (Migrate also works with SQLite)
        await db.Database.MigrateAsync();
    }

    if (app.Environment.IsDevelopment())
    {
        await AppDbContext.EnsureDevSeedAsync(db);
    }
}

// Health check endpoint
// PUBLIC_INTERFACE
app.MapGet("/", () => new { message = "Healthy" });

app.Run();
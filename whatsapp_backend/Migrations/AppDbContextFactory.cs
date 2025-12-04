using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using WhatsappBackend.Data;

namespace WhatsappBackend.Migrations
{
    /// <summary>
    /// Design-time factory for EF Core tools and helps resolving provider for migrations.
    /// Uses same environment variables as runtime.
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var provider = Environment.GetEnvironmentVariable("DATABASE_PROVIDER") ?? "SQLite";
            if (string.Equals(provider, "Postgres", StringComparison.OrdinalIgnoreCase))
            {
                var pgUrl = Environment.GetEnvironmentVariable("DATABASE_URL") ?? "Host=localhost;Database=app;Username=postgres;Password=postgres";
                builder.UseNpgsql(pgUrl);
            }
            else
            {
                var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
                Directory.CreateDirectory(dataDir);
                var dbPath = Path.Combine(dataDir, "app.db");
                builder.UseSqlite($"Data Source={dbPath}");
            }
            return new AppDbContext(builder.Options);
        }
    }
}

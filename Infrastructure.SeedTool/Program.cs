using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

var connectionString = config.GetConnectionString("DefaultConnection");
Console.WriteLine($"[INFO] Using connection string: {connectionString}");

if (connectionString is null)
{
    Console.WriteLine("[ERROR] Connection string is null.");
    return;
}

if (!connectionString.StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
{
    Console.WriteLine("[ERROR] Connection string does not start with 'Data Source='.");
    return;
}

var fileName = connectionString.Substring("Data Source=".Length).Trim();
Console.WriteLine($"[INFO] Database file will be at: {Path.GetFullPath(fileName)}");

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseSqlite(connectionString);

await using var db = new AppDbContext(optionsBuilder.Options);

Console.WriteLine("[INFO] Applying migrations (if any)...");
await db.Database.MigrateAsync();

Console.WriteLine("[INFO] Done.");

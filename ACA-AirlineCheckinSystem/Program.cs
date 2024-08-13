// See https://aka.ms/new-console-template for more information
using ACA_AirlineCheckinSystem;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


Console.WriteLine("Hello, World!");

//Adding dependency injection both thecalsses although it made it bit coplicated but good for learning
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddScoped<CheckinSystemWithLockingApproach>(
            provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                // Ensure that the connection string is correctly retrieved
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("The connection string 'DefaultConnection' is not configured.");
                }
                return new CheckinSystemWithLockingApproach(connectionString);
            });
        services.AddScoped<CheckinSystemWithoutLockingApproach>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            // Ensure that the connection string is correctly retrieved
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("The connection string 'DefaultConnection' is not configured.");
            }
            return new CheckinSystemWithoutLockingApproach(connectionString);
        });
        // Register your services
        services.AddScoped<DatabaseContext>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            // Ensure that the connection string is correctly retrieved
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("The connection string 'DefaultConnection' is not configured.");
            }
            return new DatabaseContext(connectionString);
        });
    })
    .Build();

// Resolve and use the CheckinSystemWithoutLockingApproach service
using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    Console.WriteLine("Test without Lock");
    var checkinSystemWithoutLock = services.GetRequiredService<CheckinSystemWithoutLockingApproach>();
    checkinSystemWithoutLock.CheckinSeats(); // Call the method

    Console.WriteLine("Test with Lock");
    var checkinSystemWithLock = services.GetRequiredService<CheckinSystemWithLockingApproach>();
    checkinSystemWithLock.CheckinSeats(); // Call the method

}

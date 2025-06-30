using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using ChitChatApp.Core.Application.Interfaces;
using ChitChatApp.Core.Application.Services;
using ChitChatApp.Core.Infrastructure.Persistence.Repositories;
using ChitChatApp.Core.Infrastructure.Supabase;
using System.IO;
using Microsoft.Extensions.Configuration;

public class TestRunner
{
    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsetting.json", optional: false, reloadOnChange: true)
            .Build();

        var supabaseUrl = configuration["Supabase:Url"];
        var supabaseAnonKey = configuration["Supabase:AnonKey"];

        if (string.IsNullOrEmpty(supabaseUrl) || string.IsNullOrEmpty(supabaseAnonKey))
        {
            Console.WriteLine("Error: Supabase URL or AnonKey is missing in appsetting.json.");
            return;
        }

        // Configure services
        services.AddSingleton<SupabaseService>(sp =>
            new SupabaseService(supabaseUrl, supabaseAnonKey));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthService, AuthService>();

        var serviceProvider = services.BuildServiceProvider();

        var authService = serviceProvider.GetRequiredService<IAuthService>();

        var uniqueId = Guid.NewGuid().ToString();
        var testEmail = $"testuser_{uniqueId}@example.com";
        var testUsername = $"testuser_{uniqueId}";

        Console.WriteLine("Testing User Registration...");
        bool registrationResult = await authService.RegisterAsync(testEmail, "password123", testUsername, "Test User");
        
        if (registrationResult)
        {
            Console.WriteLine("Registration successful.");
        }
        else
        {
            Console.WriteLine("Registration failed. User might already exist.");
        }

        Console.WriteLine("\nTesting User Login...");
        bool loginResult = await authService.LoginAsync(testEmail, "password123");

        if (loginResult)
        {
            Console.WriteLine("Login successful.");
            var user = await authService.GetCurrentUserAsync();
            Console.WriteLine($"Current User: {user?.user_name}");
        }
        else
        {
            Console.WriteLine("Login failed.");
        }

        if(loginResult)
        {
            await authService.LogoutAsync();
            Console.WriteLine("\nLogout successful.");
        }
    }
}

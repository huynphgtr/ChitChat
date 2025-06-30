using ChitChatApp.Core.Application.Interfaces;
using ChitChatApp.Core.Application.Services;
using ChitChatApp.Core.Infrastructure.Persistence.Repositories;
using ChitChatApp.Core.Infrastructure.Supabase;
using Microsoft.Extensions.DependencyInjection;

namespace ChitChatApp.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, string supabaseUrl, string supabaseKey)
    {
        // Register Supabase Service (Singleton)
        services.AddSingleton(provider => new SupabaseService(supabaseUrl, supabaseKey));

        // Register Repositories (Scoped)
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();

        // Register Application Services (Scoped)
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IChatService, ChatService>();

        return services;
    }

    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        // Register repositories and services without Supabase configuration
        // This overload requires Supabase to be configured separately

        // Register Repositories (Scoped)
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();

        // Register Application Services (Scoped)
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IChatService, ChatService>();

        return services;
    }
}
using ChitChatApp.Core.Application.Interfaces;
using ChitChatApp.Core.Domain.Entity;
using ChitChatApp.Core.Infrastructure.Supabase;
using SupabaseClient = Supabase.Client;

namespace ChitChatApp.Core.Application.Services;

public class AuthService : IAuthService
{
    private readonly SupabaseClient _supabaseClient;
    private readonly IUserRepository _userRepository;

    public AuthService(SupabaseService supabaseService, IUserRepository userRepository)
    {
        _supabaseClient = supabaseService.Client;
        _userRepository = userRepository;
    }

    public bool IsAuthenticated => _supabaseClient.Auth.CurrentUser != null;

    public async Task<bool> LoginAsync(string email, string password)
    {
        try
        {
            var session = await _supabaseClient.Auth.SignIn(email, password);
            return session?.User != null;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> RegisterAsync(string email, string password, string username, string fullName)
    {
        try
        {
            // Check if user already exists
            if (await _userRepository.UserExistsAsync(email, username))
                return false;

            // Register with Supabase Auth
            var session = await _supabaseClient.Auth.SignUp(email, password);
            if (session?.User?.Id == null)
                return false;

            // Create user profile
            var user = new Users
            {
                id = Guid.Parse(session.User.Id),
                email = email,
                user_name = username,
                full_name = fullName,
                status = true,
                avatar_url = null
            };

            await _userRepository.CreateUserAsync(user);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            await _supabaseClient.Auth.SignOut();
        }
        catch
        {
            // Handle logout error if needed
        }
    }

    public async Task<Users?> GetCurrentUserAsync()
    {
        var currentUser = _supabaseClient.Auth.CurrentUser;
        if (currentUser?.Id == null)
            return null;

        if (Guid.TryParse(currentUser.Id, out var userId))
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        return null;
    }

    public Guid? GetCurrentUserId()
    {
        var currentUser = _supabaseClient.Auth.CurrentUser;
        if (currentUser?.Id == null)
            return null;

        if (Guid.TryParse(currentUser.Id, out var userId))
            return userId;

        return null;
    }
}
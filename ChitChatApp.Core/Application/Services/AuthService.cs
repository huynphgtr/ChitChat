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
            if (session?.User != null)
            {
                Console.WriteLine($"Login successful for {email}");
                return true;
            }
            Console.WriteLine($"Login failed for {email}: Invalid credentials");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login failed for {email}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RegisterAsync(string email, string password, string username, string fullName)
    {
        try
        {
            Console.WriteLine($"Starting registration for {email}/{username}");
            
            // Check if user already exists
            bool userExists = await _userRepository.UserExistsAsync(email, username);
            Console.WriteLine($"User exists check result: {userExists}");
            
            if (userExists)
            {
                Console.WriteLine($"Registration failed for {email}/{username}: User already exists.");
                return false;
            }

            username = string.IsNullOrEmpty(username) ? email.Substring(0, email.IndexOf('@')) : username;
            fullName = string.IsNullOrEmpty(fullName) ? "User" : fullName;

            // Try a simplified approach - signup without triggering the problematic automatic profile creation
            Console.WriteLine($"Attempting Supabase Auth SignUp for {email}");
            
            Guid userId;
            Supabase.Gotrue.Session? session = null;
            bool authSucceeded = false;
            
            try
            {
                // Try auth signup - this will likely fail due to trigger
                session = await _supabaseClient.Auth.SignUp(email, password);
                
                if (session?.User?.Id == null)
                {
                    Console.WriteLine($"Registration failed for {email}/{username}: No user ID returned.");
                    return false;
                }
                
                userId = Guid.Parse(session.User.Id);
                Console.WriteLine($"Auth signup successful. User ID: {userId}");
                authSucceeded = true;
            }
            catch (Exception authEx)
            {
                Console.WriteLine($"Auth signup failed due to trigger: {authEx.Message}");
                
                // Extract user ID from error and attempt manual profile creation
                if (authEx.Message.Contains("null value in column") && authEx.Message.Contains("users"))
                {
                    var uuidPattern = @"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
                    var match = System.Text.RegularExpressions.Regex.Match(authEx.Message, uuidPattern);
                    
                    if (match.Success && Guid.TryParse(match.Value, out userId))
                    {
                        Console.WriteLine($"Extracted user ID from error: {userId}. Creating complete profile...");
                        
                        // Create the complete user profile manually
                        var newUser = new Users(email, fullName, username)
                        {
                            id = userId,
                            status = true,
                            avatar_url = null
                        };
                        
                        // Try to create the profile (this might work if the partial record was rolled back)
                        try
                        {
                            await _userRepository.CreateUserAsync(newUser);
                            Console.WriteLine("Successfully created complete user profile.");
                            
                            // Now try to sign in
                            session = await _supabaseClient.Auth.SignIn(email, password);
                            Console.WriteLine("Successfully signed in after profile creation.");
                        }
                        catch (Exception profileEx)
                        {
                            Console.WriteLine($"Failed to create complete profile: {profileEx.Message}");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Could not extract user ID from error.");
                        return false;
                    }
                }
                else
                {
                    throw; // Re-throw if it's a different type of error
                }
            }
            
            // If we reached here via the success path, create the profile manually
            if (session?.User != null && authSucceeded)
            {
                Console.WriteLine("Creating user profile manually...");
                var newUser = new Users(email, fullName, username)
                {
                    id = userId,
                    status = true,
                    avatar_url = null
                };
                
                await _userRepository.CreateUserAsync(newUser);
            }
            Console.WriteLine($"Registration successful for {email}/{username}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Registration failed for {email}/{username}: {ex.Message}");
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
using ChitChatApp.Core.Application.Interfaces;
using ChitChatApp.Core.Domain.Entity;
using ChitChatApp.Core.Infrastructure.Supabase;
using SupabaseClient = Supabase.Client;

namespace ChitChatApp.Core.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SupabaseClient _supabaseClient;

    public UserRepository(SupabaseService supabaseService)
    {
        _supabaseClient = supabaseService.Client;
    }

    public async Task<Users?> GetUserByIdAsync(Guid id)
    {
        var result = await _supabaseClient
            .From<Users>()
            .Where(u => u.id == id)
            .Single();
        
        return result;
    }

    public async Task<Users?> GetUserByEmailAsync(string email)
    {
        var result = await _supabaseClient
            .From<Users>()
            .Where(u => u.email == email)
            .Single();
        
        return result;
    }

    public async Task<Users?> GetUserByUsernameAsync(string username)
    {
        var result = await _supabaseClient
            .From<Users>()
            .Where(u => u.user_name == username)
            .Single();
        
        return result;
    }

    public async Task<IEnumerable<Users>> GetAllUsersAsync()
    {
        var result = await _supabaseClient
            .From<Users>()
            .Get();
        
        return result.Models;
    }

    public async Task<Users> CreateUserAsync(Users user)
    {
        var result = await _supabaseClient
            .From<Users>()
            .Insert(user);
        
        return result.Models.First();
    }

    public async Task<Users> UpdateUserAsync(Users user)
    {
        var result = await _supabaseClient
            .From<Users>()
            .Update(user);
        
        return result.Models.First();
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        try
        {
            await _supabaseClient
                .From<Users>()
                .Where(u => u.id == id)
                .Delete();
            
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UserExistsAsync(string email, string username)
    {
        var result = await _supabaseClient
            .From<Users>()
            .Where(u => u.email == email || u.user_name == username)
            .Get();
        
        return result.Models.Any();
    }
}
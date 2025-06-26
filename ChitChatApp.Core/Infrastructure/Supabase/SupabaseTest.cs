using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChatApp.Core.Infrastructure.Supabase
{
    public class SupabaseTest
    {
        static async Task Main(string[] args)
        {
            try
            {
                var supabaseService = new SupabaseService(SupabaseConfig.Url, SupabaseConfig.Key);
                await supabaseService.Client.InitializeAsync();
                Console.WriteLine("Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Supabase: {ex.Message}");
            }
        }
    }
}

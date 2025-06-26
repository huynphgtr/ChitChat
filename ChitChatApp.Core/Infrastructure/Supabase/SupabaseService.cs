using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChatApp.Core.Infrastructure.Supabase
{
    public class SupabaseService
    {
        private readonly Client _client;

        public SupabaseService(string url, string key)
        {
            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };
            _client = new Client(url, key, options);
        }

        public Client Client => _client;
    }
}

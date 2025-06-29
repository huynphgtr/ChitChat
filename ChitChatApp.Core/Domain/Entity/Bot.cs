using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChatApp.Core.Domain.Entity
{
    [Table("bot")]
    public class Bot : BaseModel
    {
        [PrimaryKey("id", false)]
        public int id { get; set; }

        [Column("bot_name")]
        public required string bot_name { get; set; }

        [Column("user_id")]
        public Guid user_id { get; set; }

        [Column("bot_api")]
        public required string bot_api { get; set; }

        [Column("created_at")]
        public DateTime created_at { get; set; }
    }

}

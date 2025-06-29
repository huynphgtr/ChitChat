using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using ChitChatApp.Core.Domain.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChatApp.Core.Domain.Entity
{
    [Table("bot_chat")]
    public class Bot_chat : BaseModel
    {
        [PrimaryKey("id", true)]
        public int id { get; set; }

        [Column("bot_id")]
        public int bot_id { get; set; }

        [Column("user_id")]
        public Guid user_id { get; set; }

        [Column("content")]
        public required string content { get; set; }

        [Column("status")]
        public required string status { get; set; }

        [Column("created_at")]
        public DateTime created_at { get; set; }
    }

}

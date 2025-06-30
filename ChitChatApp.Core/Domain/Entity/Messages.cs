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
    [Table("messages")]
    public class Messages : BaseModel
    {
        [PrimaryKey("id", true)]
        public long id { get; set; }

        [Column("room_id")]
        public Guid? room_id { get; set; }

        [Column("sender_id")]
        public Guid? sender_id { get; set; }

        [Column("content")]
        public string content { get; set; }

        [Column("message_type")]
        public string message_type { get; set; }

        [Column("sent_at")]
        public DateTime sent_at { get; set; }

        [Column("is_read")]
        public bool is_read { get; set; }

        public Messages(string content, string message_type)
        {
            this.content = content;
            this.message_type = message_type;
        }

        public Messages() 
        {
            content = string.Empty;
            message_type = string.Empty;
        }
    }
}

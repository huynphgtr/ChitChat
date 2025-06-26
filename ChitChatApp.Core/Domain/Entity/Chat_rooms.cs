using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChatApp.Core.Domain.Entity
{
    [Table("chat_rooms")]
    public class Chat_rooms : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid id { get; set; }

        [Column("name")]
        public string name { get; set; }

        [Column("is_group")]
        public bool is_group { get; set; }

        [Column("created_at")]
        public DateTime created_at { get; set; }

        [Column("is_deleted")]
        public bool is_deleted { get; set; }
    }

}

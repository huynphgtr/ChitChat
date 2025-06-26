using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChatApp.Core.Domain.Entity
{
    [Table("user_contacts")]
    public class User_contacts : BaseModel
    {
        [PrimaryKey("id", false)]
        public int id { get; set; }

        [Column("user_id")]
        public Guid user_id { get; set; }

        [Column("friend_id")]
        public Guid friend_id { get; set; }

        [Column("created_at")]
        public DateTime created_at { get; set; }
    }
}

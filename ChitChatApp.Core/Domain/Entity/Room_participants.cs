using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChatApp.Core.Domain.Entity
{
    [Table("room_participants")]
    public class Room_participants : BaseModel
    {
        [PrimaryKey("id", false)]
        public long id { get; set; }

        [Column("room_id")]
        public Guid room_id { get; set; }

        [Column("user_id")]
        public Guid user_id { get; set; }

        [Column("joined_at")]
        public DateTime? joined_at { get; set; }
    }

}

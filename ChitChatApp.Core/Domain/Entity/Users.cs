using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChatApp.Core.Domain.Entity
{
    [Table("users")]
    public class Users : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid id { get; set; }

        [Column("email")]
        public required string email { get; set; }

        [Column("full_name")]
        public required string full_name { get; set; }

        [Column("user_name")]
        public required string user_name { get; set; }

        [Column("avatar_url")]
        public required string avatar_url { get; set; }

        [Column("user_role")]
        public required string user_role { get; set; }

        [Column("status")]
        public bool status { get; set; }
    }
}

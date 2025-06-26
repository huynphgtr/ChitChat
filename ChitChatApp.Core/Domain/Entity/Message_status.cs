using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChatApp.Core.Domain.Entity
{
    [Table("message_status")]
    public class Message_status : BaseModel
    {
        [PrimaryKey("id", false)]
        public int id { get; set; }

        [Column("message_id")]
        public int message_id { get; set; }

        [Column("receiver_id")]
        public Guid receiver_id { get; set; }

        [Column("status")]
        public string status { get; set; }

        [Column("updated_at")]
        public DateTime updated_at { get; set; }
    }

}

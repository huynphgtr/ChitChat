using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChatApp.Core.Domain.Entity
{

    [Table("attachments")]
    public class Attachments : BaseModel
    {
        [PrimaryKey("id", false)]
        public int id { get; set; }

        [Column("message_id")]
        public int message_id { get; set; }

        [Column("file_url")]
        public required string file_url { get; set; }

        [Column("file_type")]
        public required string file_type { get; set; }

        [Column("file_size")]
        public int file_size { get; set; }

        [Column("uploaded_at")]
        public DateTime uploaded_at { get; set; }
    }

}

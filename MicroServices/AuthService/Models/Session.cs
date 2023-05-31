using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Session
    {
        [Key]
        [Column("session_id")]
        public int SessionId {get; set;}
        [Column("user_id")]
        public int UserId {get; set;}

        [Column("session_expiration")]
        public DateTime DateExpiration {get; set;} = DateTime.Now.AddDays(7);
    }
}

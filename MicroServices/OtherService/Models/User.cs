using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId {get; set;}

        [Required]
        [Column("user_username")]
        public string Username {get; set;} = String.Empty;

        [Required]
        [Column("user_email")]
        public string Email {get; set;} = String.Empty;

        [Required]
        [Column("user_passwordhash")]
        public byte[] PasswordHash {get; set;} = new byte[0];

        [Required]
        [Column("user_passwordsalt")]
        public byte[] PasswordSalt {get; set;} = new byte[0];

        [Column("user_name")]
        public string? Name {get; set;} = null;

        [Column("user_surname")]
        public string? Surname {get; set;} = null;
    }
}
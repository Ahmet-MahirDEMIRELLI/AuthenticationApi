using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationApi.Classes
{
	[Table("Users")]
	public class User
	{
        [Key]
        [Column("UserId")]
        public int UserId { get; set; }

        [Column("UserName")]
        public required string UserName { get; set; }

        [Column("Password")]
        public required string Password { get; set; }

        [Column("Email")]
        public required string Email { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationApi.Classes
{
	[Table("LoggedUsers")]
	public class LoggedUser
	{
        [Key]
        [Column("LogId")]
        public int LogId { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }

        [Column("LogInDateTime")]
        public DateTime LogInDateTime { get; set; }

        [Column("SpecialCode")]
        public required string SpecialCode { get; set; }
    }
}

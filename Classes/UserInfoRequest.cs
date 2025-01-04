namespace AuthenticationApi.Classes
{
	public class UserInfoRequest
	{
        public int UserId { get; set; }
        public required string SpecialCode { get; set; }
    }
}

namespace AuthenticationApi.Classes
{
	public class GetMyInfoRequest
	{
        public int UserId { get; set; }
        public required string SpecialCode { get; set; }
    }
}

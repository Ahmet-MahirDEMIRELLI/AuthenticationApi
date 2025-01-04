namespace AuthenticationApi.Classes
{
	public class LogOutRequest
	{
        public required string UserName { get; set; }
        public required string SpecialCode { get; set; }
    }
}

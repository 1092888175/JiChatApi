namespace JiChatApi.DTO
{
    public class LoginRequest
    {
        public long UserId {  get; set; }
        public required string Password { get; set; }
    }
}

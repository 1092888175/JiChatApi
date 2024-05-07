using JiChatApi.Entity;

namespace JiChatApi.DTO
{
    public class LoginResponse
    {
        public required long UserId {  get; set; }
        public required string Token { get; set; }
        public required JiChatUserDetail? UserDetail { get; set; }
        public required List<JiChatFriend> Friends{ get; set; }
        public required List<JiChatGroupInfo> Groups { get; set; }
    }
}

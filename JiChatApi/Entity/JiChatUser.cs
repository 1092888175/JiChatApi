using Microsoft.AspNetCore.Identity;

namespace JiChatApi.Entity
{
    public class JiChatUser:IdentityUser<long>
    {
        public string? Token { get; set; }
        public JiChatUserDetail UserDetail { get; set; }=new JiChatUserDetail();
        public List<JiChatFriend> Friends { get; set; } = [];
        public List<JiChatGroupInfo> Groups { get; set; } = [];
    }
}

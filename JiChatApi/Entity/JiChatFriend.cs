using Microsoft.EntityFrameworkCore;

namespace JiChatApi.Entity
{
    [PrimaryKey("UserId", "FriendId")]
    public class JiChatFriend
    {
        
        public long UserId {  get; set; }
        public long FriendId { get; set; }
        public string? CustomizedName {  get; set; }
        public JiChatMessagePermissionEnum MessagePermission { get; set; } = JiChatMessagePermissionEnum.NotifyAll;
        

    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace JiChatApi.Entity
{
    [PrimaryKey("UserId","GroupId")]
    public class JiChatGroupInfo
    {
        public long UserId {  get; set; }
        public long GroupId { get; set; }
        public string? CustomizedGroupName {  get; set; }
        public JiChatMessagePermissionEnum MessagePermission { get; set; } = JiChatMessagePermissionEnum.NotifyAll;
    }
}

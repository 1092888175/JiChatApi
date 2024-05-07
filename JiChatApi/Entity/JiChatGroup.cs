using System.ComponentModel.DataAnnotations;

namespace JiChatApi.Entity
{
    public class JiChatGroup
    {
        [Key]
        public long GroupId {  get; set; }
        public string GroupName { get; set; } = null!;
        public string? Avatar {  get; set; } = null;
        public required JiChatUser Owner {  get; set; }
        public List<JiChatUser> Member {  get; set; } = [];

    }
}
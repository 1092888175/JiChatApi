using JiChatApi.Entity;

namespace JiChatApi.DTO
{
    public class GetGroupInfoResponse
    {
        public long GroupId { get; set; }
        public string GroupName { get; set; } = null!;
        public string? Avatar {  get; set; } = null!;
        public required long OwnerId { get; set; }
        public List<long> MemberId { get; set; } = [];
    }
}

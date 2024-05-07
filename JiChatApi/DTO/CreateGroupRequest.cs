namespace JiChatApi.DTO
{
    public class CreateGroupRequest
    {
        public required long UserId { get; set; }
        public required string GroupName { get; set; }
    }
}

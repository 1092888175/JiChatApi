using System.ComponentModel.DataAnnotations;

namespace JiChatApi.Entity
{
    public class JiChatUserDetail
    {
        [Key]
        public long UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Avatar { get; set; }
        public string? Sex {  get; set; }
        public int Age {  get; set; }
    }
}

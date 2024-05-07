using System.ComponentModel.DataAnnotations;

namespace JiChatApi.DTO
{
    public class RegisterModel
    {
        public string? Email {  get; set; }
        public string? Phone {  get; set; }
        public string? UserName { get; set; }
        public required string Password {  get; set; }

    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JiChatApi.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailPhoneToUserDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "JiChatUserDetail",
                type: "NVARCHAR2(2000)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "JiChatUserDetail",
                type: "NVARCHAR2(2000)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "JiChatGroup",
                type: "NVARCHAR2(2000)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "JiChatUserDetail");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "JiChatUserDetail");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "JiChatGroup");
        }
    }
}

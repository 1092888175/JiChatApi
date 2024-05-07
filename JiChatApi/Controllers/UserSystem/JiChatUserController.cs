using JiChatApi.Data;
using JiChatApi.DTO;
using JiChatApi.Entity;
using JiChatApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace JiChatApi.Controllers.UserSystem
{
    [Route("api/user")]
    [ApiController]
    public class JiChatUserController : ControllerBase
    {
        private readonly JiChatContext _context;
        private readonly UserManager<JiChatUser> _userManager;

        public JiChatUserController(
            JiChatContext context,
            UserManager<JiChatUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserIdModel>> registerUser(RegisterModel model)
        {
            string password = model.Password;
            if (password == null)
            {
                return BadRequest();
            }
            if(model.UserName == null)
            {
                model.UserName = "济信用户";
            }
            JiChatUser user = new()
            {
                UserName = Guid.NewGuid().ToString(),
                UserDetail = new JiChatUserDetail() { 
                    UserName = model.UserName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Avatar = "http://43.142.90.90/0.jpg"
                }
            };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.ToString());
            }
            var token = await RongYunService.getUserToken(user.Id);
            if (token == "")
            {
                return StatusCode(500);
            }
            user.Token = token;
            _context.SaveChanges();
            return Ok(new UserIdModel
            {
                UserId = user.Id,
            });
        }
        [HttpPost("change_user_info")]
        public async Task<ActionResult> changeUserInfo(JiChatUserDetail userDetail)
        {
            if (userDetail == null)
            {
                return BadRequest("Request Empty");
            }
            var userDetailInDb = await _context.JiChatUserDetail.FindAsync(userDetail.UserId);
            if (userDetailInDb == null)
            {
                return NotFound("User Not Found");
            }
            else
            {
                userDetailInDb.UserName = userDetail.UserName;
                userDetailInDb.Email = userDetail.Email;
                userDetailInDb.Phone = userDetail.Phone;
                userDetailInDb.Avatar = userDetail.Avatar;
                userDetailInDb.Sex = userDetail.Sex;
                userDetailInDb.Age = userDetail.Age;
                await _context.SaveChangesAsync();
                return Ok("Success");
            }
        }
        [HttpGet("get_friend_list/{userId}")]
        public async Task<ActionResult<List<JiChatFriend>>> getFriendList(long userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString())
                ;
            if (user == null)
            {
                return NotFound("User Not Found");
            }
            else
            {
                _context.Entry(user)
                    .Collection(b => b.Friends)
                    .Load();
                return user.Friends;
            }
        }
        [HttpGet("get_group_list/{userId}")]
        public async Task<ActionResult<List<JiChatGroupInfo>>> getGroupList(long userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString())
                ;
            if (user == null)
            {
                return NotFound("User Not Found");
            }
            else
            {
                _context.Entry(user)
                    .Collection(b => b.Groups)
                    .Load();
                return user.Groups;
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> login(LoginRequest model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString())
                ;
            if (user == null)
            {
                return NotFound("User Not Found");
            }
            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                _context.Entry(user)
                    .Collection(b => b.Friends)
                    .Load();
                _context.Entry(user)
                    .Collection(b => b.Groups)
                    .Load();
                _context.Entry(user)
                    .Reference(b => b.UserDetail)
                    .Load();
                if (user.Token == null)
                {
                    var token = await RongYunService.getUserToken(model.UserId);
                    if (token == "")
                    {
                        return StatusCode(500);
                    }
                    else
                    {
                        user.Token = token;
                        _context.SaveChanges();
                        return Ok(new LoginResponse{
                            UserId = user.Id,
                            Token = user.Token,
                            UserDetail = await _context.JiChatUserDetail.FindAsync(user.Id),
                            Friends = user.Friends,
                            Groups = user.Groups
                        });
                    }
                }
                else
                {
                    return Ok(new LoginResponse
                    {
                        UserId = user.Id,
                        Token = user.Token,
                        UserDetail = await _context.JiChatUserDetail.FindAsync(user.Id),
                        Friends = user.Friends,
                        Groups = user.Groups
                    });
                }
            }
            return Unauthorized();
        }
    }
}

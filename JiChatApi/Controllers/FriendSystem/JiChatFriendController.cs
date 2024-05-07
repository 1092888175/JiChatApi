using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JiChatApi.Data;
using JiChatApi.Entity;
using NuGet.Versioning;
using JiChatApi.DTO;
using System.Drawing.Printing;

namespace JiChatApi.Controllers.FriendSystem
{
    [Route("api/friend")]
    [ApiController]
    public class JiChatFriendController : ControllerBase
    {
        private readonly JiChatContext _context;

        public JiChatFriendController(JiChatContext context)
        {
            _context = context;
        }


        [HttpPost("add_friend")]
        public async Task<ActionResult<JiChatFriend>> AddFriend(FriendRequest model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            else
            {
                var user = await _context.Users.FindAsync(model.UserId);
                var friend = await _context.Users.FindAsync(model.FriendId);
                if (user == null)
                {
                    return NotFound("User Not Found");
                }
                if (friend == null)
                {
                    return NotFound("Friend User Not Found");
                }
                user.Friends.Add(new JiChatFriend
                {
                    UserId = model.UserId,
                    FriendId = model.FriendId
                });
                friend.Friends.Add(new JiChatFriend
                {
                    UserId = model.FriendId,
                    FriendId = model.UserId
                });
                _context.SaveChanges();
                return Ok(new JiChatFriend
                {
                    UserId = model.UserId,
                    FriendId = model.FriendId
                });
            }
        }
        [HttpPost("delete_friend")]
        public async Task<ActionResult> DeleteFriend(FriendRequest model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            else
            {
                var user = await _context.Users.FindAsync(model.UserId);
                if (user == null)
                {
                    return NotFound("User Not Found");
                }
                _context.Entry(user)
                   .Collection(b => b.Friends)
                   .Load();
                Console.WriteLine(user.Friends.Count);
                var friend = user.Friends.Find(b => {
                    Console.WriteLine($"{b.FriendId}");
                    return b.FriendId == model.FriendId;
                    });
                if(friend == null)
                {
                    return NotFound("Friend Not Found");
                }
                var status = user.Friends.Remove(friend);
                user = await _context.Users.FindAsync(model.FriendId);
                if(user == null)
                {
                    return NotFound("User Not Found");
                }
                _context.Entry(user)
                   .Collection(b => b.Friends)
                   .Load();
                friend = user.Friends.Single(b => b.FriendId == model.UserId);
                status = status && user.Friends.Remove(friend);
                await _context.SaveChangesAsync();
                if (status)
                    return Ok("Friend Deleted Successfully");
                else
                    return NotFound("Friends Delete Failed");
            }
        }
        [HttpPost("change_friend_information")]
        public async Task<ActionResult> ChangeFriendInformation(JiChatFriend model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            else
            {
                var user = await _context.Users.FindAsync(model.UserId);
                if (user == null)
                {
                    return NotFound("User Not Found");
                }
                var friend = user.Friends.Single(b => b.FriendId == model.FriendId);
                if (friend == null)
                {
                    return NotFound("Friend Not Found");
                }
                else
                {
                    friend.CustomizedName = model.CustomizedName;
                    friend.MessagePermission = model.MessagePermission;
                    int status = _context.SaveChanges();
                    if (status > 0 )
                        return Ok("Friend Info Updated Successfully");
                    else
                        return BadRequest("Friends Info Updated Failed");
                }
            }
        }
    }
}

using JiChatApi.Data;
using JiChatApi.DTO;
using JiChatApi.Entity;
using JiChatApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices.Protocols;
using System.Drawing.Printing;

namespace JiChatApi.Controllers.GroupSystem
{
    [Route("api/group")]
    [ApiController]
    public class JiChatGroupController : ControllerBase
    {
        private readonly JiChatContext _context;

        public JiChatGroupController(JiChatContext context)
        {
            _context = context;
        }
        [HttpPost("create")]
        public async Task<ActionResult<CreateGroupResponse>> controllerCreateGroup(CreateGroupRequest model)
        {
            JiChatGroup group = new()
            {
                GroupName = model.GroupName,
                Avatar = "http://43.142.90.90/0.jpg",
                Owner = _context.Users.Where(b => b.Id == model.UserId).First(),
            };
            var result = await _context.JiChatGroup.AddAsync(group);
            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.ToString());
            }
            _context.SaveChangesAsync().Wait();
            var res = await RongYunGroupService.createGroup(group.Owner.Id, group.GroupId, group.GroupName);
            if (res != null && res == "200")
            {
                _context.Users
                    .Where(b => b.Id == model.UserId)
                    .First()
                    .Groups
                    .Add(new JiChatGroupInfo
                    {
                        UserId = model.UserId,
                        GroupId = group.GroupId,
                        CustomizedGroupName = group.GroupName
                    });
                await _context.SaveChangesAsync();
                return new CreateGroupResponse
                {
                    GroupId = group.GroupId,
                    GroupName = group.GroupName,
                    Avatar = group.Avatar,
                    OwnerId = group.Owner.Id,
                    MemberId = group.Member.Select(b => b.Id).ToList()
                };
            }
            else
            {
                _context.JiChatGroup.Remove(group);
                _context.SaveChangesAsync().Wait();
                return StatusCode(StatusCodes.Status500InternalServerError, result.ToString());
            }
        }
        [HttpPost("dismiss")]
        public async Task<ActionResult> controllerDismissGroup(GroupRequest model)
        {
            var group = _context.JiChatGroup.Where(b => b.GroupId == model.GroupId).First();
            if (group == null)
            {
                return NotFound();
            }
            _context.Entry(group)
                .Reference(b=>b.Owner)
                .Load();
            var res = await RongYunGroupService.dismissGroup(group.Owner.Id, group.GroupId);
            if (res != null && res == "200")
            {
                _context.JiChatGroup.Remove(group);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("join")]
        public async Task<ActionResult<JiChatGroupInfo>> controllerJoinGroup(GroupRequest model)
        {
            var group = _context.JiChatGroup.Where(b => b.GroupId == model.GroupId).First();
            if (group == null)
            {
                return NotFound("Group Not Found");
            }
            Console.WriteLine("before join");
            var res = await RongYunGroupService.joinGroup(model.UserId, model.GroupId);
            if (res != null && res == "200")
            {
                _context.Users
                    .Where(b => b.Id == model.UserId)
                    .First()
                    .Groups
                    .Add(new JiChatGroupInfo
                    {
                        UserId = model.UserId,
                        GroupId = group.GroupId,
                        CustomizedGroupName = group.GroupName
                    });
                await _context.SaveChangesAsync();
                return Ok(new JiChatGroupInfo
                {
                    GroupId = group.GroupId,
                    UserId = model.UserId,
                    CustomizedGroupName = group.GroupName,
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("join_confirm")]
        public async Task<ActionResult<JiChatGroupInfo>> controllerJoinGroupConfirm(GroupRequest model)
        {
            var group = _context.JiChatGroup.Where(b => b.GroupId == model.GroupId).First();
            if (group == null)
            {
                return NotFound("Group Not Found");
            }
            Console.WriteLine("before join");
            var res = await RongYunGroupService.joinGroup(model.UserId, model.GroupId);
            if (res != null && res == "200")
            {
                _context.Users
                    .Where(b => b.Id == model.UserId)
                    .First()
                    .Groups
                    .Add(new JiChatGroupInfo
                    {
                        UserId = model.UserId,
                        GroupId = group.GroupId,
                        CustomizedGroupName = group.GroupName
                    });
                await _context.SaveChangesAsync();
                return Ok(new JiChatGroupInfo
                {
                    GroupId = group.GroupId,
                    UserId = model.UserId,
                    CustomizedGroupName = group.GroupName,
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("quit")]
        public async Task<ActionResult> controllerQuitGroup(GroupRequest model)
        {
            var group = _context.JiChatGroup.Where(b => b.GroupId == model.GroupId).First();
            if (group == null)
            {
                return NotFound("Group Not Found");
            }
            _context.Entry(group)
                .Reference(b => b.Owner)
                .Load();
            Console.WriteLine("before quit");
            var res = await RongYunGroupService.quitGroup(model.UserId, model.GroupId);
            Console.WriteLine(res);
            if (res != null && res == "200")
            {
                var user = _context.Users
                    .Where(b => b.Id == model.UserId)
                    .First();
                _context.Entry(user)
                    .Collection(b => b.Groups)
                    .Load();
                var gp = user.Groups
                    .Where(b => b.GroupId == model.GroupId)
                    .First();
                if (
                   user
                    .Groups
                    .Remove(gp)
                )
                {
                    await _context.SaveChangesAsync();
                    return Ok("Quit Group Success");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

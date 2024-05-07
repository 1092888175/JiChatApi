using JiChatApi.Data;
using JiChatApi.DTO;
using JiChatApi.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JiChatApi.Controllers.QuerySystem
{
    [Route("api/query")]
    [ApiController]
    public class JiChatQueryController : ControllerBase
    {
        private readonly JiChatContext _context;
        public JiChatQueryController(JiChatContext context)
        {
            _context = context;
        }
        [HttpGet("get_user_info/{userId}")]
        public async Task<ActionResult<JiChatUserDetail>> getUserInfo(long userId)
        {
            var userDetail = await _context.JiChatUserDetail.FindAsync(userId);
            if(userDetail == null)
            {
                return NotFound("User Not Found");
            }
            return Ok(userDetail);
        }
        
        [HttpGet("get_group_info/{groupId}")]
        public async Task<ActionResult<GetGroupInfoResponse>> getGroupInfo(long groupId)
        {
                var group = await _context.JiChatGroup.FindAsync(groupId);
                if (group==null)
                {
                    return NotFound("Group Not Found");
                }
                _context.Entry(group)
                    .Collection(b => b.Member)
                    .Load();
                return Ok(new GetGroupInfoResponse
                {
                    GroupId = group.GroupId,
                    GroupName = group.GroupName,
                    Avatar = group.Avatar,
                    OwnerId = group.Owner.Id,
                    MemberId = group.Member.Select(p => p.Id).ToList(),
                });
        }

    }
}

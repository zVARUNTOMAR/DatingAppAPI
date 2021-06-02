using DatingAppAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatingAppAPI.Extensions;
using DatingAppAPI.Entites;
using DatingAppAPI.DTOs;
using DatingAppAPI.Helpers;

namespace DatingAppAPI.Controllers
{
    [Authorize]
    public class LikesController : BaseAPIController
    {
        private readonly ILikesRepository _likesRepository;
        private readonly IUserRepository _userRepository;

        public LikesController(IUserRepository userRepository,ILikesRepository likesRepository)
        {
            _likesRepository = likesRepository;
            _userRepository = userRepository;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username) {

            var sourceUserId = User.GetUserId();

            var likedUser = await _userRepository.GetByUsernameAsync(username);

            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

            if (likedUser == null) return NotFound();

            if (sourceUser.UserName == username) { return BadRequest("You cannot like yourself"); }

            var userLike = await _likesRepository.GetUserLike(sourceUserId, likedUser.Id);
             
            if (userLike != null) {
                return BadRequest("You already like this user");
            }

            userLike = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUser.Id

            };

            sourceUser.LikedUsers.Add(userLike);

            if (await _userRepository.SaveAllAsync()) {

                return Ok();
            }

            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikeDTO>>> GetUserLikes([FromQuery] LikesParams likeParams) {

            likeParams.UserId = User.GetUserId();

            var users =  await _likesRepository.GetUserLikes(likeParams);

            Response.AddPaginationHeader(users.CurrentPage, users.TotalPages, users.PageSize, users.TotalCount);

            return Ok(users);

        }
    }
}

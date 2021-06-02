using DatingAppAPI.DTOs;
using DatingAppAPI.Entites;
using DatingAppAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Interfaces
{
    public interface ILikesRepository
    {
        Task<UserLike> GetUserLike(int sourceId, int likedUserId);

        Task<AppUser> GetUserWithLikes(int userId);

        Task<PagedList<LikeDTO>> GetUserLikes(LikesParams likesParams);
    }
}

using DatingAppAPI.DTOs;
using DatingAppAPI.Entites;
using DatingAppAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);

        Task<bool> SaveAllAsync();

        Task<IEnumerable<AppUser>> GetUsersAsync();

        Task<AppUser> GetUserByIdAsync(int id);

        Task<AppUser> GetByUsernameAsync(string username);

        Task<MemberDTO> GetMemberAsync(string username);

        Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams);

    }
}

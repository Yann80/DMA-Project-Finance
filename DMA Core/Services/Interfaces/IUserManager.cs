using Core.DTO;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DMA_ProjectManagement.Core.Services.Interfaces
{
    public interface IUserManager
    {
        Task<ClaimsPrincipal> LoginAsync(BackOfficeUserDTO backOfficeUserDTO);

        Task<bool> ChangePasswordAsync(string Id, string currentPassword, string newPassword);

        Task CreateUserAsync(BackOfficeUserDTO backOfficeUserDTO);

        Task<List<BackOfficeUserDTO>> GetUsersAsync();

        Task<bool> DeleteUserAsync(string Id);

        Task<BackOfficeUserDTO> UpdateUserAsync(BackOfficeUserDTO backOfficeUserDTO);

        Task<BackOfficeUserDTO> GetUserAsync(string Id);

        Task<List<IdentityRole>> GetRolesAsync();

        Task<IdentityRole> GetRoleAsync(string Id);

        Task CreateRoleAsync(IdentityRole identityRole);

        Task<bool> DeleteRoleAsync(string Id);

        Task<IdentityRole> UpdateRoleAsync(IdentityRole role);

        Task<BackOfficeUserDTO> GetUserInfoAsync();

        Task Logout();
    }
}

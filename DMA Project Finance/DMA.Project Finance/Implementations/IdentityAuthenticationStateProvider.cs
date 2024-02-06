using Core.DTO;
using DMA_ProjectManagement.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace DMA.ProjectManagement.Implementations
{
    public class IdentityAuthenticationStateProvider : AuthenticationStateProvider
    {
        private BackOfficeUserDTO _userInfoCache;
        private readonly IUserManager _userManager;

        public IdentityAuthenticationStateProvider(IUserManager userManager)
        {
            this._userManager = userManager;
        }

        public async Task Login(BackOfficeUserDTO backOfficeUserDTO)
        {
            await _userManager.LoginAsync(backOfficeUserDTO);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Logout()
        {
            await _userManager.Logout();
            _userInfoCache = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private async Task<BackOfficeUserDTO> GetUserInfo()
        {
            if (_userInfoCache != null && _userInfoCache.IsAuthenticated) return _userInfoCache;
            _userInfoCache = await _userManager.GetUserInfoAsync();
            return _userInfoCache;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = await GetUserInfo();
                if (userInfo.IsAuthenticated)
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userInfo.Username),
                    new Claim(ClaimTypes.Role, userInfo.Role ?? string.Empty),
                    new Claim(ClaimTypes.Email, userInfo.Email ?? string.Empty)
                };
                    identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Request failed:" + ex.ToString());
            }
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
    }
}

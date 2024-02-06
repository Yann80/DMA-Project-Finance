using Core.DTO;
using DMA_ProjectManagement.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;

namespace DMA_ProjectManagement.Core.Services
{
    public class UserManagerService : IUserManager
    {
        private readonly HttpClient _httpClient;

        public UserManagerService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("httpClient");
        }

        public async Task<ClaimsPrincipal> LoginAsync(BackOfficeUserDTO backOfficeUserDTO)
        {
            ClaimsPrincipal claimsPrincipal = null;

            var content = System.Text.Json.JsonSerializer.Serialize(new Dictionary<string, string>
            {
                {"id",backOfficeUserDTO.Id ?? string.Empty },
                { "username", backOfficeUserDTO.Username },
                { "password", backOfficeUserDTO.Password ?? string.Empty },
                { "email", backOfficeUserDTO.Email ?? "user@example.com" },
                { "role", backOfficeUserDTO.Role ?? string.Empty }
            });
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync("api/Identity/Login", httpContent);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await result.Content.ReadAsStringAsync();
                var userDTO = JsonConvert.DeserializeObject<BackOfficeUserDTO>(responseBody);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userDTO.Username),
                    new Claim(ClaimTypes.Role, userDTO.Role ?? string.Empty),
                    new Claim(ClaimTypes.Email, userDTO.Email ?? string.Empty)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            }

            return claimsPrincipal;
        }

        public async Task<List<BackOfficeUserDTO>> GetUsersAsync()
        {
            var listOfUserDTO = new List<BackOfficeUserDTO>();
            var result = await _httpClient.GetAsync("api/Identity/GetUsers");

            if(result.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await result.Content.ReadAsStringAsync();
                listOfUserDTO = JsonConvert.DeserializeObject<List<BackOfficeUserDTO>>(responseBody);
            }

            return listOfUserDTO;
        }

        public async Task<BackOfficeUserDTO> GetUserAsync(string Id)
        {
            var userDTO = new BackOfficeUserDTO();

            var result = await _httpClient.GetAsync($"api/Identity/GetUser/{Id}");

            if(result.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await result.Content.ReadAsStringAsync();
                userDTO = JsonConvert.DeserializeObject<BackOfficeUserDTO>(responseBody);
            }

            return userDTO;
        }

        public async Task CreateUserAsync(BackOfficeUserDTO backOfficeUserDTO)
        {
            var content = System.Text.Json.JsonSerializer.Serialize(backOfficeUserDTO);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            await _httpClient.PostAsync("api/Identity/CreateUser",httpContent);
        }

        public async Task<bool> DeleteUserAsync(string Id)
        {
            var result = await _httpClient.DeleteAsync($"api/Identity/DeleteUser/{Id}");
            return result.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<BackOfficeUserDTO> UpdateUserAsync(BackOfficeUserDTO backOfficeUserDTO)
        {
            backOfficeUserDTO.Password = backOfficeUserDTO.Password ?? string.Empty;

            var content = System.Text.Json.JsonSerializer.Serialize(backOfficeUserDTO);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            var result = await _httpClient.PutAsync($"api/Identity/UpdateUser/{backOfficeUserDTO.Id}", httpContent);

            if (result.StatusCode == HttpStatusCode.NoContent)
            {
                return backOfficeUserDTO;
            }

            return null;
        }

        public async Task<List<IdentityRole>> GetRolesAsync()
        {
            var listOfRoles = new List<IdentityRole>();

            var result = await _httpClient.GetAsync("api/Identity/GetRoles");

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await result.Content.ReadAsStringAsync();
                listOfRoles = JsonConvert.DeserializeObject<List<IdentityRole>>(responseBody);
            }

            return listOfRoles;
        }

        public async Task<IdentityRole> GetRoleAsync(string id)
        {
            var role = new IdentityRole();

            var result = await _httpClient.GetAsync($"api/Identity/GetRole/{id}");
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await result.Content.ReadAsStringAsync();
                role = JsonConvert.DeserializeObject<IdentityRole>(responseBody);
            }

            return role;
        }

        public async Task CreateRoleAsync(IdentityRole identityRole)
        {
            var content = System.Text.Json.JsonSerializer.Serialize(identityRole);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            await _httpClient.PostAsync("api/Identity/CreateRole", httpContent);
        }

        public async Task<bool> DeleteRoleAsync(string Id)
        {
            var result = await _httpClient.DeleteAsync($"api/Identity/DeleteRole/{Id}");
            return result.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<IdentityRole> UpdateRoleAsync(IdentityRole identityRole)
        {
            var content = System.Text.Json.JsonSerializer.Serialize(identityRole);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            var result = await _httpClient.PutAsync($"api/Identity/UpdateRole/{identityRole.Id}", httpContent);

            if (result.StatusCode == HttpStatusCode.NoContent)
            {
                return identityRole;
            }

            return null;
        }

        public async Task<BackOfficeUserDTO> GetUserInfoAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<BackOfficeUserDTO>("api/Identity/UserInfo");
            return result;
        }

        public async Task Logout()
        {
            var result = await _httpClient.PostAsync("api/Identity/SignOut", null);
            result.EnsureSuccessStatusCode();
        }

        public async Task<bool> ChangePasswordAsync(string userName, string currentPassword, string newPassword)
        {
            var passwords = System.Text.Json.JsonSerializer.Serialize(new string[]{currentPassword, newPassword});
            var httpContent = new StringContent(passwords, Encoding.UTF8, "application/json");

            var result = await _httpClient.PutAsync($"api/Identity/ChangePassword/{userName}", httpContent);
            return result.StatusCode == HttpStatusCode.OK;
        }
    }
}

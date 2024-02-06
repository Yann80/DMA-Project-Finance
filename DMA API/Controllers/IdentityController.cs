using Core.DTO;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DMA.Controllers
{
    /// <summary>
    /// API controller for user administration
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<BackOfficeUser> _userManager;
        private readonly SignInManager<BackOfficeUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Controller constructor for dependency injection
        /// </summary>
        /// <param name="userManager">User manager</param>
        /// <param name="signInManager">SignIn manager</param>
        /// <param name="roleManager">Role manager</param>
        public IdentityController(UserManager<BackOfficeUser> userManager, SignInManager<BackOfficeUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="userCredentials">DTO for the backoffice user</param>
        /// <param name="rememberMe">Indicates if login is persistent, by default is true</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] BackOfficeUserDTO userCredentials, bool rememberMe = true)
        {
            var user = await _userManager.FindByNameAsync(userCredentials.Username);

            if (user == null)
            {
                return NotFound();
            }

            var result = _signInManager.PasswordSignInAsync(userCredentials.Username, userCredentials.Password, rememberMe, lockoutOnFailure: false).Result;

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            userCredentials.Role = userRoles.SingleOrDefault();

            return Ok(userCredentials);
        }

        [HttpPut]
        [Route("ChangePassword/{userName}")]
        public async Task<IActionResult> ChangePassword(string userName,[FromBody] string[] passwords)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ChangePasswordAsync(user, passwords[0], passwords[1]);

            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            return Ok();
        }

        /// <summary>
        /// Returns a list of users from the store
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUsers")]
        public async Task<List<BackOfficeUserDTO>> GetUsers()
        {
            var listOfUsersDTO = new List<BackOfficeUserDTO>();

            foreach (var user in _userManager.Users.OrderBy(usr => usr.UserName).ToList())
            {
                var role = await _userManager.GetRolesAsync(user);
                listOfUsersDTO.Add(new BackOfficeUserDTO()
                {
                    Id = user.Id,
                    Username = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Role = role.Count > 0 ? role[0] : string.Empty
                });
            }

            return listOfUsersDTO;
        }

        /// <summary>
        /// Get a user from the store
        /// </summary>
        /// <param name="Id">User Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUser/{Id}")]
        public async Task<BackOfficeUserDTO> GetUser(string Id)
        {
            BackOfficeUserDTO userDTO = null;

            var user = _userManager.Users.Where(user => user.Id == Id).SingleOrDefault();

            if (user != null)
            {
                userDTO = new BackOfficeUserDTO()
                {
                    Id = user.Id,
                    Username = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Password = string.Empty,
                    Role = string.Join(';', await _userManager.GetRolesAsync(user))
                };
            }

            return userDTO;
        }

        /// <summary>
        /// Delete a user from the store
        /// </summary>
        /// <param name="Id">User Id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteUser/{Id}")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles != null && roles.Count > 0)
            {
                await _userManager.RemoveFromRolesAsync(user, roles);
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return NoContent();
        }

        /// <summary>
        /// Create and save a new user in the store
        /// </summary>
        /// <param name="backOfficeUserDTO">DTO for backoffice user</param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUserAsync(BackOfficeUserDTO backOfficeUserDTO)
        {
            var user = new BackOfficeUser { UserName = backOfficeUserDTO.Username, Email = backOfficeUserDTO.Email };

            var passwordHasher = new PasswordHasher<BackOfficeUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, backOfficeUserDTO.Password);

            var resultCreateUser = await _userManager.CreateAsync(user);
            if (!resultCreateUser.Succeeded)
            {
                return BadRequest();
            }

            var resultAddToRole = await _userManager.AddToRoleAsync(user, backOfficeUserDTO.Role);
            if (!resultAddToRole.Succeeded)
            {
                return BadRequest();
            }

            return Ok(user);
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="Id">User Id</param>
        /// <param name="backOfficeUserDTO">DTO for backoffice user</param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateUser/{Id}")]
        public async Task<IActionResult> UpdateUserAsync(string Id, [FromBody] BackOfficeUserDTO backOfficeUserDTO)
        {
            var userToUpdate = _userManager.Users.SingleOrDefault(user => user.Id == Id);
            if (userToUpdate == null)
            {
                return NotFound();
            }

            if(!string.IsNullOrEmpty(backOfficeUserDTO.Email))
            {
                var code = await _userManager.GenerateChangeEmailTokenAsync(userToUpdate, backOfficeUserDTO.Email);
                await _userManager.ChangeEmailAsync(userToUpdate, backOfficeUserDTO.Email,code);
            }

            var currentRoles = await _userManager.GetRolesAsync(userToUpdate);
            if ((currentRoles.Count > 0 && currentRoles[0] != backOfficeUserDTO.Role) || currentRoles.Count == 0)
            {
                var newRole = await _roleManager.FindByNameAsync(backOfficeUserDTO.Role);

                if (currentRoles.Count > 0)
                {
                    await _userManager.RemoveFromRoleAsync(userToUpdate, currentRoles[0]);
                }

                var result = await _userManager.AddToRoleAsync(userToUpdate, newRole.NormalizedName);
                if (!result.Succeeded)
                {
                    return BadRequest();
                }
            }

            if (!string.IsNullOrEmpty(backOfficeUserDTO.Password))
            {
                var passwordHasher = new PasswordHasher<BackOfficeUser>();
                userToUpdate.PasswordHash = passwordHasher.HashPassword(userToUpdate, backOfficeUserDTO.Password);

                var result = await _userManager.UpdateAsync(userToUpdate);
                if (!result.Succeeded)
                {
                    return BadRequest();
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Returns a list of roles from the store
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRoles")]
        public List<IdentityRole> GetRoles()
        {
            return _roleManager.Roles.OrderBy(r => r.Name).ToList();
        }

        /// <summary>
        /// Create and save a new role in the store
        /// </summary>
        /// <param name="identityRole">IdentityRole object</param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole(IdentityRole identityRole)
        {
            var roleExist = await _roleManager.RoleExistsAsync(identityRole.Name);
            if (roleExist)
            {
                return NoContent();
            }

            var result = await _roleManager.CreateAsync(identityRole);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        /// <summary>
        /// Get the specified role from the store
        /// </summary>
        /// <param name="Id">Identity role Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRole/{Id}")]
        public async Task<IActionResult> GetRole(string Id)
        {
            return Ok(_roleManager.Roles.Where(role => role.Id == Id).SingleOrDefault());
        }

        /// <summary>
        /// Delete the specified role from the store
        /// </summary>
        /// <param name="Id">Identity role Id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteRole/{Id}")]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var roleToDelete = _roleManager.Roles.SingleOrDefault(role => role.Id == Id);
            if (roleToDelete == null)
            {
                return NotFound();
            }

            await _roleManager.DeleteAsync(roleToDelete);

            return NoContent();
        }

        /// <summary>
        /// Update a given role in the store
        /// </summary>
        /// <param name="Id">Identity role Id</param>
        /// <param name="identityRole">Identity object</param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateRole/{Id}")]
        public async Task<IActionResult> UpdateRole(string Id, [FromBody] IdentityRole identityRole)
        {
            var roleToUpdate = _roleManager.Roles.SingleOrDefault(role => role.Id == Id);
            if (roleToUpdate == null)
            {
                return NotFound();
            }

            roleToUpdate.Name = identityRole.Name;

            await _roleManager.UpdateAsync(roleToUpdate);

            return NoContent();
        }


        [HttpGet]
        [Route("UserInfo")]
        public BackOfficeUserDTO UserInfo()
        {
            //var user = await _userManager.GetUserAsync(HttpContext.User);
            return new BackOfficeUserDTO
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                Username = User.Identity.Name,
                Email = User.FindAll(ClaimTypes.Email).Select(r => r.Value).FirstOrDefault(),
                Role = User.FindAll(ClaimTypes.Role).Select(r => r.Value).FirstOrDefault()
        };
        }

        /// <summary>
        /// Sign out the current user
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SignOut")]
        public async Task<IActionResult> SignOutAsync()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}

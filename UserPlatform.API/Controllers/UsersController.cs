using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserPlatform.API.Helpers.Interfaces;
using UserPlatform.API.Models;
using UserPlatform.Common.Enums;
using UserPlatform.Common.Models;
using UserPlatform.Domain.Entities;

namespace UserPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;

        public UsersController(IUserHelper userHelper, IConfiguration configuration)
        {
            _userHelper = userHelper;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Data = ModelState
                });
            }

            User user = await _userHelper.GetUserAsync(request.Username);
            if (user == null)
            {
                return NotFound(new ResponseViewModel
                {
                    IsSuccess = false,
                    Message = "User not exist"
                });
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.ValidatePasswordAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new ResponseViewModel
                {
                    IsSuccess = false,
                    Message = "User or password incorrect"
                });
            }

            return Ok(new ResponseViewModel
            {
                IsSuccess = true,
                Message = "Successful log-in",
                //Data = GetToken(user)
            });

        }

        //private object GetToken(User user)
        //{
        //    Claim[] claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    };

        //    SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
        //    SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    JwtSecurityToken token = new JwtSecurityToken(
        //        _configuration["Tokens:Issuer"],
        //        _configuration["Tokens:Audience"],
        //        claims,
        //        expires: DateTime.UtcNow.AddDays(99),
        //        signingCredentials: credentials);

        //    return new
        //    {
        //        token = new JwtSecurityTokenHandler().WriteToken(token),
        //        expiration = token.ValidTo,
        //        user
        //    };
        //}


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> PostUser([FromBody] UserViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseViewModel
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Data = ModelState
                });
            }

            User user = await _userHelper.GetUserAsync(request.Email);
            if (user != null)
            {
                return BadRequest(new ResponseViewModel
                {
                    IsSuccess = false,
                    Message = "User Exist"
                });
            }

            user = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email,
                UserType = UserType.Operativo
            };

            IdentityResult result = await _userHelper.AddUserAsync(user, request.Password);
            if (result != IdentityResult.Success)
            {
                return BadRequest(new ResponseViewModel
                {
                    IsSuccess = false,
                    Message = result.Errors.FirstOrDefault().Description
                });
            }

            User userNew = await _userHelper.GetUserAsync(request.Email);
            await _userHelper.AddUserToRoleAsync(userNew, user.UserType.ToString());

            return Ok(new ResponseViewModel
            {
                IsSuccess = true,
                Message = "User created",
                Data = user
            });
        }


        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[HttpPut]
        //[Route("Update")]
        //public async Task<IActionResult> PutUser([FromBody] UserEditRequest request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(new ResponseViewModel
        //        {
        //            IsSuccess = false,
        //            Message = "Bad request",
        //            Data = ModelState
        //        });
        //    }

        //    string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
        //    User user = await _userHelper.GetUserAsync(email);
        //    if (user == null)
        //    {
        //        return BadRequest(new ResponseViewModel
        //        {
        //            IsSuccess = false,
        //            Message = "User Does not exist"
        //        });
        //    }

        //    user.FirstName = request.FirstName;
        //    user.LastName = request.LastName;

        //    IdentityResult result = await _userHelper.UpdateUserAsync(user);
        //    if (!result.Succeeded)
        //    {
        //        return BadRequest(new ResponseViewModel
        //        {
        //            IsSuccess = false,
        //            Message = result.Errors.FirstOrDefault().Description
        //        });
        //    }

        //    User updatedUser = await _userHelper.GetUserAsync(email);
        //    return Ok(new ResponseViewModel
        //    {
        //        IsSuccess = true,
        //        Message = "User updated",
        //        Data = GetToken(updatedUser)
        //    });
        //}

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[HttpPost]
        //[Route("ChangePassword")]
        //public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(new ResponseViewModel
        //        {
        //            IsSuccess = false,
        //            Message = "Bad request",
        //            Data = ModelState
        //        });
        //    }

        //    string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
        //    User user = await _userHelper.GetUserAsync(email);
        //    if (user == null)
        //    {
        //        return NotFound(new Response
        //        {
        //            IsSuccess = false,
        //            Message = "User Does not exist"
        //        });
        //    }

        //    IdentityResult result = await _userHelper.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        //    if (!result.Succeeded)
        //    {
        //        return BadRequest(new Response
        //        {
        //            IsSuccess = false,
        //            Message = "Error changing password"
        //        });
        //    }

        //    return Ok(new Response { IsSuccess = true, Message = "Password Changed Successfully" });
        //}
    }
}
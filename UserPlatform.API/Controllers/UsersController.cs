using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserPlatform.API.Services.Interfaces;
using UserPlatform.Domain.Models;

namespace UserPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("/Users/GetAll")]
        public async Task<ActionResult> GetUsers()
        {
            ResponseViewModel result = await _userService.GetAll();
            if (!result.IsSuccess)
            {
                return result.IsException ?
                    BadRequest(result.Message) : NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpGet("/Users/GetById/{id}")]
        public async Task<ActionResult> GetUser(string id)
        {
            ResponseViewModel result = await _userService.GetById(id);
            if (!result.IsSuccess)
            {
                return result.IsException ?
                    BadRequest(result.Message) : NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPost("/Users/Create")]
        public async Task<ActionResult> PostUser(UserViewModelCreate model)
        {
            ResponseViewModel result = await _userService.Create(model);
            if (!result.IsSuccess)
            {
                return result.IsException ?
                    BadRequest(result.Message) : NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpPut("/Users/Update/{id}")]
        public async Task<IActionResult> PutUser(UserViewModel model, string id)
        {
            ResponseViewModel result = await _userService.Update(id, model);
            if (!result.IsSuccess)
            {
                return result.IsException ?
                    BadRequest(result.Message) : NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpDelete("/Users/DeleteTicket/{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            ResponseViewModel result = await _userService.Delete(id);
            if (!result.IsSuccess)
            {
                return result.IsException ?
                    BadRequest(result.Message) : NotFound(result.Message);
            }

            return Ok(result.Message);
        }

        [HttpPost("/Users/LogIn")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ResponseViewModel result = await _userService.LogIn(model);
            if (!result.IsSuccess)
            {
                return result.IsException ?
                    BadRequest(result.Message) : NotFound(result.Message);
            }

            return Ok(result.Data);
        }

        [HttpGet("/Users/LogOut")]
        public async Task<ActionResult> LogOut()
        {
            ResponseViewModel result = await _userService.LogOut();
            if (!result.IsSuccess)
            {
                return result.IsException ?
                    BadRequest(result.Message) : NotFound(result.Message);
            }

            return Ok(result.Message);
        }
    }
}
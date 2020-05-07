using Backend_Dis_App.BaseClasses;
using Backend_Dis_App.Models;
using Backend_Dis_App.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Dis_App.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult LoginIntoAccount([FromBody] LoginModel loginModel)
        {
            if (_userService.Login(loginModel))
                return Ok(loginModel);
            else
                return Unauthorized("Invalid username or password");
        }
    }
}

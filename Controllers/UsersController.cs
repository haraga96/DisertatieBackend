using System;
using Backend_Dis_App.BaseClasses;
using Backend_Dis_App.Models;
using Backend_Dis_App.Services.Interfaces;
using Backend_Dis_App.Validators;
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
        private readonly IEmailValidator _emailValidator;
        private readonly IPasswordValidator _passwordValidator;

        private string ErrorMessage { get; set; }

        public UsersController(IUserService userService, IEmailValidator emailValidator, IPasswordValidator passwordValidator)
        {
            _userService = userService;
            _emailValidator = emailValidator;
            _passwordValidator = passwordValidator;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult LoginIntoAccount([FromBody] LoginModel loginModel)
        {
            var isValid = true;
            if (!_emailValidator.CheckRule(loginModel.EmailAddress))
            {
                ErrorMessage = "*Email invalid format.";
                isValid = false;
            }
            if (!_passwordValidator.CheckRule(loginModel.Password))
            {
                ErrorMessage += "*Password invalid format. It must contains 8 characters, at least one letter and one digit.";
                isValid = false;
            }
            if (isValid)
                return Ok(loginModel);
            else
                return BadRequest(ErrorMessage);
        }
    }
}

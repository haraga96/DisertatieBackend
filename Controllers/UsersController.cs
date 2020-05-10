﻿using System;
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
    [Route("api/[controller]/")]
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
        [Route("login")]
        public IActionResult LoginIntoAccount([FromBody] LoginModel user)
        {
            var isValid = true;
            if (!_emailValidator.CheckRule(user.EmailAddress))
            {
                ErrorMessage = "*Email invalid format.";
                isValid = false;
            }
            if (!_passwordValidator.CheckRule(user.Password))
            {
                ErrorMessage += "*Password invalid format. It must contains 8 characters, at least one letter and one digit.";
                isValid = false;
            }
            if (isValid)
                return Ok(user);
            else
                return BadRequest(ErrorMessage);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("forgot")]
        public IActionResult ForgotPassword([FromBody] User user)
        {
            if (!_emailValidator.CheckRule(user.EmailAddress))
            {
                ErrorMessage = "*Email invalid format.";
                return BadRequest(ErrorMessage);
            }
            return Ok("Email has been sent");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        public IActionResult CreateAccount([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.FirstName)|| string.IsNullOrWhiteSpace(user.LastName)
                || string.IsNullOrWhiteSpace(user.EmailAddress)|| string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                ErrorMessage = "*Please fill all required fields.";
                return BadRequest(ErrorMessage);
            }
            return Ok("Account created.");
        }
    }
}

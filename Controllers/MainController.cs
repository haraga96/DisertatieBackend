﻿using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Backend_Dis_App.BaseClasses;
using Backend_Dis_App.Database;
using Backend_Dis_App.Models;
using Backend_Dis_App.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Dis_App.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/")]
    public class MainController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IDocumentsService _documentsService;
        private readonly TaxAppContext _db;
        private User _user;

        public MainController(IUserService userService, IDocumentsService documentsService)
        {
            _db = new TaxAppContext();
            _userService = userService;
            _documentsService = documentsService;
        }

        
        [HttpGet]
        [Route("mainpageuser")]
        public async Task<IActionResult> GetInfosUser()
        {
            //decode
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accessToken);
            var tokenS = handler.ReadToken(accessToken) as JwtSecurityToken;
            var emailAddress = tokenS.Claims.FirstOrDefault(claim => claim.Type == "email").Value;

            //search in db
            _user = await _userService.GetUserByEmail(emailAddress);
            return Ok(_user);
        }
    }
}
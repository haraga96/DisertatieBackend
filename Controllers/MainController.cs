using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Backend_Dis_App.BaseClasses;
using Backend_Dis_App.Database;
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
        private readonly TaxAppContext _db;

        public MainController(IUserService userService)
        {
            _db = new TaxAppContext();
            _userService = userService;
        }

        
        [HttpGet]
        [Route("mainpage")]
        public async Task<IActionResult> GetInfos()
        {
            //decode
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accessToken);
            var tokenS = handler.ReadToken(accessToken) as JwtSecurityToken;
            var emailAddress = tokenS.Claims.FirstOrDefault(claim => claim.Type == "email").Value;

            //search in db
            var user = await _userService.GetUserByEmail(emailAddress);
            //user.Documents = await _userService.GetDocumentsByUserId(user);
            return Ok(user);
        }
    }
}
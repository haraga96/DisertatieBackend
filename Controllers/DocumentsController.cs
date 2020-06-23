using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Backend_Dis_App.BaseClasses;
using Backend_Dis_App.Database;
using Backend_Dis_App.Models;
using Backend_Dis_App.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_Dis_App.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/")]
    public class DocumentsController : BaseController
    { 
        private readonly IDocumentsService _documentsService;
        private readonly TaxAppContext _db;
        private User _user;

        public DocumentsController(IDocumentsService documentsService)
        {
            _db = new TaxAppContext();
            _documentsService = documentsService;
        }

        [HttpGet]
        [Route("infos")]
        public async Task<IActionResult> GetInfosDocuments()
        {
            //decode
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accessToken);
            var tokenS = handler.ReadToken(accessToken) as JwtSecurityToken;
            var emailAddress = tokenS.Claims.FirstOrDefault(claim => claim.Type == "email").Value;

            //search in db
            _user = await _db.User.FirstOrDefaultAsync(x => x.EmailAddress.Equals(emailAddress));
            var documents = await _documentsService.GetDocumentsByUserId(_user);
            return Ok(documents);
        }
    }
}
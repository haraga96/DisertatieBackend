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
    public class ComputeController : BaseController
    {
        private readonly IDocumentsService _documentsService;
        private readonly IEmailService _emailService;
        private readonly TaxAppContext _db;
        private User _user;

        public ComputeController(IEmailService emailService, IDocumentsService documentsService)
        {
            _db = new TaxAppContext();
            _documentsService = documentsService;
            _emailService = emailService;
        }

        [HttpGet]
        [Route("sendsum")]
        public async Task<IActionResult> SendSum([FromQuery] string emailAddress)
        {

            //search in db
            _user = await _db.User.FirstOrDefaultAsync(x => x.EmailAddress.Equals(emailAddress));
            var documents = await _documentsService.GetDocumentsByUserId(_user);
            int sum = 0;
            foreach(var document in documents)
            {
                sum += document.ValueDue;
            }
            await _emailService.SendComputeAsync(emailAddress, "Total Due", sum);
            return Ok(sum);
        }
    }
}
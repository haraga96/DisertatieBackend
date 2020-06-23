using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend_Dis_App.Database;
using Backend_Dis_App.Models;
using Backend_Dis_App.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Dis_App.Services.Implementation
{
    public class DocumentsService : IDocumentsService
    {
        private readonly TaxAppContext _db;

        public DocumentsService()
        {
            _db = new TaxAppContext();
        }

        public async Task<ICollection<Documents>> GetDocumentsByUserId(User user)
        {
            var list = await _db.Documents.Where(x => x.UserId == user.Id).ToListAsync();
            return list;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend_Dis_App.Database;
using Backend_Dis_App.Models;
using Backend_Dis_App.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_Dis_App.Services.Implementation
{
    public class CountryService : ICountryService
    {
        private TaxAppContext _db;

        public CountryService()
        {
            _db = new TaxAppContext();
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            var countries = await _db.Country.ToListAsync();
            return countries;
        }
    }
}

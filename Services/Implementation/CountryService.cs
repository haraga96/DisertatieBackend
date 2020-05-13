using System;
using System.Threading.Tasks;
using Backend_Dis_App.Database;
using Backend_Dis_App.Models;
using Backend_Dis_App.Services.Interfaces;

namespace Backend_Dis_App.Services.Implementation
{
    public class CountryService : ICountryService
    {
        private TaxAppContext _db;

        public CountryService()
        {
            _db = new TaxAppContext();
        }

        public async Task<Country> GetAllCountriesAsync()
        {
            var countries = await _db.Country.FindAsync();
            return countries;
        }
    }
}

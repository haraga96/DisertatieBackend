using System.Threading.Tasks;
using Backend_Dis_App.Models;

namespace Backend_Dis_App.Services.Interfaces
{
    public interface ICountryService
    {
        Task<Country> GetAllCountriesAsync();
    }
}

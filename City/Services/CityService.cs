using Microsoft.EntityFrameworkCore;

namespace City.Services
{
    public class CityService : ICityService
    {
        private readonly MainDbContext _context;
        public CityService(MainDbContext context)
        {
            _context = context;
        }
        public async Task<List<City.Models.City>> AddCity(City.Models.City city)
        {
            _context.Cites.Add(city);
            _context.SaveChanges();
            return await _context.Cites.ToListAsync();
        }
        public async Task<List<City.Models.City>> GetAllCity()
        {
            return await _context.Cites.ToListAsync();
        }
    }
}

namespace City.Services
{
    public interface ICityService
    {
        Task<List<City.Models.City>> AddCity(City.Models.City city);
        Task<List<City.Models.City>> GetAllCity();
    }
}

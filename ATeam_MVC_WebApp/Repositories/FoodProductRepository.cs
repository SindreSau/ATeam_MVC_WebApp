using ATeam_MVC_WebApp.Interfaces;

namespace ATeam_MVC_WebApp.Repositories;

public class FoodProductRepository : IFoodProductRepository
{
    public async Task<IEnumerable<FoodProduct>> GetFoodProductsAsync(int pageNumber, int pageSize, string orderBy, bool nokkelhull)
    {
        throw new NotImplementedException();
    }

    public async Task<FoodProduct> GetFoodProductAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<FoodProduct> AddFoodProductAsync(FoodProduct foodProduct)
    {
        throw new NotImplementedException();
    }

    public async Task<FoodProduct> UpdateFoodProductAsync(FoodProduct foodProduct)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteFoodProductAsync(int id)
    {
        throw new NotImplementedException();
    }
}
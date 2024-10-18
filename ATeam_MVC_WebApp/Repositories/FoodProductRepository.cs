using ATeam_MVC_WebApp.Data;
using ATeam_MVC_WebApp.Interfaces;
using ATeam_MVC_WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ATeam_MVC_WebApp.Repositories;

public class FoodProductRepository : IFoodProductRepository
{
    private readonly ApplicationDbContext _context;

    public FoodProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<FoodProduct>> GetFoodProductsAsync(int pageNumber, int pageSize, string orderBy, bool nokkelhull)
    {
        var query = _context.FoodProducts.AsQueryable();

        query = query.Where(fp => fp.NokkelhullQualified == nokkelhull);

        switch (orderBy.ToLower())
        {
            case "productname":
                query = query.OrderBy(fp => fp.ProductName);
                break;
            case "energykcal":
                query = query.OrderBy(fp => fp.EnergyKcal);
                break;
            case "fat":
                query = query.OrderBy(fp => fp.Fat);
                break;
            case "carbohydrates":
                query = query.OrderBy(fp => fp.Carbohydrates);
                break;
            case "protein":
                query = query.OrderBy(fp => fp.Protein);
                break;
            case "fiber":
                query = query.OrderBy(fp => fp.Fiber);
                break;
            case "salt":
                query = query.OrderBy(fp => fp.Salt);
                break;
            default:
                query = query.OrderBy(fp => fp.Id);
                break;
        }

        query = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
        
        return await query.ToListAsync();
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
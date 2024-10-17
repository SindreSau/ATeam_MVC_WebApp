namespace ATeam_MVC_WebApp.Interfaces;

public interface IFoodProductRepository
{
    // get all food products with pagination and sorting, and filtering by nokkelhull
    Task<IEnumerable<FoodProduct>> GetFoodProductsAsync(int pageNumber, int pageSize, string orderBy, bool nokkelhull);

    // get a food product by id
    Task<FoodProduct> GetFoodProductAsync(int id);

    // add a food product
    Task<FoodProduct> AddFoodProductAsync(FoodProduct foodProduct);

    // update a food product
    Task<FoodProduct> UpdateFoodProductAsync(FoodProduct foodProduct);

    // delete a food product
    Task<bool> DeleteFoodProductAsync(int id);
}
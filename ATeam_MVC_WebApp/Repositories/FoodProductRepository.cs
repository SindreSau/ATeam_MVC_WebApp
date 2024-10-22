using ATeam_MVC_WebApp.Data;
using ATeam_MVC_WebApp.Interfaces;
using ATeam_MVC_WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ATeam_MVC_WebApp.Repositories
{
    // Repository class for managing food products
    public class FoodProductRepository : IFoodProductRepository
    {
        private readonly ApplicationDbContext _context;

        // Constructor that initializes the repository with the application database context
        public FoodProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Asynchronously retrieves a paginated list of food products based on filter and sort criteria
        public async Task<IEnumerable<FoodProduct>> GetFoodProductsAsync(int pageNumber, int pageSize, string orderBy, bool? nokkelhull)
        {
            // Create a queryable collection of food products
            var query = _context.FoodProducts.AsQueryable();

            // Filter products based on whether they are Nokkelhull qualified
            if (nokkelhull != null)
            {
                query = query.Where(fp => fp.NokkelhullQualified == nokkelhull);
            }

            // Apply sorting based on the specified order
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
                    query = query.OrderBy(fp => fp.FoodProductId); // Default ordering by Id
                    break;
            }

            // Apply pagination
            query = query
                .Skip((pageNumber - 1) * pageSize) // Skip the previous pages
                .Take(pageSize); // Take the specified number of records for the current page
            
            // Execute the query and return the list of food products
            return await query.ToListAsync();
        }

        // Asynchronously retrieves a specific food product by its ID
        public async Task<FoodProduct> GetFoodProductAsync(int id)
        {
            // Find and return the food product by ID
            var foodProduct = await _context.FoodProducts.FindAsync(id);
            return foodProduct ?? throw new KeyNotFoundException($"FoodProduct with ID {id} not found.");
        }

        // Asynchronously adds a new food product to the database
        public async Task<FoodProduct> AddFoodProductAsync(FoodProduct foodProduct)
        {
            // Add the new food product to the context
            _context.FoodProducts.Add(foodProduct);
            // Save changes to the database
            await _context.SaveChangesAsync();
            return foodProduct; // Return the added product
        }

        // Asynchronously updates an existing food product in the database
        public async Task<FoodProduct> UpdateFoodProductAsync(FoodProduct foodProduct)
        {
            var existingFoodProduct = await _context.FoodProducts.FindAsync(foodProduct.FoodProductId);
            if (existingFoodProduct == null)
            {
                throw new KeyNotFoundException($"FoodCategory with ID {foodProduct.FoodProductId} not found. Could not update.");
            }

            existingFoodProduct.ProductName = foodProduct.ProductName;
            _context.FoodProducts.Update(existingFoodProduct);
            await _context.SaveChangesAsync();
            return existingFoodProduct;
        }

        // Asynchronously deletes a food product by its ID
        public async Task<bool> DeleteFoodProductAsync(int id)
        {
            // Find the food product by ID
            var foodProduct = await _context.FoodProducts.FindAsync(id);

            // If the product does not exist, return false
            if (foodProduct == null)
            {
                return false;
            }

            // Remove the product from the context
            _context.FoodProducts.Remove(foodProduct);
            // Save changes to the database
            await _context.SaveChangesAsync();
            return true; // Return true indicating successful deletion
        }
    }
}

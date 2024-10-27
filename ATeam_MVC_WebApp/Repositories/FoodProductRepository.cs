using ATeam_MVC_WebApp.Data;
using ATeam_MVC_WebApp.Interfaces;
using ATeam_MVC_WebApp.Models;
using ATeam_MVC_WebApp.Services;
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
            var query = _context.FoodProducts
            .Include(fp => fp.FoodCategory) // Include related FoodCategory
            .Include(fp => fp.CreatedBy) // Include related CreatedBy
            .AsQueryable();

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

        // Get paginated food products by vendor ID with filter and sort criteria
        public async Task<IEnumerable<FoodProduct>> GetFoodProductsByVendorAsync(string vendorId, int pageNumber, int pageSize, string orderBy, bool? nokkelhull)
        {
            // Create a queryable collection of food products
            var query = _context.FoodProducts
                .Include(fp => fp.FoodCategory)
                .Include(fp => fp.CreatedBy)
                .Where(fp => fp.CreatedById == vendorId) // Filter by vendor ID
                .AsQueryable();

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

        // Get the count of food products by vendor
        public async Task<int> GetFoodProductsByVendorCountAsync(string vendorId)
        {
            // Count the number of food products by the specified vendor
            return await _context.FoodProducts
                .Where(fp => fp.CreatedById == vendorId)
                .CountAsync();
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
            if (foodProduct == null)
            {
                throw new ArgumentNullException(nameof(foodProduct));
            }

            // Calculate Nokkelhull qualification before saving
            foodProduct.NokkelhullQualified = NutritionCalculatorService.IsNokkelhullQualified(
                (float)foodProduct.EnergyKcal,
                (float)foodProduct.Protein,
                (float)foodProduct.Carbohydrates,
                (float)foodProduct.Fat,
                (float)foodProduct.Fiber,
                (float)foodProduct.Salt
            );

            _context.Add(foodProduct);
            await _context.SaveChangesAsync();
            return foodProduct;
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
        //oskar, gjorde dette får å få til view
        // public async Task<IEnumerable<FoodProduct>> GetAll()
        // {
        //     return await _context.FoodProducts.ToListAsync();
        // }
    }
}

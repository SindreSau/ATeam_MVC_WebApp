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
        private readonly ILogger<FoodProductRepository> _logger;

        // Constructor that initializes the repository with the application database context
        public FoodProductRepository(ApplicationDbContext context, ILogger<FoodProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Asynchronously retrieves a paginated list of food products based on filter and sort criteria
        public async Task<IEnumerable<FoodProduct>> GetFoodProductsAsync(int pageNumber, int pageSize, string orderBy, bool? nokkelhull, string searchTerm = "")
        {
            _logger.LogInformation("Getting food products with parameters: PageNumber={PageNumber}, PageSize={PageSize}, OrderBy={OrderBy}, Nokkelhull={Nokkelhull}, SearchTerm={SearchTerm}",
                pageNumber, pageSize, orderBy, nokkelhull, searchTerm);

            // Create a queryable collection of food products
            var query = _context.FoodProducts
                .Include(fp => fp.FoodCategory)
                .Include(fp => fp.CreatedBy)
                .AsQueryable();

            // Apply search if term provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                _logger.LogInformation("Applying search filter for term: {SearchTerm}", searchTerm);
                searchTerm = searchTerm.ToLower();
                query = query.Where(p =>
                    (p.ProductName != null && EF.Functions.Like(p.ProductName.ToLower(), $"%{searchTerm}%")) ||
                    (p.FoodCategory != null && p.FoodCategory.CategoryName != null &&
                    EF.Functions.Like(p.FoodCategory.CategoryName.ToLower(), $"%{searchTerm}%")) ||
                    (p.CreatedBy != null && p.CreatedBy.UserName != null &&
                    EF.Functions.Like(p.CreatedBy.UserName.ToLower(), $"%{searchTerm}%"))
                );
            }

            // Filter products based on whether they are Nokkelhull qualified
            if (nokkelhull != null)
            {
                _logger.LogInformation("Applying Nokkelhull filter: {Nokkelhull}", nokkelhull);
                query = query.Where(fp => fp.NokkelhullQualified == nokkelhull);
            }

            // Apply sorting based on the specified order
            switch (orderBy.ToLower())
            {
                case "productname":
                    query = query.OrderBy(fp => fp.ProductName ?? "");  // Null coalescing
                    break;
                case "categoryname":
                    query = query.OrderBy(fp => fp.FoodCategory!.CategoryName)
                        .ThenBy(fp => fp.ProductName);
                    break;
                default:
                    query = query.OrderBy(fp => fp.CreatedAt); // Default ordering by CreatedAt
                    break;
            }

            // Log the SQL query before pagination
            var sqlBeforePaging = query.ToQueryString();
            _logger.LogInformation("Generated SQL before pagination: {Sql}", sqlBeforePaging);

            // Apply pagination
            query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            // Log final SQL
            var finalSql = query.ToQueryString();
            _logger.LogInformation("Final SQL with pagination: {Sql}", finalSql);

            // Execute the query and return the list of food products
            var results = await query.ToListAsync();
            _logger.LogInformation("Query returned {Count} results", results.Count);

            return results;
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

        public async Task<int> GetFoodProductsCountAsync(string searchTerm = "", bool? nokkelhull = null)
        {
            var query = _context.FoodProducts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(p =>
                    (p.ProductName != null && EF.Functions.Like(p.ProductName.ToLower(), $"%{searchTerm}%")) ||
                    (p.FoodCategory != null && p.FoodCategory.CategoryName != null &&
                    EF.Functions.Like(p.FoodCategory.CategoryName.ToLower(), $"%{searchTerm}%")) ||
                    (p.CreatedBy != null && p.CreatedBy.UserName != null &&
                    EF.Functions.Like(p.CreatedBy.UserName.ToLower(), $"%{searchTerm}%"))
                );
            }

            if (nokkelhull.HasValue)
            {
                query = query.Where(p => p.NokkelhullQualified == nokkelhull.Value);
            }

            return await query.CountAsync();
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

    }
}

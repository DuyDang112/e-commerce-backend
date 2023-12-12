
using Product.Api.Entity;
using ILogger = Serilog.ILogger;

namespace Product.Api.Persistence;

    public class ProductContextSeed
    {
        public static async Task SeedProductAsync(ProductContext context, ILogger logger)
        {
            if (!context.products.Any())
            {
                context.AddRange(getCatalogProduct());
                await context.SaveChangesAsync();
                logger.Information("Seed Data for Product DB associated with context {DbcontextName}", nameof(ProductContext));
            }
        }

        private static IEnumerable<ProductCatalog> getCatalogProduct()
        {
        return new List<ProductCatalog>()
        {
            new ()
            { 
                No = "nước uống",
                Name = "Cà Phê",
                Summary = "cà phê thơm ngon",
                Description = "Description",
                Price = (decimal)22.500
            },
            new ()
            {
                No = "thức ăn",
                Name = "bún bò",
                Summary = "bún bò huế",
                Description = "Description",
                Price = (decimal)35.000
            },
        };
        }
    }


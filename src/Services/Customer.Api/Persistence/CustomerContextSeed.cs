using Microsoft.EntityFrameworkCore;
namespace Customer.Api.Persistence;

public static class CustomerContextSeed
{
    public static  IHost SeedCustomerData(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var customerContext = scope.ServiceProvider.GetRequiredService<CustomerContext>();
        customerContext.Database.MigrateAsync().GetAwaiter().GetResult();

        CreatedCustomer(customerContext, "DuyDang", "Dang", "Vo", "Dangvo@112gmail.com").GetAwaiter().GetResult();
        CreatedCustomer(customerContext, "NguyenVanA", "A", "NguyenVan", "Vannguyen@112gmail.com").GetAwaiter().GetResult();
        return host;
    }

    private static async Task CreatedCustomer(CustomerContext customerContext,string UserName, string FirstName,string LastName, string Email)
    {
        var customer = await customerContext.customers.SingleOrDefaultAsync(x => x.UserName.Equals(UserName) || x.Email.Equals(Email));
        if (customer == null)
        {
            var newCustomer = new Entities.Customer
            {
                UserName = UserName,
                FirrtName = FirstName,
                LastName = LastName,
                Email = Email
            };

            await customerContext.customers.AddAsync(newCustomer);
            await customerContext.SaveChangesAsync();
        }
    }
              
}


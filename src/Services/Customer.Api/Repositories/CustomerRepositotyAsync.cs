using Contract.Common.Interfaces;
using Customer.Api.Persistence;
using Customer.Api.Reoisitories.Interfaces;
using Infrastructures.Common;
using Microsoft.EntityFrameworkCore;

namespace Customer.Api.Reoisitories
{
    public class CustomerRepositotyAsync : RepositoryBaseAsync<Entities.Customer, int, CustomerContext> , ICustomerRepository
    {
        public CustomerRepositotyAsync(CustomerContext context, IUnitOfWork<CustomerContext> unitOfWork) : base(context, unitOfWork)
        {
        }

        public async Task<Entities.Customer?> GetCustomerByUserNameAsync(string userName) => 
           await FindByCondition(x => x.UserName.Equals(userName))
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Entities.Customer>> GetCustomersAsync() =>
            await FindAll().ToListAsync();
    }
}

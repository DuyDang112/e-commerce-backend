using Contract.Common.Interfaces;
using Customer.Api.Entities;
using Customer.Api.Persistence;

namespace Customer.Api.Reoisitories.Interfaces
{
    public interface ICustomerRepository : IRepositoryBaseAsync<Entities.Customer,int,CustomerContext>
    {
        Task<Entities.Customer?> GetCustomerByUserNameAsync(string userName);
        Task<IEnumerable<Entities.Customer>> GetCustomersAsync();
    }
}

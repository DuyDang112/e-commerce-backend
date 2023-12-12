using AutoMapper;
using Customer.Api.Reoisitories.Interfaces;
using Customer.Api.Services.Interfaces;
using Shared.DTOs.Customer;

namespace Customer.Api.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<IResult> GetCustomerByUserNameAsync(string userName)
        {
            var entity = await _customerRepository.GetCustomerByUserNameAsync(userName);
            if (entity == null) return Results.NotFound();

            var result = _mapper.Map<CustomerDto>(entity);
            return Results.Ok(result);
        }

        public async Task<IResult> GetCustomersAsync() => 
            Results.Ok(await _customerRepository.GetCustomersAsync());
    }
}

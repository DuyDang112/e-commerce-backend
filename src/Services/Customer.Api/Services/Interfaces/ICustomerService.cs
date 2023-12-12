namespace Customer.Api.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<IResult> GetCustomerByUserNameAsync(string userName);
        public Task<IResult> GetCustomersAsync();
    }
}

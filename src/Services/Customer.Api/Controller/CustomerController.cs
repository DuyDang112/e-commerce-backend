using AutoMapper;
using Customer.Api.Reoisitories.Interfaces;
using Customer.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Customer;

namespace Customer.Api.Controller
{
    public static class CustomerController
    {
        public static void MapCustomerApi(this WebApplication app)
        {
            var mapper = app.Services.GetRequiredService<IMapper>();

            app.MapGet("/api/customers",
                async (ICustomerService customerService) => await customerService.GetCustomersAsync());

            app.MapGet("/api/customers/{username}",
                async (string username, ICustomerService customerService) => await customerService.GetCustomerByUserNameAsync(username));

            app.MapPost("/api/customer",
                async (Customer.Api.Entities.Customer customer, ICustomerRepository customerRepository) =>
                {
                    await customerRepository.CreateAsync(customer);
                    await customerRepository.SaveChangesAsync();

                    return Results.Ok(customer);
                });

            app.MapDelete("/api/customer/{id}",
               async (int id, ICustomerRepository customerRepository) =>
               {
                   var customer = await customerRepository.FindByCondition(x => x.Id.Equals(id)).SingleOrDefaultAsync();
                   if (customer == null) return Results.NotFound();

                   await customerRepository.DeleteAsync(customer);
                   await customerRepository.SaveChangesAsync();

                   return Results.NoContent();
               });

            app.MapPut("/api/customer/{id}",
               async (int id, CustomerDto customerDto, ICustomerRepository customerRepository) =>
               {
                   var customer = await customerRepository.FindByCondition(x => x.Id.Equals(id)).SingleOrDefaultAsync();
                   if (customer == null) return Results.NotFound();

                   var updateCustomer = mapper.Map(customerDto, customer);
                   await customerRepository.UpdateAsync(updateCustomer);
                   await customerRepository.SaveChangesAsync();

                   var result = mapper.Map<CustomerDto>(customer);
                   return Results.Ok(result);
               });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Serilog;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        private readonly OrderContext _context;
        private readonly ILogger _logger;

        public OrderContextSeed(OrderContext context, ILogger logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if(_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while initialiseing the database");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while seeding the database");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            if (!_context.Orders.Any())
            {
                await _context.Orders.AddRangeAsync(
                    new Order
                    {
                        UserName = "customer1", FirstName = "Customer1", LastName = "customer1",
                        EmailAddress = "customer1@gmail.com",
                        PhoneNumber = "0372998596",
                        ShippingAddress = "ho chi minh city",
                        InvoiceAddress = "VietNam",
                        TotalPrice = 300,

                    },
                    new Order
                    {
                        UserName = "customer2",
                        FirstName = "customer2",
                        LastName = "customer2",
                        EmailAddress = "customer2@gmail.com",
                        PhoneNumber = "0372998596",
                        ShippingAddress = "ho chi minh city",
                        InvoiceAddress = "VietNam",
                        TotalPrice = 500,
                    }); 
            }
        }
    }
}

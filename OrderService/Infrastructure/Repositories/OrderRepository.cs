using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using OrderService.Domain.Interfaces;

namespace OrderService.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(Order order)
    {
        if (await _context.Orders.AnyAsync(o => o.Id == order.Id))
        {
            return;
        }
        await _context.Orders.AddAsync(order);
    }

    public async Task<Order?> GetByIdAsync(Guid orderId)
    {
        var order = await _context.Orders.Include(order => order.Items)
                                  .FirstOrDefaultAsync(o => o.Id == orderId);
        return order;
    }

    public async Task<List<Order>> GetByUserIdAsync(Guid userId)
    {
        var orders = await _context.Orders.Where(order => order.UserId == userId).ToListAsync();
        return orders;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
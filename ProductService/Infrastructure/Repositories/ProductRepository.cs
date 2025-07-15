using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;

namespace ProductService.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _context;

    public ProductRepository(ProductDbContext context)
    {
        _context = context;
    }
    
    public async Task<Product> GetByIdAsync(Guid id)
    {
        return await _context.Products.FirstOrDefaultAsync(product => product.Id == id) ?? Product.Empty();
    }

    public async Task<List<Product>> GetAllAsync(int page, int pageSize)
    {
        await Task.CompletedTask;
        return _context.Products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task<Product> UpdateAsync(ProductDto product)
    {
        var foundProduct = await GetByIdAsync(product.Id);
        if (foundProduct.IsEmpty())
        {
            throw new KeyNotFoundException();
        }
        foundProduct.Update(product);
        await _context.SaveChangesAsync();
        return foundProduct;
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await GetByIdAsync(id);
        if (!product.IsEmpty())
        {
            _context.Products.Remove(product);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetBulkAsync(IEnumerable<Guid> products)
    {
        return await _context.Products.Where(p => products.Contains(p.Id)).ToListAsync();
    }
}
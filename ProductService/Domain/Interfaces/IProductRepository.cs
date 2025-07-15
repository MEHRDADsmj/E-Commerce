using ProductService.Domain.Entities;

namespace ProductService.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync(int page, int pageSize);
    Task AddAsync(Product product);
    Task<Product> UpdateAsync(ProductDto product);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Product>> GetBulkAsync(IEnumerable<Guid> products);
}
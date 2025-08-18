using Adly.Application.Repositories.Category;
using Adly.Domain.Entities.Ad;
using Adly.Infrastructure.Persistence.Configurations;
using Adly.Infrastructure.Persistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Adly.Infrastructure.Persistence.Repositories;

internal class CategoryRepository(AdlyDbContext db) : BaseRepository<CategoryEntity>(db), ICategoryRepository
{
    public async Task CreateAsync(CategoryEntity category, CancellationToken cancellationToken = default)
    {
        await base.AddAsync(category, cancellationToken);
    }

    public async Task<List<CategoryEntity>> GetCategoriesBasedOnNameAsync(string categoryName, CancellationToken cancellationToken = default)
    {
        return await base.TableAsNoTracking
            .Where(c => c.Name.Contains(categoryName))
            .ToListAsync(cancellationToken);
    }

    public async Task<CategoryEntity?> GetCategoryByIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await base.TableAsNoTracking
            .FirstOrDefaultAsync(c => c.Id.Equals(categoryId), cancellationToken: cancellationToken);
    }
}
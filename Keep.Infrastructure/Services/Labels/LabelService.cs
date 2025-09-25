using Keep.Application.Labels;
using Keep.Domain.Entities;
using Keep.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Keep.Infrastructure.Services.Labels;

public class LabelService : ILabelService
{
    private readonly KeepDbContext _dbContext;

    public LabelService(KeepDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Label>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Labels
            .AsNoTracking()
            .Include(l => l.Notes)
            .ToListAsync(cancellationToken);
    }

    public async Task<Label?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Labels
            .AsNoTracking()
            .Include(l => l.Notes)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<Label> CreateAsync(Label label, CancellationToken cancellationToken = default)
    {
        label.Id = label.Id == Guid.Empty ? Guid.NewGuid() : label.Id;

        _dbContext.Labels.Add(label);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return label;
    }
}

using Keep.Domain.Entities;

namespace Keep.Application.Labels;

public interface ILabelService
{
    Task<IReadOnlyList<Label>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Label?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Label> CreateAsync(Label label, CancellationToken cancellationToken = default);
}

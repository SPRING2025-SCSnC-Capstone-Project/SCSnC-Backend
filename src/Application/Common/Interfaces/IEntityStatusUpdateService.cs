namespace Application.Common.Interfaces;

public interface IEntityStatusUpdateService
{
    Task UpdateEntityStatusesAsync(CancellationToken cancellationToken);
}
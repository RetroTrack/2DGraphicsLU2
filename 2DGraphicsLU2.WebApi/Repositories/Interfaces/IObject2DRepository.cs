using _2DGraphicsLU2.WebApi.Models;

namespace _2DGraphicsLU2.WebApi.Repositories.Interfaces
{
    public interface IObject2DRepository
    {
        Task<Object2D?> InsertAsync(Guid environmentId, Object2D object2D, string userId);
        Task<Object2D?> ReadAsync(Guid environmentId, Guid objectId, string userId);
        Task<IEnumerable<Object2D>> ReadAsync(Guid environmentId, string userId);
        bool IsObjectOutOfBounds(Object2D object2D, Environment2D? environment);
        Task UpdateAsync(Guid environmentId, Object2D object2D, string userId);
        Task DeleteAsync(Guid environmentId, Guid objectId, string userId);
    }
}

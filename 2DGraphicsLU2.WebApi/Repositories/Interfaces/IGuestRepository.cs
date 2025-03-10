using _2DGraphicsLU2.WebApi.Models;

namespace _2DGraphicsLU2.WebApi.Repositories.Interfaces
{
    public interface IGuestRepository
    {
        Task<bool> InsertAsync(Guid environmentId, string userId, string guestUsername);
        Task<IEnumerable<Object2D>> ReadObjectsAsync(Guid environmentId, string userId);
        Task<Environment2D?> ReadAsync(Guid environmentId, string userId);
        Task<IEnumerable<Environment2D>> ReadAsync(string userId);
        Task DeleteAsync(Guid environmentId, string userId);
        Task DeleteAsync(Guid environmentId, string userId, string guestUsername);
    }
}

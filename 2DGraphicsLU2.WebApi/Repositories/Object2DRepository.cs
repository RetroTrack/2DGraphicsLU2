using _2DGraphicsLU2.WebApi.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace _2DGraphicsLU2.WebApi.Repositories
{
    public class Object2DRepository
    {
        private readonly string sqlConnectionString;

        public Object2DRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task<Object2D?> InsertAsync(Guid environmentId, Object2D object2D, string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                // Check if the environment exists and belongs to the user
                var environment = await sqlConnection.QuerySingleOrDefaultAsync<Environment2D>("SELECT * FROM [Environment2D] " +
                    "WHERE Id = @Id AND UserId = @UserId",
                    new { Id = environmentId, UserId = userId });

                if (environment == null || object2D.PositionX > environment.MaxLength || object2D.PositionX < -environment.MaxLength ||
                    object2D.PositionY > environment.MaxHeight || object2D.PositionY < -environment.MaxHeight)
                    return null;

                // Insert the object
                await sqlConnection.ExecuteAsync(
                    "INSERT INTO [Object2D] (Id, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer, EnvironmentId) " +
                    "VALUES (@Id, @PrefabId, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer, @EnvironmentId)",
                    new { object2D.Id, object2D.PrefabId, object2D.PositionX, object2D.PositionY, object2D.ScaleX, object2D.ScaleY, object2D.RotationZ, object2D.SortingLayer, EnvironmentId = environmentId });

                return object2D;
            }
        }



        public async Task<Object2D?> ReadAsync(Guid environmentId, Guid objectId, string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Object2D>("SELECT obj2d.* FROM [Object2D] obj2d " +
                    "JOIN [Environment2D] env2d ON obj2d.EnvironmentId = env2d.Id " +
                    "WHERE obj2d.Id = @ObjectId AND obj2d.EnvironmentId = @EnvironmentId AND env2d.UserId = @UserId",
                    new { ObjectId = objectId, EnvironmentId = environmentId, UserId = userId });
            }
        }

        public async Task<IEnumerable<Object2D>> ReadAsync(Guid environmentId, string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Object2D>("SELECT obj2d.* FROM [Object2D] obj2d " +
                    "JOIN [Environment2D] env2d ON obj2d.EnvironmentId = env2d.Id " +
                    "WHERE obj2d.EnvironmentId = @EnvironmentId AND env2d.UserId = @UserId",
                    new { EnvironmentId = environmentId, UserId = userId });
            }
        }

        public async Task UpdateAsync(Guid environmentId, Object2D object2D, string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var environment = await sqlConnection.QuerySingleOrDefaultAsync<Environment2D>("SELECT * FROM [Environment2D] " +
                    "WHERE Id = @Id AND UserId = @UserId",
                    new { Id = environmentId, UserId = userId });

                if (environment == null || object2D.PositionX > environment.MaxLength || object2D.PositionX < -environment.MaxLength ||
                    object2D.PositionY > environment.MaxHeight || object2D.PositionY < -environment.MaxHeight)
                    return;

                await sqlConnection.ExecuteAsync("UPDATE [Object2D] " +
                    "SET PrefabId = @PrefabId, PositionX = @PositionX, PositionY = @PositionY, ScaleX = @ScaleX, ScaleY = @ScaleY, RotationZ = @RotationZ, SortingLayer = @SortingLayer " +
                    "FROM [Object2D] obj2d " +
                    "JOIN [Environment2D] env2d ON obj2d.EnvironmentId = env2d.Id " +
                    "WHERE obj2d.Id = @Id AND obj2d.EnvironmentId = @EnvironmentId AND env2d.UserId = @UserId",
                    new { object2D.Id, object2D.PrefabId, object2D.PositionX, object2D.PositionY, object2D.ScaleX, object2D.ScaleY, object2D.RotationZ, object2D.SortingLayer, EnvironmentId = environmentId, UserId = userId });
            }
        }

        public async Task DeleteAsync(Guid environmentId, Guid objectId, string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE obj2d " +
                    "FROM[Object2D] obj2d " +
                    "JOIN[Environment2D] env2d ON obj2d.EnvironmentId = env2d.Id " +
                    "WHERE obj2d.Id = @ObjectId AND obj2d.EnvironmentId = @EnvironmentId AND env2d.UserId = @UserId",
                    new { ObjectId = objectId, EnvironmentId = environmentId, UserId = userId });
            }
        }

    }
}

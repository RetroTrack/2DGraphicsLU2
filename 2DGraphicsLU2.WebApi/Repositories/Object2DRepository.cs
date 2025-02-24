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

        public async Task<Object2D> InsertAsync(Object2D object2D)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var environmentId = await sqlConnection.ExecuteAsync("INSERT INTO [Object2D] (Id, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer) VALUES (@Id, @PrefabId, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer)", object2D);
                return object2D;
            }
        }

        public async Task<Object2D?> ReadAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Object2D>("SELECT * FROM [Object2D] WHERE Id = @Id", new { id });
            }
        }

        public async Task<IEnumerable<Object2D>> ReadAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Object2D>("SELECT * FROM [Object2D]");
            }
        }

        public async Task UpdateAsync(Object2D environment)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("UPDATE [Object2D] SET " +
                                                 "PrefabId = @PrefabId, " +
                                                 "PositionX = @PositionX," +
                                                 "PositionY = @PositionY," +
                                                 "ScaleX = @ScaleX," +
                                                 "ScaleY = @ScaleY," +
                                                 "RotationZ = @RotationZ," +
                                                 "SortingLayer = @SortingLayer"
                                                 , environment);

            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Object2D] WHERE Id = @Id", new { id });
            }
        }

    }
}

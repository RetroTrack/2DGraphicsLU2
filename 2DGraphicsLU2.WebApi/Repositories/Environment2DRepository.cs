using _2DGraphicsLU2.WebApi.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace _2DGraphicsLU2.WebApi.Repositories
{
    public class Environment2DRepository
    {
        private readonly string sqlConnectionString;

        public Environment2DRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task<Environment2D> InsertAsync(Environment2D environment2D, string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var environmentId = await sqlConnection.ExecuteAsync("INSERT INTO [Environment2D] (Id, [Name], [MaxHeight], MaxLength, UserId) " +
                    "VALUES (@Id, @Name, @MaxHeight, @MaxLength, @UserId)", 
                    new { environment2D.Id, environment2D.Name, environment2D.MaxHeight, environment2D.MaxLength, UserId = userId });
                return environment2D;
            }
        }

        public async Task<Environment2D?> ReadAsync(Guid environmentId, string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Environment2D>("SELECT * FROM [Environment2D] " +
                    "WHERE Id = @Id AND UserId = @UserId", 
                    new { Id = environmentId, UserId = userId });
            }
        }

        public async Task<IEnumerable<Environment2D>> ReadAsync(string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Environment2D>("SELECT * FROM [Environment2D] " +
                    "WHERE UserId = @UserId", 
                    new { UserId = userId });
            }
        }

        public async Task UpdateAsync(Environment2D environment, string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("UPDATE [Environment2D] SET [Name] = @Name, [MaxHeight] = @MaxHeight, MaxLength = @MaxLength " +
                                                 "WHERE Id = @Id AND UserId = @UserId"
                                                 , new { environment.Id, environment.Name, environment.MaxHeight, environment.MaxLength , UserId = userId });

            }
        }

        public async Task DeleteAsync(Guid environmentId, string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Environment2D] " +
                    "WHERE Id = @Id AND UserId = @UserId", 
                    new { Id = environmentId, UserId = userId });
            }
        }
    }
}

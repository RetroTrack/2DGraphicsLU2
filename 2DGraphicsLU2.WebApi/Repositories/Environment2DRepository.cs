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
                var environmentId = await sqlConnection.ExecuteAsync("INSERT INTO [Environment2D] (Id, [Name], [MaxHeight], MaxLength) VALUES (@Id, @Name, @MaxHeight, @MaxLength) WHERE userId = @userId", new { environment2D, userId });
                return environment2D;
            }
        }

        public async Task<Environment2D?> ReadAsync(Guid id, string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Environment2D>("SELECT * FROM [Environment2D] WHERE Id = @Id && WHERE userId = @userId", new { id, userId });
            }
        }

        public async Task<IEnumerable<Environment2D>> ReadAsync(string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Environment2D>("SELECT * FROM [Environment2D] WHERE userId = @userId", new { userId });
            }
        }

        public async Task UpdateAsync(Environment2D environment, string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("UPDATE [Environment2D] SET " +
                                                 "[Name] = @Name, " +
                                                 "[MaxHeight] = @MaxHeight " +
                                                 "MaxLength = @MaxLength " +
                                                 "WHERE Id = @Id " +
                                                 "WHERE userId = @userId"
                                                 , new { environment, userId });

            }
        }

        public async Task DeleteAsync(Guid id, string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Environment2D] WHERE Id = @Id && WHERE userId = @userId", new { id, userId });
            }
        }
    }
}

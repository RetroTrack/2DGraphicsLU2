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

        public async Task<Environment2D?> InsertAsync(Environment2D environment2D, string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                IEnumerable<Environment2D> existingEnvironments = await sqlConnection.QueryAsync<Environment2D>(
                "SELECT * FROM [Environment2D] WHERE UserId = @UserId",
                new { UserId = userId });


                if (string.IsNullOrWhiteSpace(environment2D.Name))
                    environment2D.Name = "New World";

                if (existingEnvironments.Count() >= 5 || existingEnvironments.Any(environment => environment.Name.Equals(environment2D.Name)) || environment2D.Name.Length > 25)
                    return null;

                if (environment2D.MaxHeight < 10)
                    environment2D.MaxHeight = 10;
                else if (environment2D.MaxHeight > 100)
                    environment2D.MaxHeight = 100;

                if (environment2D.MaxLength < 20)
                    environment2D.MaxLength = 20;
                else if (environment2D.MaxLength > 200)
                    environment2D.MaxLength = 200;



                await sqlConnection.ExecuteAsync("INSERT INTO [Environment2D] (Id, [Name], [MaxHeight], MaxLength, UserId) " +
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
                                                 , new { environment.Id, environment.Name, environment.MaxHeight, environment.MaxLength, UserId = userId });

            }
        }

        public async Task DeleteAsync(Guid environmentId, string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE obj2d " +
                    "FROM [Object2D] obj2d " +
                    "JOIN [Environment2D] env2d ON obj2d.EnvironmentId = env2d.Id " +
                    "WHERE obj2d.EnvironmentId = @EnvironmentId AND env2d.UserId = @UserId",
                    new { EnvironmentId = environmentId, UserId = userId });

                await sqlConnection.ExecuteAsync("DELETE FROM [Environment2D] " +
                    "WHERE Id = @Id AND UserId = @UserId",
                    new { Id = environmentId, UserId = userId });
            }
        }
    }
}

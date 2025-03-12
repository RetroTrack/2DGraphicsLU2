using System;
using _2DGraphicsLU2.WebApi.Models;
using _2DGraphicsLU2.WebApi.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

namespace _2DGraphicsLU2.WebApi.Repositories
{
    public class GuestRepository : IGuestRepository
    {
        private readonly string sqlConnectionString;

        public GuestRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task<bool> InsertAsync(Guid environmentId, string userId, string guestUsername)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                // Check if the environment exists and belongs to the user
                var environment = await sqlConnection.QuerySingleOrDefaultAsync<Environment2D>(
                    "SELECT * FROM [Environment2D] " +
                    "WHERE Id = @EnvironmentId AND UserId = @UserId",
                    new { EnvironmentId = environmentId, UserId = userId });

                if (environment == null )
                    return false;
                // Check if the guest exists and is not the user
                var guestId = await sqlConnection.QuerySingleOrDefaultAsync<string>(
                    "SELECT (Id) FROM [auth].[AspNetUsers] " +
                    "WHERE UserName = @UserName",
                    new { UserName = guestUsername});
                if (guestId == null || guestId == userId)
                    return false;

                // Check if the guest already exists
                var existingGuest = await sqlConnection.QuerySingleOrDefaultAsync<string>(
                    "SELECT UserId FROM [Guest] " +
                    "WHERE EnvironmentId = @EnvironmentId AND UserId = @UserId",
                    new { EnvironmentId = environmentId, UserId = guestId });

                if (existingGuest != null)
                    return false;

                // Insert the object
                await sqlConnection.ExecuteAsync(
                    "INSERT INTO [Guest] (UserId, EnvironmentId) " +
                    "VALUES (@UserId, @EnvironmentId)",
                    new { UserId = guestId ,EnvironmentId = environmentId });
                return true;
            }
        }

        public async Task<IEnumerable<Object2D>> ReadObjectsAsync(Guid environmentId, string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Object2D>("SELECT obj2d.* FROM [Object2D] obj2d " +
                    "JOIN [Guest] guest ON obj2d.EnvironmentId = guest.EnvironmentId " +
                    "WHERE obj2d.EnvironmentId = @EnvironmentId AND guest.UserId = @UserId",
                    new { EnvironmentId = environmentId, UserId = userId });
            }
        }

        public async Task<Environment2D?> ReadAsync(Guid environmentId, string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Environment2D>(
                    "SELECT environment.* FROM [Environment2D] environment " +
                    "JOIN [Guest] guest ON environment.Id = guest.EnvironmentId " +
                    "WHERE environment.Id = @EnvironmentId AND guest.UserId = @UserId",
                    new { EnvironmentId = environmentId, UserId = userId });
            }
        }

        public async Task<IEnumerable<Environment2D>> ReadAsync(string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Environment2D>(
                    "SELECT environment.* FROM [Environment2D] environment " +
                    "JOIN [Guest] guest ON environment.Id = guest.EnvironmentId " +
                    "WHERE guest.UserId = @UserId",
                    new {UserId = userId });
            }
        }

        public async Task DeleteAsync(Guid environmentId, string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync(
                    "DELETE FROM [Guest] " +
                    "WHERE EnvironmentId = @EnvironmentId AND UserId = @UserId",
                    new { EnvironmentId = environmentId, UserId = userId });
            }
        }

        public async Task DeleteAsync(Guid environmentId, string userId, string guestUsername)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                // Check if the environment exists and belongs to the user
                var environment = await sqlConnection.QuerySingleOrDefaultAsync<Environment2D>(
                    "SELECT * FROM [Environment2D] " +
                    "WHERE Id = @EnvironmentId AND UserId = @UserId",
                    new { EnvironmentId = environmentId, UserId = userId });

                if (environment == null)
                    return;
                // Check if the guest exists and is not the user
                var guestId = await sqlConnection.QuerySingleOrDefaultAsync<string>(
                    "SELECT (Id) FROM [auth].[AspNetUsers] " +
                    "WHERE UserName = @UserName",
                    new { UserName = guestUsername });
                if (guestId == null || guestId == userId)
                    return;
                // Delete the guest
                await sqlConnection.ExecuteAsync(
                    "DELETE FROM [Guest] " +
                    "WHERE EnvironmentId = @EnvironmentId AND UserId = @GuestId",
                    new { EnvironmentId = environmentId, GuestId = guestId });
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Mosaico.Base.Exceptions;
using Mosaico.Core.EntityFramework.Configurations;
using Mosaico.Persistence.SqlServer.Abstractions;

namespace Mosaico.Persistence.SqlServer
{
    public class DatabaseQueryContext : IDbQueryContext, IAsyncDisposable, IDisposable
    {
        private readonly IIndex<string, SqlServerConfiguration> _sqlConfigurations;
        private SqlConnection _sqlConnection;

        public DatabaseQueryContext(IIndex<string, SqlServerConfiguration> sqlConfigurations)
        {
            _sqlConfigurations = sqlConfigurations;
        }

        public async Task<IEnumerable<T>> SelectMany<T>(string query, object queryParams = null)
        {
            if (!_sqlConfigurations.TryGetValue("Tokenizer", out var config))
            {
                throw new InvalidConfigException("Tokenizer SQL Server");
            }
            await using SqlConnection conn = new SqlConnection(config.ConnectionString);
            _sqlConnection = conn;
            _sqlConnection.Open();
            var result = await _sqlConnection.QueryAsync<T>(
                query,
                queryParams,
                commandType: CommandType.Text);
            return result;
        }

        public async Task<T> Single<T>(string query, object queryParams = null)
        {
            if (!_sqlConfigurations.TryGetValue("Tokenizer", out var config))
            {
                throw new InvalidConfigException("Tokenizer SQL Server");
            }
            await using SqlConnection conn = new SqlConnection(config.ConnectionString);
            _sqlConnection = conn;
            _sqlConnection.Open();
            var result = await conn.QuerySingleAsync<T>(
                query,
                queryParams,
                commandType: CommandType.Text);
            return result;
        }

        public async Task<T> FirstOrDefault<T>(string query, object queryParams = null)
        {
            if (!_sqlConfigurations.TryGetValue("Tokenizer", out var config))
            {
                throw new InvalidConfigException("Tokenizer SQL Server");
            }
            await using SqlConnection conn = new SqlConnection(config.ConnectionString);
            _sqlConnection = conn;
            _sqlConnection.Open();
            var result = await conn.QueryFirstOrDefaultAsync<T>(
                query,
                queryParams,
                commandType: CommandType.Text);
            return result;
        }
        
        public async ValueTask DisposeAsync()
        {
            if (_sqlConnection != null)
            {
                await _sqlConnection?.CloseAsync();
            }
        }

        public void Dispose()
        {
            _sqlConnection?.Dispose();
        }
    }
}
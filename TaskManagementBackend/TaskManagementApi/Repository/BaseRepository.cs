using Microsoft.Data.SqlClient;

namespace TaskManagementApi.Repository;

public abstract class BaseRepository
{
    private readonly string _connectionString;

    protected BaseRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("SqlServer")!;
    }

    // All child repositories use this to get a connection
    protected SqlConnection CreateConnection() => new SqlConnection(_connectionString);
}
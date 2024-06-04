using MySql.Data.MySqlClient;

namespace SmartStep;

public class DBHelper
{
    public MySqlConnectionStringBuilder _connectionString { get; }

    public DBHelper()
    {
        _connectionString = new MySqlConnectionStringBuilder
        {
            Server = "localhost",
            Database = "smartstep",
            UserID = "root",
            Password = "rootroot"
        };
    }
}
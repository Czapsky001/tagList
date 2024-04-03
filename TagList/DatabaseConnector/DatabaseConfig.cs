namespace TagList.DatabaseConnector;


public class DatabaseConfig
{
    public static string GetConnectionString()
    {
        string host = Environment.GetEnvironmentVariable("DATABASE_HOST");
        string port = Environment.GetEnvironmentVariable("DATABASE_PORT");
        string database = Environment.GetEnvironmentVariable("DATABASE_NAME");
        string user = Environment.GetEnvironmentVariable("DATABASE_USER");
        string password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
        return $"Server=db;Port={port};Database={database};User Id={user};Password={password};";


    }

}

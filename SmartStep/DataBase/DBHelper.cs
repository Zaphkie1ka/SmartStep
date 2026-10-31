using System;
using System.IO;
using System.Text.Json;
using MySql.Data.MySqlClient;

namespace SmartStep;

/// <summary>
/// Builds the MySQL connection string. Settings are taken, in order of priority, from:
/// 1) environment variables SMARTSTEP_DB_SERVER / _PORT / _NAME / _USER / _PASSWORD;
/// 2) appsettings.json next to the executable (see appsettings.example.json);
/// 3) defaults for local development (no password).
/// Credentials must never be committed to the repository.
/// </summary>
public class DBHelper
{
    public MySqlConnectionStringBuilder _connectionString { get; }

    public DBHelper()
    {
        var builder = new MySqlConnectionStringBuilder
        {
            Server = "localhost",
            Port = 3306,
            Database = "smartstep",
            UserID = "root",
            Password = ""
        };

        ApplyFile(builder);
        ApplyEnvironment(builder);

        _connectionString = builder;
    }

    private static void ApplyFile(MySqlConnectionStringBuilder builder)
    {
        string path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (!File.Exists(path))
            return;

        using var stream = File.OpenRead(path);
        using var doc = JsonDocument.Parse(stream);
        if (!doc.RootElement.TryGetProperty("Database", out var db))
            return;

        if (db.TryGetProperty("Server", out var server) && server.GetString() is { Length: > 0 } s)
            builder.Server = s;
        if (db.TryGetProperty("Port", out var port) && port.TryGetUInt32(out var p))
            builder.Port = p;
        if (db.TryGetProperty("Database", out var name) && name.GetString() is { Length: > 0 } n)
            builder.Database = n;
        if (db.TryGetProperty("User", out var user) && user.GetString() is { Length: > 0 } u)
            builder.UserID = u;
        if (db.TryGetProperty("Password", out var password) && password.GetString() is { } pw)
            builder.Password = pw;
    }

    private static void ApplyEnvironment(MySqlConnectionStringBuilder builder)
    {
        string? Env(string key) => Environment.GetEnvironmentVariable($"SMARTSTEP_DB_{key}");

        if (!string.IsNullOrEmpty(Env("SERVER"))) builder.Server = Env("SERVER")!;
        if (uint.TryParse(Env("PORT"), out var port)) builder.Port = port;
        if (!string.IsNullOrEmpty(Env("NAME"))) builder.Database = Env("NAME")!;
        if (!string.IsNullOrEmpty(Env("USER"))) builder.UserID = Env("USER")!;
        if (Env("PASSWORD") is { } password) builder.Password = password;
    }
}

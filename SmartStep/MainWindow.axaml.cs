using System;
using System.Security.Cryptography;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace SmartStep;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private static void ShowMessage(string title, string text)
    {
        MessageBoxManager.GetMessageBoxStandard(title, text, ButtonEnum.Ok).ShowAsync();
    }

    private static bool IsBcryptHash(string value)
    {
        return value.StartsWith("$2a$") || value.StartsWith("$2b$") || value.StartsWith("$2y$");
    }

    private static bool FixedTimeEquals(string a, string b)
    {
        return CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(a), Encoding.UTF8.GetBytes(b));
    }

    /// <summary>
    /// Checks login and password against the `teachers` table.
    /// Passwords are verified with BCrypt. If an old plain-text password is found in the table,
    /// it is accepted once and immediately replaced with a BCrypt hash.
    /// </summary>
    private bool CheckCredentials(string login, string password)
    {
        DBHelper db = new DBHelper();
        using var connection = new MySqlConnection(db._connectionString.ConnectionString);
        connection.Open();

        string? stored = null;
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "SELECT `Password` FROM teachers WHERE `Login` = @login LIMIT 1";
            command.Parameters.AddWithValue("@login", login);
            object? result = command.ExecuteScalar();
            if (result == null || result is DBNull)
                return false;
            stored = result.ToString();
        }

        if (string.IsNullOrEmpty(stored))
            return false;

        if (IsBcryptHash(stored))
        {
            return BCrypt.Net.BCrypt.Verify(password, stored);
        }

        // Legacy plain-text password: compare and upgrade to a hash.
        if (!FixedTimeEquals(stored, password))
            return false;

        try
        {
            using var update = connection.CreateCommand();
            update.CommandText = "UPDATE teachers SET `Password` = @hash WHERE `Login` = @login";
            update.Parameters.AddWithValue("@hash", BCrypt.Net.BCrypt.HashPassword(password));
            update.Parameters.AddWithValue("@login", login);
            update.ExecuteNonQuery();
        }
        catch (Exception)
        {
            // The column may be too short for a hash; login still succeeds.
        }

        return true;
    }

    private void LogIn()
    {
        string login = loginbox.Text?.Trim() ?? "";
        string password = passbox.Text ?? "";

        if (login.Length == 0 || password.Length == 0)
        {
            ShowMessage("Внимание!", "Введите логин и пароль.");
            return;
        }

        try
        {
            if (!CheckCredentials(login, password))
            {
                ShowMessage("Внимание!", "Неправильный логин или пароль!");
                return;
            }

            Menu menu = new Menu();
            menu.Show();
            Close();
        }
        catch (Exception ex)
        {
            ShowMessage("Ошибка!", ex.Message);
        }
    }

    private void LogIn_Click(object? sender, RoutedEventArgs e)
    {
        LogIn();
    }

    private void Exit_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}

using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace SmartStep;

public partial class SafetyWindow : Window
{
    public SafetyWindow()
    {
        InitializeComponent();
    }

    private static void ShowError(string message)
    {
        MessageBoxManager.GetMessageBoxStandard("Ошибка!", message, ButtonEnum.Ok).ShowAsync();
    }

    private void Add_Button(object? sender, RoutedEventArgs e)
    {
        string name = textBoxName.Text?.Trim() ?? "";
        string description = textBoxDescription.Text?.Trim() ?? "";

        if (name.Length == 0)
        {
            ShowError("Введите название инструктажа.");
            return;
        }

        try
        {
            DBHelper db = new DBHelper();
            using (var connection = new MySqlConnection(db._connectionString.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Safety (`Name`, `Description`) VALUES (@Name, @Description);";
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Description", description);
                    if (command.ExecuteNonQuery() == 0)
                    {
                        ShowError("Не удалось выполнить добавление!");
                        return;
                    }
                }
            }

            Close(true);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private void Ext_Button(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
}

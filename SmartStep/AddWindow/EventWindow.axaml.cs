using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace SmartStep;

public partial class EventWindow : Window
{
    public EventWindow()
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
        string location = textBoxLocation.Text?.Trim() ?? "";

        if (name.Length == 0)
        {
            ShowError("Введите название мероприятия.");
            return;
        }
        if (datePickerDate.SelectedDate is not { } date)
        {
            ShowError("Выберите дату проведения.");
            return;
        }
        if (location.Length == 0)
        {
            ShowError("Введите локацию.");
            return;
        }

        int count = (int)(numericCount.Value ?? 0);
        if (count < 0)
        {
            ShowError("Количество не может быть отрицательным.");
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
                    command.CommandText =
                        "INSERT INTO Events (`Name`, `Date`, `Location`, `Count`) VALUES (@Name, @Date, @Location, @Count);";
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Date", date.Date);
                    command.Parameters.AddWithValue("@Location", location);
                    command.Parameters.AddWithValue("@Count", count);
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

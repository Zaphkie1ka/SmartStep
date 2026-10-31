using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace SmartStep;

public partial class ProgressWindow : Window
{
    /// <summary>Item for a ComboBox: shows Title, carries the database ID.</summary>
    private sealed class ListItem
    {
        public int Id { get; }
        public string Title { get; }

        public ListItem(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public override string ToString() => Title;
    }

    public ProgressWindow()
    {
        InitializeComponent();
        try
        {
            LoadLists();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private static void ShowError(string message)
    {
        MessageBoxManager.GetMessageBoxStandard("Ошибка!", message, ButtonEnum.Ok).ShowAsync();
    }

    private void LoadLists()
    {
        var students = new List<ListItem>();
        var events = new List<ListItem>();

        DBHelper db = new DBHelper();
        using (var connection = new MySqlConnection(db._connectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT ID, Last_Name, First_Name FROM Students ORDER BY Last_Name, First_Name";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new ListItem(reader.GetInt32("ID"),
                            reader.GetString("Last_Name") + " " + reader.GetString("First_Name")));
                    }
                }

                command.CommandText = "SELECT ID, Name FROM Events ORDER BY `Date` DESC";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        events.Add(new ListItem(reader.GetInt32("ID"), reader.GetString("Name")));
                    }
                }
            }
        }

        comboBoxStudent.ItemsSource = students;
        comboBoxEvent.ItemsSource = events;
    }

    private void Add_Button(object? sender, RoutedEventArgs e)
    {
        if (comboBoxStudent.SelectedItem is not ListItem student)
        {
            ShowError("Выберите студента.");
            return;
        }
        if (comboBoxEvent.SelectedItem is not ListItem ev)
        {
            ShowError("Выберите мероприятие.");
            return;
        }
        string? result = (comboBoxResult.SelectedItem as ComboBoxItem)?.Content?.ToString();
        if (string.IsNullOrWhiteSpace(result))
        {
            ShowError("Выберите результат участия.");
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
                        "INSERT INTO Progress (StudentName, EventName, Result) VALUES (@Student, @Event, @Result);";
                    command.Parameters.AddWithValue("@Student", student.Id);
                    command.Parameters.AddWithValue("@Event", ev.Id);
                    command.Parameters.AddWithValue("@Result", result);
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

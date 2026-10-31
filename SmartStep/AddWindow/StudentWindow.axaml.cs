using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;
using SmartStep.Models;

namespace SmartStep;

public partial class StudentWindow : Window
{
    public Students NewStudents { get; set; }
    private List<Orders> _order { get; set; } = new();

    public StudentWindow()
    {
        InitializeComponent();
        NewStudents = new Students();
        DataContext = NewStudents;
        try
        {
            UpdateComboBox();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private void UpdateComboBox()
    {
        _order = new List<Orders>();
        DBHelper db = new DBHelper();
        using (var connection = new MySqlConnection(db._connectionString.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM `Order`";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _order.Add(new Orders
                        {
                            ID = reader.GetInt32("ID"),
                            Type = reader.GetString("Type"),
                            Date = reader.GetDateTime("Date")
                        });
                    }
                }
            }
        }
        comboBoxOrder.ItemsSource = _order;
    }

    private static void ShowError(string message)
    {
        MessageBoxManager.GetMessageBoxStandard("Ошибка!", message, ButtonEnum.Ok).ShowAsync();
    }

    /// <summary>Returns an error text for the first invalid field, or null if all fields are valid.</summary>
    private string? Validate()
    {
        if (string.IsNullOrWhiteSpace(NewStudents.Last_Name)) return "Введите фамилию студента.";
        if (string.IsNullOrWhiteSpace(NewStudents.First_Name)) return "Введите имя студента.";

        if (datePickerBirthday.SelectedDate is not { } birthday)
            return "Выберите дату рождения.";
        if (birthday.Date > DateTime.Today)
            return "Дата рождения не может быть в будущем.";

        if (string.IsNullOrWhiteSpace(NewStudents.School)) return "Введите школу.";
        if (string.IsNullOrWhiteSpace(NewStudents.Class)) return "Введите класс.";
        if (string.IsNullOrWhiteSpace(NewStudents.Address)) return "Введите адрес.";
        if (string.IsNullOrWhiteSpace(NewStudents.ParentLast_Name)) return "Введите фамилию родителя.";
        if (string.IsNullOrWhiteSpace(NewStudents.ParentFirst_Name)) return "Введите имя родителя.";

        if (string.IsNullOrWhiteSpace(NewStudents.ParentNumber)) return "Введите номер телефона родителя.";
        int digits = 0;
        foreach (char c in NewStudents.ParentNumber)
            if (char.IsDigit(c)) digits++;
        if (digits < 10 || digits > 15)
            return "Номер телефона должен содержать от 10 до 15 цифр.";

        if (comboBoxOrder.SelectedItem is not Orders)
            return "Выберите приказ.";

        return null;
    }

    private void Add_Button(object? sender, RoutedEventArgs e)
    {
        string? error = Validate();
        if (error != null)
        {
            ShowError(error);
            return;
        }

        try
        {
            NewStudents.Birthday = datePickerBirthday.SelectedDate!.Value.Date;
            int orderId = ((Orders)comboBoxOrder.SelectedItem!).ID;

            DBHelper db = new DBHelper();
            using (var connect = new MySqlConnection(db._connectionString.ConnectionString))
            {
                connect.Open();
                using (var command = connect.CreateCommand())
                {
                    command.CommandText =
                        "INSERT INTO Students (First_Name, Last_Name, Birthday, School, Class, Address, ParentFirst_Name, ParentLast_Name, ParentNumber, OrderNumber) " +
                        "VALUES (@First_Name, @Last_Name, @Birthday, @School, @Class, @Address, @ParentFirst_Name, @ParentLast_Name, @ParentNumber, @OrderNumber);";
                    command.Parameters.AddWithValue("@First_Name", NewStudents.First_Name.Trim());
                    command.Parameters.AddWithValue("@Last_Name", NewStudents.Last_Name.Trim());
                    command.Parameters.AddWithValue("@Birthday", NewStudents.Birthday);
                    command.Parameters.AddWithValue("@School", NewStudents.School.Trim());
                    command.Parameters.AddWithValue("@Class", NewStudents.Class.Trim());
                    command.Parameters.AddWithValue("@Address", NewStudents.Address.Trim());
                    command.Parameters.AddWithValue("@ParentFirst_Name", NewStudents.ParentFirst_Name.Trim());
                    command.Parameters.AddWithValue("@ParentLast_Name", NewStudents.ParentLast_Name.Trim());
                    command.Parameters.AddWithValue("@ParentNumber", NewStudents.ParentNumber.Trim());
                    command.Parameters.AddWithValue("@OrderNumber", orderId);
                    if (command.ExecuteNonQuery() == 0)
                    {
                        ShowError("Не удалось выполнить добавление!");
                        return;
                    }
                }
            }

            // Tell the caller that a student was added so it can refresh its list.
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

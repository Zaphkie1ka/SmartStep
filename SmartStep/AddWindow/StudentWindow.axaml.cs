using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;
using SmartStep.Models;
using Order = Mysqlx.Crud.Order;

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
        comboBoxOrder = this.Find<ComboBox>("comboBoxOrder");
        UpdateComboBox();
    }

    private void UpdateComboBox()
    {
        _order = new List<Orders>();
        _order.Clear();
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
            connection.Close();
        }
        comboBoxOrder.ItemsSource = _order;
    }
    private void Add_Button(object? sender, RoutedEventArgs e)
    {
        try
        {
            DBHelper db = new DBHelper();
            using (var connect = new MySqlConnection(db._connectionString.ConnectionString))
            {
                connect.Open();
                using (var command = connect.CreateCommand())
                {
                    command.CommandText =
                        "INSERT INTO Students (First_Name, Last_Name, Birthday, School, Class, Address, ParentFirst_Name, ParentLast_Name, ParentNumber, OrderNumber)" +
                        "VALUES (@First_Name, @Last_Name, @Birthday, @School, @Class, @Address, @ParentFirst_Name, @ParentLast_Name, @ParentNumber, @OrderNumber);";
                    command.Parameters.AddWithValue("@First_Name", NewStudents.First_Name);
                    command.Parameters.AddWithValue("@Last_Name", NewStudents.Last_Name);
                    command.Parameters.AddWithValue("@Birthday", NewStudents.Birthday);
                    command.Parameters.AddWithValue("@School", NewStudents.School);
                    command.Parameters.AddWithValue("@Class", NewStudents.Class);
                    command.Parameters.AddWithValue("@Address", NewStudents.Address);
                    command.Parameters.AddWithValue("@ParentFirst_Name", NewStudents.ParentFirst_Name);
                    command.Parameters.AddWithValue("@ParentLast_Name", NewStudents.ParentLast_Name);
                    command.Parameters.AddWithValue("@ParentNumber", NewStudents.ParentNumber);
                    command.Parameters.AddWithValue("@OrderNumber", ((comboBoxOrder.SelectedItem) as Orders).ID);
                    var rowsCount = command.ExecuteNonQuery();
                    if (rowsCount == 0)
                    {
                        MessageBoxManager
                            .GetMessageBoxStandard("Ошибка!", "Не удалось выполнить добавление!", ButtonEnum.Ok).ShowAsync();
                    }
                }

                connect.Close();
            }
            Close();
        }
        catch (Exception ex)
        {
            MessageBoxManager.GetMessageBoxStandard("Ошибка!", ex.Message, ButtonEnum.Ok).ShowAsync();
        }
    }

    private void Ext_Button(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
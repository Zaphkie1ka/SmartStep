using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;
using SmartStep.Models;

namespace SmartStep;

public partial class Menu : Window
{
    private List<Students> _students;
    private List<Directions> _directions;
    private List<Safety> _safety;
    private List<Events> _events;
    private List<Progress> _progresses;
    private List<Orders> _orders;
    private List<StudentSafety> _studsafety;
    private List<StudentInGroup> _studingroup;
    private List<Groups> _groups;
    
    int year = DateTime.Now.Year;
    int month = DateTime.Now.Month;
    public Menu()
    {
        InitializeComponent();
        ShowBD();
        GenerateColumnsForMonth();
    }

    public void ShowBD()
    {
        try
        {
            _students = new List<Students>();
            _directions = new List<Directions>();
            _events = new List<Events>();
            _orders = new List<Orders>();
            DBHelper db = new DBHelper();
            using (var connection = new MySqlConnection(db._connectionString.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * From Students";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _students.Add(new Students
                            {
                                ID = reader.GetInt32("ID"),
                                First_Name = reader.GetString("First_Name"),
                                Last_Name = reader.GetString("Last_Name"),
                                Birthday = reader.GetDateTime("Birthday"),
                                School = reader.GetString("School"),
                                Class = reader.GetString("Class"),
                                Address = reader.GetString("Address"),
                                ParentFirst_Name = reader.GetString("ParentFirst_Name"),
                                ParentLast_Name = reader.GetString("ParentLast_Name"),
                                ParentNumber = reader.GetString("ParentNumber"),
                                OrderNumber = reader.GetInt32("OrderNumber")
                            });
                        }
                    }
                    command.CommandText = "SELECT * From Events";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _events.Add(new Events
                            {
                                ID = reader.GetInt32("ID"),
                                Name = reader.GetString("Name"),
                                Date = reader.GetDateTime("Date"),
                                Location = reader.GetString("Location"),
                                Count = reader.GetInt32("Count")
                            });
                        }
                    }
                    command.CommandText = "SELECT * From `Order`";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _orders.Add(new Orders
                            {
                                ID = reader.GetInt32("ID"),
                                Type = reader.GetString("Type"),
                                Date = reader.GetDateTime("Date")
                            });
                        }
                    }
                    connection.Close();
                }
                Students.ItemsSource = _students;
                Events.ItemsSource = _events;
                Order.ItemsSource = _orders;
            }
        }
        catch (Exception e)
        {
            MessageBoxManager.GetMessageBoxStandard("Ошибка!", e.Message, ButtonEnum.Ok).ShowAsync();
        }
        
    }
    /*private void GenerateColumnsForMonth()
    {
        MainGrid.Columns.Clear();
        DataGridTextColumn studentColumn = new DataGridTextColumn
        {
            Header = "ФИО ученика",
            Binding = new Binding("Models.Students.First_Name")
        };
        MainGrid.Columns.Add(studentColumn);

        string[] months = {"Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"};
        comboBoxMonth.ItemsSource = months; // Устанавливаем источник данных для ComboBox

// Теперь можно безопасно выбирать элемент из ComboBox
        string selectedMonth = (string)comboBoxMonth.SelectedItem; // Выбираем месяц из ComboBox
        if (selectedMonth != null)
        {
            int monthNumber = Convert.ToInt32(selectedMonth);

            int daysInMonth = DateTime.DaysInMonth(year, monthNumber);

            for (int i = 1; i <= daysInMonth; i++)
            {
                DataGridTextColumn column = new DataGridTextColumn
                {
                    Header = i.ToString(),
                    Binding = new Binding($"Day{i}")
                };
                MainGrid.Columns.Add(column);
            }
        }
        else
        {
            // Обработка случая, когда элемент не выбран
            MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите месяц из списка");
        }

        DataGridTextColumn dateColumn = new DataGridTextColumn
        {
            Header = "Дата проведения",
            Binding = new Binding("Date")
        };
        MainGrid.Columns.Add(dateColumn);

        DataGridTextColumn descriptionColumn = new DataGridTextColumn
        {
            Header = "Содержание занятия",
            Binding = new Binding("Description")
        };
        MainGrid.Columns.Add(descriptionColumn);

        DataGridTextColumn hoursColumn = new DataGridTextColumn
        {
            Header = "Часы занятия",
            Binding = new Binding("Hours")
        };
        MainGrid.Columns.Add(hoursColumn);

        DataGridTextColumn noteColumn = new DataGridTextColumn
        {
            Header = "Примечание",
            Binding = new Binding("Note")
        };
        MainGrid.Columns.Add(noteColumn);
    }*/
    private void GenerateColumnsForMonth()
    {
        MainGrid.Columns.Clear();
        DataGridTextColumn studentColumn = new DataGridTextColumn
        {
            Header = "ФИО ученика",
            Binding = new Binding("Models.Students.First_Name")
        };
        MainGrid.Columns.Add(studentColumn);
        for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
        {
            DataGridTextColumn column = new DataGridTextColumn
            {
                Header = day.ToString(),
                Binding = new Binding($"Day{day}"),
                Width = new DataGridLength(50)
            };
            MainGrid.Columns.Add(column);
        }
        DataGridTextColumn dateColumn = new DataGridTextColumn
        {
            Header = "Дата проведения",
            Binding = new Binding("Date")
        };
        MainGrid.Columns.Add(dateColumn);
        DataGridTextColumn descriptionColumn = new DataGridTextColumn
        {
            Header = "Содержание занятия",
            Binding = new Binding("Description")
        };
        MainGrid.Columns.Add(descriptionColumn);
        DataGridTextColumn hoursColumn = new DataGridTextColumn
        {
            Header = "Часы занятия",
            Binding = new Binding("Hours")
        };
        MainGrid.Columns.Add(hoursColumn);
        DataGridTextColumn noteColumn = new DataGridTextColumn
        {
            Header = "Примечание",
            Binding = new Binding("Note")
        };
        MainGrid.Columns.Add(noteColumn);
    }
    private void Button_Add_Student(object? sender, RoutedEventArgs e)
    {
        try
        {
            StudentWindow windowstud = new StudentWindow();
            windowstud.Show();
        }
        catch (Exception exception)
        {
            MessageBoxManager.GetMessageBoxStandard("Ошибка!", exception.Message, ButtonEnum.Ok).ShowAsync();
        }
        
    }

    private void Button_Upd_Student(object? sender, RoutedEventArgs e)
    {
        
    }

    private void Button_Add_Events(object? sender, RoutedEventArgs e)
    {
        EventWindow windowevent = new EventWindow();
        windowevent.Show();
    }

    private void Button_Upd_Events(object? sender, RoutedEventArgs e)
    {
        
    }

    private void Button_Add_Progress(object? sender, RoutedEventArgs e)
    {
        ProgressWindow windowprog = new ProgressWindow();
        windowprog.Show();
    }

    private void Button_Upd_Progress(object? sender, RoutedEventArgs e)
    {
        
    }

    private void Button_Add_Safety(object? sender, RoutedEventArgs e)
    {
        SafetyWindow windowsafe = new SafetyWindow();
        windowsafe.Show();
    }

    private void Button_Upd_Safety(object? sender, RoutedEventArgs e)
    {
        
    }

    private void Update_Table(object? sender, RoutedEventArgs e)
    {
        //GenerateColumnsForMonth();
    }

    private void ComboBoxMonth_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        GenerateColumnsForMonth();
    }

    private void Button_Add_Order(object? sender, RoutedEventArgs e)
    {
        
    }
}
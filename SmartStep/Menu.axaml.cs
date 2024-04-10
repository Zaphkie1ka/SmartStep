using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SmartStep.Models;

namespace SmartStep;

public partial class Menu : Window
{
    private List<Students> _students;
    private List<Teachers> _teachers;
    private List<Safety> _safety;
    private List<Events> _events;
    private List<Achievements> _achievements;
    private List<LessonRecord> _lesson;
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
        
    }
    private void GenerateColumnsForMonth()
    {
        MainGrid.Columns.Clear();
        DataGridTextColumn studentColumn = new DataGridTextColumn
        {
            Header = "ФИО ученика",
            Binding = new Binding("Name")
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
            Binding = new Binding("Date"),
        };
        MainGrid.Columns.Add(dateColumn);
        DataGridTextColumn descriptionColumn = new DataGridTextColumn
        {
            Header = "Содержание занятия",
            Binding = new Binding("Description"),
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
    private void Button_OnCommand(object? sender, RoutedEventArgs e)
    {
        Command window3 = new Command();
        window3.Show();
    }
}
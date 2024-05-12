using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace SmartStep;

public partial class StudentWindow : Window
{
    public StudentWindow()
    {
        InitializeComponent();
    }

    private void Add_Button(object? sender, RoutedEventArgs e)
    {
        
    }

    private void Ext_Button(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
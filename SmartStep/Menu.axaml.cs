using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace SmartStep;

public partial class Menu : Window
{
    public Menu()
    {
        InitializeComponent();
    }
    
    private void Button_OnCommand(object? sender, RoutedEventArgs e)
    {
        Command window3 = new Command();
        window3.Show();
    }
}
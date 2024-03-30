using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace SmartStep;

public partial class Command : Window
{
    private Menu menus = new Menu();
    public Command()
    {
        InitializeComponent();
    }

    public void Aboba()
    {
        if (menus.TabIndex == 1)
        {
            //данные добавляются на первую страницу
        }
    }

    private void Button_Add(object? sender, RoutedEventArgs e)
    {
        
    }
    private void Button_Update(object? sender, RoutedEventArgs e)
    {
        
    }
    private void Button_Delete(object? sender, RoutedEventArgs e)
    {
        
    }
    private void Button_Exit(object? sender, RoutedEventArgs e)
    {
        Hide();
    }
}
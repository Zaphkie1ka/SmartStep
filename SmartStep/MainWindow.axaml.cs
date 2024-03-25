using System.Data;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;
using Tmds.DBus.Protocol;

namespace SmartStep;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void LogIn_Click(object? sender, RoutedEventArgs e)
    {
        /*string LoginUser = loginbox.Text;
        string PassUser = passbox.Text;
        DBHelper db = new DBHelper();
        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        MySqlCommand command =
            new MySqlCommand("SELECT * FROM 'Teachers' WHERE 'Login'= @uL AND 'Password'= @uP");
        command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = LoginUser;
        command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = PassUser;
        adapter.SelectCommand = command;
        adapter.Fill(table);*/
    }

    private void Guest_Click(object? sender, RoutedEventArgs e)
    {
        
    }
}
using System.Data;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using MySql.Data.MySqlClient;
using SmartStep.Models;
using Tmds.DBus.Protocol;

namespace SmartStep;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void LogIn()
    {
        /*string LoginUser = loginbox.Text;
        string PassUser = passbox.Text;
        DBHelper db = new DBHelper();
        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        var connection = new MySqlConnection(db._connectionString.ConnectionString);
        connection.Open();
        MySqlCommand command = new MySqlCommand("SELECT * FROM teachers WHERE `Login`= @userL AND `Password`= @userP");
        command.Parameters.Add("@userL", MySqlDbType.VarChar).Value = LoginUser;
        command.Parameters.Add("@userP", MySqlDbType.VarChar).Value = PassUser;
        adapter.SelectCommand = command;
        adapter.Fill(table);
        if (table.Rows.Count > 0)
        {
            window2.Show();
            Close();
        }
        else
        {
            MessageBoxManager.GetMessageBoxStandard("Внимание!", "Неправильный пароль!");
        }*/
        Menu window2 = new Menu();
        window2.Show();
    }
    private void LogIn_Click(object? sender, RoutedEventArgs e)
    {
        LogIn();
    }

    private void Guest_Click(object? sender, RoutedEventArgs e)
    {
        
    }
}
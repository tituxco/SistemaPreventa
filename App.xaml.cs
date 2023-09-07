using Microsoft.Extensions.Hosting;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.Xaml;
using MySqlConnector;

namespace SistemaPreventa
{
    public partial class App : Application
    {
        public static DatabaseConfigSqlite DatabaseConfigSqlite { get;set; }

        protected override void OnStart()
        {
            // Configura y ejecuta el servicio en segundo plano
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<SyncService>();
                })
                .Build();

            host.Start();
        }
        public App( DatabaseConfigSqlite databaseConfigSqlite)
        {
            InitializeComponent();
            MainPage = new LoginPage();
            DatabaseConfigSqlite = databaseConfigSqlite;
        }
    }
}

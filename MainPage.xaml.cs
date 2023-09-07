using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using MySqlConnector;
//using MySql.Data.MySqlClient;

namespace SistemaPreventa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : FlyoutPage
    {
        //private string _dbConnectionString;

        public MainPage()
        {
            InitializeComponent();
            if (UsuarioConectado.Usuario.privilegios == "VENDEDOR")
            {
                this.Flyout = new MenuPage();
                this.Detail = new NavigationPage(new HomePage());
            }else if (UsuarioConectado.Usuario.privilegios == "ADMINISTRADOR")
            {
                this.Flyout = new MenuPageAdministrador();
                this.Detail = new NavigationPage(new HomePageAdministrador());
            }           
        }
    }
}

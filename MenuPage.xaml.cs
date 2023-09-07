using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace SistemaPreventa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
            //lblDatosMenuPage.Text = "Vendedor: <" + UsuarioConectado.VendedorSeleccionado.id.ToString() + "> " + UsuarioConectado.VendedorSeleccionado.apellido + ", " + UsuarioConectado.VendedorSeleccionado.id;
            lblDatosMenuPage.Text += "\n" + "Datos: " + FileAccessHelper.GetLocalFilePath("sistemapreventa.db3");
        }

        

        private void btnAdministracion_Clicked(object sender, EventArgs e)
        {       
        }
        private void btnInicio_Clicked(object sender, EventArgs e)
        {
            var mainPage = (MainPage)Application.Current.MainPage;
            if (UsuarioConectado.Usuario.privilegios == "VENDEDOR") 
            { 
                mainPage.Detail = new NavigationPage(new HomePage()); 
            }else if (UsuarioConectado.Usuario.privilegios=="ADMINISTRADOR")
            {
                mainPage.Detail = new NavigationPage(new HomePageAdministrador());
            }                                    
        }

        private void btnPedidos_Clicked(object sender, EventArgs e)
        {
            var mainPage = (MainPage)Application.Current.MainPage;
            if (UsuarioConectado.Usuario.privilegios == "VENDEDOR")
            {
                mainPage.Detail = new NavigationPage(new PedidosPage());
            }
            else if (UsuarioConectado.Usuario.privilegios == "ADMINISTRADOR")
            {
                mainPage.Detail = new NavigationPage(new HomePageAdministrador());
            }

            // mainPage.IsPresented = false; // Ocultar el FlyoutMenu
        }
        private void btnPrductos_Clicked(object sender, EventArgs e)
        {
            var mainPage = (MainPage)Application.Current.MainPage;
            if (UsuarioConectado.Usuario.privilegios == "VENDEDOR")
            {
                mainPage.Detail = new NavigationPage(new ProductosPage());
            }
            else if (UsuarioConectado.Usuario.privilegios == "ADMINISTRADOR")
            {
                mainPage.Detail = new NavigationPage(new HomePageAdministrador());
            }

            
            // mainPage.IsPresented = false; // Ocultar el FlyoutMenu
        }

        private void btnClientes_Clicked(object sender, EventArgs e)
        {
            var mainPage = (MainPage)Application.Current.MainPage;
            if (UsuarioConectado.Usuario.privilegios == "VENDEDOR")
            {
                mainPage.Detail = new NavigationPage(new ClientesPage());
            }
            else if (UsuarioConectado.Usuario.privilegios == "ADMINISTRADOR")
            {
                mainPage.Detail = new NavigationPage(new HomePageAdministrador());
            }
           
            // mainPage.IsPresented = false; // Ocultar el FlyoutMenu
        }
    }
}

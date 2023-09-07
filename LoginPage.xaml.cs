using Microsoft.Maui.Controls;
using System.Security.Cryptography;
using System.Text;

namespace SistemaPreventa
{
    public partial class LoginPage : ContentPage
    {
       // private readonly INavigation _navigation;
        public LoginPage()
        {
            InitializeComponent();
            DatabaseConfig.CheckDatabaseConnectionAutorizacion();
            //_navigation = navigation;
        }

        private async void LoginButton_Click(object sender, System.EventArgs e)
        {
            try {
                activityIndicator.IsRunning = true;
                string username = txtUsername.Text.ToLower();
                string password = DatabaseConfig.GetMd5Hash(txtPassword.Text);

                // Aquí realizas la verificación de las credenciales con la base de datos
                // y el resto de la lógica de autenticación

                // Ejemplo básico de verificación (reemplaza con tu propia lógica):
                //if (DatabaseConfig.ComprobarUsuarioValido(username, password))
                if (await DatabaseConfig.ComprobarUsuarioAutorizado(username, password))
                {
                    // Autenticación exitosa, redirigir al siguiente formulario o página
                    //DisplayAlert("Authentication Successful", "Redirecting to the main page.", "OK");
                    UsuarioConectado.Usuario = await DatabaseConfig.ObtenerDatosUsuario(username, password);
                    UsuarioConectado.Empresa = DatabaseConfig.ObtenerDatosEmpresa();                    
                    DatosComunes.configFiscal = await DatabaseConfig.ObtenerConfigFiscal();
                    DatosComunes.confGenerales=await DatabaseConfig.ObtenerConfiguraciones();
                    DatosComunes.localidades=await DatabaseConfig.ObtenerLocalidades();
                    DatosComunes.tiposContribuyentes=await DatabaseConfig.ObtenerTiposContibuyente();
                    DatosComunes.listasPrecios = await DatabaseConfig.ObtenerListasPrecio();
                    DatosComunes.categoriaInsumos = await DatabaseConfig.ObtenerCategoriasInsumos();                    
                    DatosComunes.idAlmacen = UsuarioConectado.Usuario.idAlmacen;
                    DatosComunes.idCaja = 3;
                    DatosComunes.ptoVta = 3;

                    if (UsuarioConectado.Usuario!= null && UsuarioConectado.Empresa!=null &&
                        DatosComunes.configFiscal!=null && DatosComunes.confGenerales !=null &&
                        DatosComunes.localidades!=null)
                    {
                        //dejamos la seleccion del vendedor para la pagina home
                        if (UsuarioConectado.Usuario.privilegios == "VENDEDOR" || UsuarioConectado.Usuario.privilegios == "ADMINISTRADOR")
                        {
                            Application.Current.MainPage = new MainPage();
                        }
                        else 
                        {
//                            Application.Current.MainPage = new ;
                            await DisplayAlert("Mensaje","Error en el tipo de usuario, contacte al administrador","Ok");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudieron cargar algunos datos necesarios para el funcionamiento del sistema", "ok");
                    }
                }
                else
                {
                    // Credenciales inválidas, mostrar mensaje de error
                    lblErrorMessage.Text = "Usuario invalido o no activado";
                }
            }catch (Exception ex)
            {
                lblErrorMessage.Text = ex.Message;
            }
            finally { activityIndicator.IsRunning = false; }
        }

        private void ForgotPassword_Tapped(object sender, System.EventArgs e)
        {
            // Lógica para restablecer la contraseña
            DisplayAlert("Forgot Password", "Implement password reset logic here.", "OK");
        }        
    }
}

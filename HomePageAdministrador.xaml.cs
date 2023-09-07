namespace SistemaPreventa;

public partial class HomePageAdministrador : ContentPage
{
	public HomePageAdministrador()
	{
		InitializeComponent();

        lblNombreFantasia.Text = UsuarioConectado.Empresa.nombrefantasia;
        lblRazonSocial.Text = UsuarioConectado.Empresa.razonsocial;
        lblDireccion.Text = UsuarioConectado.Empresa.direccion;
        lblLocalidad.Text = UsuarioConectado.Empresa.localidad;


        // Cargar y mostrar la imagen del logo si está disponible
        if (UsuarioConectado.Empresa.logo != null)
        {
            imgLogo.Source = ImageSource.FromStream(() => new MemoryStream(UsuarioConectado.Empresa.logo));
        }
    }
}
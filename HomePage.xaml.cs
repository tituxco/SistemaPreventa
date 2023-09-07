namespace SistemaPreventa;

public partial class HomePage : ContentPage
{
    DatabaseConfigSqlite databaseConfigSqlite = new DatabaseConfigSqlite(DatosComunes.BaseDeDatosLocal);
    public HomePage()
	{
		InitializeComponent();
		//DatabaseConfig.CheckDatabaseConnection();        

        if (UsuarioConectado.Empresa != null)
        {
            // Mostrar los datos de la empresa en los elementos de la página

            UsuarioConectado.Vendedores = DatabaseConfig.ObtenerDatosVendedor(UsuarioConectado.Usuario.idvendedor);
            if (UsuarioConectado.VendedorSeleccionado != null)
            {
                pckListaVendedores.ItemsSource = UsuarioConectado.Vendedores;
                int idv = UsuarioConectado.Vendedores.FindIndex(v => v.id == UsuarioConectado.VendedorSeleccionado.id);
                pckListaVendedores.SelectedIndex = idv;
                //lblPedidosPendientes.Text = "Pedidos pendientes de envio: " + databaseConfigSqlite.ObtenerPedidosLocales().Where(pedidos => pedidos.observaciones.Contains("LOCAL")).ToList().Count();
            }
            else
            {
                pckListaVendedores.ItemsSource = UsuarioConectado.Vendedores;
                pckListaVendedores.SelectedIndex = 0;
            }
                
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

    private void pckListaVendedores_SelectedIndexChanged(object sender, EventArgs e)
    {
        var vendedorSeleccionado = pckListaVendedores.SelectedItem as fact_vendedor;
        UsuarioConectado.VendedorSeleccionado = vendedorSeleccionado;
        //lblPedidosPendientes.Text = "Pedidos pendientes de envio: " + databaseConfigSqlite.ObtenerPedidosLocales().Where(pedidos => pedidos.observaciones.Contains("LOCAL")).ToList().Count();
    }
}
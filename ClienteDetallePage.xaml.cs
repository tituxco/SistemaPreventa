namespace SistemaPreventa;

public partial class ClienteDetallePage : ContentPage
{
    private fact_clientes _cliente;
    public ClienteDetallePage(fact_clientes clienteDetalle)
    {
        InitializeComponent();

        _cliente = clienteDetalle;
        if (_cliente != null)
        {
            CargarDatosCliente();
        }
        else
        {
            localidadesPicker.ItemsSource = DatosComunes.localidades;
            tipoContribuyentePicker.ItemsSource = DatosComunes.tiposContribuyentes;
            listaPreciosPicker.ItemsSource = DatosComunes.listasPrecios;
            int idlista = DatosComunes.listasPrecios.FindIndex(lista => lista.id == UsuarioConectado.VendedorSeleccionado.listaPrecios);
            listaPreciosPicker.SelectedIndex = idlista;
        }
    }
    private void CargarDatosCliente()
    {      
        // Establecer los valores iniciales en los campos
        nombreEntry.Text = _cliente.nomapell_razon;
        domicilioEntry.Text = _cliente.dir_domicilio;     
        
        localidadesPicker.ItemsSource = DatosComunes.localidades;        
        int idLocalidad = DatosComunes.localidades.FindIndex(localidad => localidad.id == _cliente.dir_localidad);
        localidadesPicker.SelectedIndex = idLocalidad;

        tipoContribuyentePicker.ItemsSource = DatosComunes.tiposContribuyentes;
        int idIva = DatosComunes.tiposContribuyentes.FindIndex(tipo => tipo.id == _cliente.iva_tipo);
        tipoContribuyentePicker.SelectedIndex = idIva;

        cuitEntry.Text = _cliente.cuit;
        telefonoEntry.Text= _cliente.telefono;
        contactoEntry.Text = _cliente.contacto;
        celularEntry.Text = _cliente.celular;
        emailEntry.Text = _cliente.email;
        observacionesEditor.Text= _cliente.observaciones;
        
        listaPreciosPicker.ItemsSource = DatosComunes.listasPrecios;
        int idlista = DatosComunes.listasPrecios.FindIndex(lista => lista.id == _cliente.lista_precios);
        listaPreciosPicker.SelectedIndex = idlista;

    }

    private async void Guardar_Clicked(object sender, EventArgs e)
    {
        if(_cliente == null) { _cliente=new fact_clientes(); }
        _cliente.nomapell_razon = nombreEntry.Text;
        _cliente.dir_domicilio = domicilioEntry.Text;

        var localidadSeleccionada = localidadesPicker.SelectedItem as fact_localidad;
        _cliente.dir_localidad = localidadSeleccionada.id;

        var tipoSeleccionado = tipoContribuyentePicker.SelectedItem as fact_ivatipo;
        _cliente.iva_tipo = tipoSeleccionado.id;

        _cliente.cuit = cuitEntry.Text ?? string.Empty;
        _cliente.telefono = telefonoEntry.Text ?? string.Empty;
        _cliente.contacto = contactoEntry.Text ?? string.Empty;
        _cliente.celular = celularEntry.Text ?? string.Empty;
        _cliente.email = emailEntry.Text ?? string.Empty;
        _cliente.observaciones = observacionesEditor.Text ?? string.Empty;
        _cliente.lista_precios = UsuarioConectado.VendedorSeleccionado.listaPrecios;
        _cliente.vendedor = UsuarioConectado.VendedorSeleccionado.id;

        if(await DatabaseConfig.GuardarCliente(_cliente))
        {
            await DisplayAlert("Mensaje", "Cliente guardado correctamente", "Ok");
            var mainPage = (MainPage)Application.Current.MainPage;
            mainPage.Detail = new NavigationPage(new ClientesPage());
        }
    }

    private void Cancelar_Clicked(object sender, EventArgs e)
    {
        var mainPage = (MainPage)Application.Current.MainPage;
        mainPage.Detail = new NavigationPage(new ClientesPage());
    }
}
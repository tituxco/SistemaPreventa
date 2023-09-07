using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using X.PagedList;
using static SQLite.SQLite3;


namespace SistemaPreventa;

public partial class PedidosPage : ContentPage
{
    private List<fact_Pedido> PedidosLocales;
    private IPagedList<fact_Pedido> pedidosPaginados;
    private int elementosPorPagina = 5; // Número de elementos a mostrar por página
    public int numeroPaginaActual; // Número de página actual
    public string tipoPedido;

    DatabaseConfigSqlite DatabaseConfigSqlite = new DatabaseConfigSqlite(FileAccessHelper.GetLocalFilePath("sistemaPreventa.db3"));

    private void MostrarPagina(int numeroPagina)
    {
        pedidosPaginados = PedidosLocales.ToPagedList(numeroPagina, elementosPorPagina);
        pedidosLocalesListView.ItemsSource = pedidosPaginados;
        numPagina.Text = "Pagina " + numeroPagina + " de " + pedidosPaginados.PageCount;
    }

    private void AnteriorButton_Clicked(object sender, EventArgs e)
    {
        if (pedidosPaginados.HasPreviousPage)
        {
            numeroPaginaActual--;
            MostrarPagina(numeroPaginaActual);
        }
    }

    private void SiguienteButton_Clicked(object sender, EventArgs e)
    {
        if (pedidosPaginados.HasNextPage)
        {
            numeroPaginaActual++;
            MostrarPagina(numeroPaginaActual);
        }
    }
    public PedidosPage()
	{

		InitializeComponent();

        if ( numeroPaginaActual==0)
        {
            numeroPaginaActual = 1;

        }        

        if (PedidoenCurso.clienteSeleccionado is not null)
        {
            lblClienteenCurso.Text = "Pedido en curso: " + PedidoenCurso.clienteSeleccionado.nomapell_razon;
        }

        if(tipoPedido== "Pedidos sincronizados")
        {
            PedidosLocales = DatabaseConfigSqlite.ObtenerPedidosLocales().Where(pedidos => !pedidos.observaciones.Contains("LOCAL")).ToList();            
            MostrarPagina(numeroPaginaActual);

        }
        else if (tipoPedido == "Pedidos sin sincronizar")
        {
            PedidosLocales = DatabaseConfigSqlite.ObtenerPedidosLocales().Where(pedidos => pedidos.observaciones.Contains("LOCAL")).ToList();            
            MostrarPagina(numeroPaginaActual);
        }
        else
        {
            PedidosLocales = DatabaseConfigSqlite.ObtenerPedidosLocales();
        }

        //PedidosLocales= DatabaseConfigSqlite.ObtenerPedidosLocales();

        MostrarPagina(numeroPaginaActual);
        //pedidosLocalesListView.ItemsSource = PedidosLocales;
    }
    private void tpClienteenCurso_Tapped(object sender, EventArgs e)
    {
        if (PedidoenCurso.clienteSeleccionado != null)
        {
            var mainPage = (MainPage)Application.Current.MainPage;
            mainPage.Detail = new NavigationPage(new PedidoActualDetalle());
        }
    }
    private async void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            activityIndicator.IsRunning = true;
            //PedidosLocales = DatabaseConfigSqlite.ObtenerPedidosLocales();
            Picker picker = (Picker)sender;
            tipoPedido = (string)picker.SelectedItem;
            if (tipoPedido == "Pedidos sincronizados")
            {
                PedidosLocales = DatabaseConfigSqlite.ObtenerPedidosLocales().Where(pedidos => !pedidos.observaciones.Contains("LOCAL")).ToList();
                numeroPaginaActual = 1;
                MostrarPagina(numeroPaginaActual);               
            }
            else if (tipoPedido == "Pedidos sin sincronizar")
            {
                PedidosLocales = DatabaseConfigSqlite.ObtenerPedidosLocales().Where(pedidos => pedidos.observaciones.Contains("LOCAL")).ToList();
                numeroPaginaActual = 1;
                MostrarPagina(numeroPaginaActual);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error al obtener pedidos: " + ex.Message, "Ok");
            activityIndicator.IsRunning = false;
        }
        finally
        {
            activityIndicator.IsRunning = false;
        }
    }


    private void btnBuscarPedido_Completed(object sender, EventArgs e)
    {

        BusquedaPedidos();
    }

    private void BusquedaPedidos()
    {
        if (txtBuscarPedido.Text != "")
        {
            PedidosLocales = DatabaseConfigSqlite.ObtenerPedidosLocalesBuscar(txtBuscarPedido.Text);
            //pedidosLocalesListView.ItemsSource = PedidosLocales;
            numeroPaginaActual = 1;
            MostrarPagina(numeroPaginaActual);
        }
        else
        {
            if (tipoPedido == "Pedidos sin sincronizar")
            {
                PedidosLocales = DatabaseConfigSqlite.ObtenerPedidosLocales().Where(pedidos => pedidos.observaciones.Contains("LOCAL")).ToList();
                //numeroPaginaActual = 1;                
                MostrarPagina(numeroPaginaActual);
            }
            else if (tipoPedido == "Pedidos sincronizados")
            {
                PedidosLocales = DatabaseConfigSqlite.ObtenerPedidosLocales().Where(pedidos => !pedidos.observaciones.Contains("LOCAL")).ToList();
                //numeroPaginaActual = 1;
                MostrarPagina(numeroPaginaActual);
            }               
            
        }
    }

    private async void pedidosLocalesListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        string action = await DisplayActionSheet("Que desea hacer?",cancel:"Cancelar", null, "Enviar", "Editar Pedido", "Eliminar");
        if (action == "Enviar")
        {
            bool respuesta = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Estás seguro que desea enviar el pedido al servidor?", "Sí", "No");

            if (respuesta)
            {
                if (e.Item is fact_Pedido)
                {
                    fact_Pedido pedidoSeleccionado = e.Item as fact_Pedido;
                    List<fact_itemsPedido> itemsPedidos = DatabaseConfigSqlite.ObtenerItemsPedidoLocal(pedidoSeleccionado.id);
                    if (await DatabaseConfig.SincronizarPedidoIndividual(pedidoSeleccionado, itemsPedidos, DatosComunes.ptoVta, 995, DatosComunes.idAlmacen, DatosComunes.idCaja))
                    {
                        await Toast.Make("El pedido se a enviado correctamente", ToastDuration.Short).Show();
                        BusquedaPedidos();
                        MostrarPagina(numeroPaginaActual);
                    }
                }
            }
        }
        else if (action == "Editar Pedido")
        {
            if (e.Item is fact_Pedido)
            {
                fact_Pedido pedidoSeleccionado = e.Item as fact_Pedido;
                PedidoenCurso.clienteSeleccionado = await DatabaseConfig.CargarClienteId(pedidoSeleccionado.id_cliente);
                PedidoenCurso.pedidoActual = pedidoSeleccionado;
                PedidoenCurso.itemsPedidoActual = DatabaseConfigSqlite.ObtenerItemsPedidoLocal(pedidoSeleccionado.id);

                var mainPage = (MainPage)Application.Current.MainPage;
                mainPage.Detail = new NavigationPage(new PedidoActualDetalle());
            }

        }
        else if (action == "Eliminar")
        {
            bool respuesta = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Estás seguro que desea eliminar este pedido?", "Sí", "No");

            if (respuesta)
            {
                if (e.Item is fact_Pedido)
                {
                    fact_Pedido pedidoSeleccionado = e.Item as fact_Pedido;
                    if (DatabaseConfigSqlite.EliminarPedidoLocal(pedidoSeleccionado))
                    {
                        await Toast.Make("El pedido se a eliminado correctamente", ToastDuration.Short).Show();
                        BusquedaPedidos();
                        MostrarPagina(numeroPaginaActual);
                    }
                }
            }
        }
        else
        {
            //await DisplayAlert("Mensaje", "No se detecto una acción válida", "Ok");
        }

    }
}
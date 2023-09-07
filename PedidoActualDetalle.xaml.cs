using NPOI.SS.Formula.Functions;
using System.Diagnostics;

namespace SistemaPreventa;

public partial class PedidoActualDetalle : ContentPage
{
	DatabaseConfigSqlite DatabaseConfigSqlite = new DatabaseConfigSqlite(FileAccessHelper.GetLocalFilePath("sistemaPreventa.db3"));
    public bool ModificarPedido;
	public PedidoActualDetalle()
	{
		InitializeComponent();
		CargarDatosPedidoActual();
		lblInformacionPedido.Text = "Importe total: $" + PedidoenCurso.pedidoActual.total; 
	}
	private void CargarDatosPedidoActual()
	{
		txtNombre.Text = PedidoenCurso.clienteSeleccionado.nomapell_razon;
		txtDireccion.Text= PedidoenCurso.clienteSeleccionado.dir_domicilio +"("+ PedidoenCurso.clienteSeleccionado.localidadTexto+")"; ;
		txtTelefono.Text= PedidoenCurso.clienteSeleccionado.celular;
		txtComentarios.Text = PedidoenCurso.pedidoActual.observaciones2;
		productosListView.ItemsSource = PedidoenCurso.itemsPedidoActual;
    }

    private void Finalizar_Clicked(object sender, EventArgs e)
    {
		try
		{            
                
            DatabaseConfigSqlite.EliminarPedidoLocal(PedidoenCurso.pedidoActual);                
                       
            PedidoenCurso.pedidoActual.observaciones2 = txtComentarios.Text ?? string.Empty;
            DatabaseConfigSqlite.GuardarPedidoLocal(PedidoenCurso.pedidoActual, PedidoenCurso.itemsPedidoActual);
            DisplayAlert("Pedido", "Se guardo el pedido en la base de datos local", "Ok");
            PedidoenCurso.clienteSeleccionado = null;
            PedidoenCurso.pedidoActual = new fact_Pedido();
            PedidoenCurso.itemsPedidoActual = new List<fact_itemsPedido>();
            var mainPage = (MainPage)Application.Current.MainPage;
            mainPage.Detail = new NavigationPage(new PedidosPage());

        }
        catch (Exception ex) 
		{ 
			Debug.WriteLine("error al guardar:" +ex.Message);
		}
		finally
		{

		}
    }
    private void Cancelar_Clicked(object sender, EventArgs e)
    {
        PedidoenCurso.clienteSeleccionado=null;
        PedidoenCurso.pedidoActual = new fact_Pedido();
        PedidoenCurso.itemsPedidoActual=new List<fact_itemsPedido>();

        var mainPage = (MainPage)Application.Current.MainPage;
        mainPage.Detail = new NavigationPage(new PedidosPage());
        // mainPage.IsPresented = false; // Ocultar el FlyoutMenu
    }

    private void btnGuardar_Clicked(object sender, EventArgs e)
    {
        PedidoenCurso.pedidoActual.observaciones2 = txtComentarios.Text ?? string.Empty;
        DisplayAlert("Mensaje", "Se guardaron las observaciones del pedido", "Ok");
    }

    private void btnAddProductos_Clicked(object sender, EventArgs e)
    {
        var mainPage = (MainPage)Application.Current.MainPage;
        mainPage.Detail = new NavigationPage(new ProductosPage());
        // mainPage.IsPresented = false; // Ocultar el FlyoutMenu
    }

    
    private async void productosListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        fact_itemsPedido insumoSeleccionado =e.Item as fact_itemsPedido;        
        string action = await DisplayActionSheet("Que desea hacer?", cancel: "Cancelar", null, "Modificar Item", "Eliminar Item");
        if (action == "Modificar Item")
        {

            fact_itemsPedido result = await MauiPopup.PopupAction.DisplayPopup<fact_itemsPedido>(new ProductoPedidoPop(null,insumoSeleccionado));


            //string cantidad = await DisplayPromptAsync("Agregar producto", "Cantidad:", initialValue: insumoSeleccionado.cantidad.ToString(), maxLength: 4, placeholder: "Ingrese un valor numérico", accept: "Aceptar", cancel: "Cancelar");
            //if (decimal.TryParse(cantidad, out decimal nuevaCantidad))
            //{

            insumoSeleccionado.cantidad = result.cantidad;
            insumoSeleccionado.punit = result.punit;
            insumoSeleccionado.ptotal = result.ptotal;//nuevaCantidad * insumoSeleccionado.punit;
                
            productosListView.ItemsSource = new List<fact_itemsPedido>(PedidoenCurso.itemsPedidoActual);
            PedidoenCurso.pedidoActual.subtotal = PedidoenCurso.itemsPedidoActual.Sum(item => item.ptotal);
            PedidoenCurso.pedidoActual.total = PedidoenCurso.pedidoActual.subtotal;
            lblInformacionPedido.Text = "Importe total: $" + PedidoenCurso.pedidoActual.total;            
        }
        else if (action=="Eliminar Item")
        {
            PedidoenCurso.itemsPedidoActual.Remove(insumoSeleccionado);
            productosListView.ItemsSource = new List<fact_itemsPedido>(PedidoenCurso.itemsPedidoActual);
            PedidoenCurso.pedidoActual.subtotal = PedidoenCurso.itemsPedidoActual.Sum(item => item.ptotal);
            PedidoenCurso.pedidoActual.total = PedidoenCurso.pedidoActual.subtotal;
            lblInformacionPedido.Text = "Importe total: " + PedidoenCurso.pedidoActual.total;
        }
    }
}
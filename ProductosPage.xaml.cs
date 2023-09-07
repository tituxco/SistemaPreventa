using NPOI.SS.Formula.Functions;
using CommunityToolkit.Maui.Alerts;
using System.Threading;
//using static System.Net.Mime.MediaTypeNames;
using CommunityToolkit.Maui.Core;

namespace SistemaPreventa;

public partial class ProductosPage : ContentPage
{
    public List<fact_insumos> Productos;
    public ProductosPage()
	{
		InitializeComponent();
        if (PedidoenCurso.clienteSeleccionado is not null)
        {
            lblClienteenCurso.Text = "Pedido en curso: " + PedidoenCurso.clienteSeleccionado.nomapell_razon;
        }

        pckCategoriasInsumos.ItemsSource = DatosComunes.categoriaInsumos;



	}
    private async void CargarProductosAsync()
    {
        try
        {
            activityIndicator.IsRunning = true; // Mostrar la barra de estado
            string cat = "%";
            fact_categoria_insum categoria = pckCategoriasInsumos.SelectedItem as fact_categoria_insum;
            if (categoria == null) { cat = "%"; } else { cat=categoria.id.ToString(); }    
            Productos = await DatabaseConfig.CargarProductos(insumoEntry.Text, cat);
            // Actualizar el origen de datos del ListView
            productosListView.ItemsSource = Productos; /// HAY QUE IMPLEMENTAR QUE NO MUESTRE LAS CATEGORIAS IGNORADAS, ES DECIR LAS QUE NO QUIEREN QUE SE VENDAN EN LA CALLE


        }
        catch (Exception ex)
        {
            // Manejar cualquier excepción que pueda ocurrir durante la consulta
            Console.WriteLine($"Error al cargar los clientes: {ex.Message}");
        }
        finally
        {
            activityIndicator.IsRunning = false; // Ocultar la barra de estado
        }
    }

    private void insumoEntry_Completed(object sender, EventArgs e)
    {
        CargarProductosAsync();
    }

    private async  void productosListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        try
        {
            if (PedidoenCurso.clienteSeleccionado != null)
            {


                fact_insumos insumoSeleccionado = e.Item as fact_insumos;                

                fact_itemsPedido result = await MauiPopup.PopupAction.DisplayPopup<fact_itemsPedido>(new ProductoPedidoPop(insumoSeleccionado,null));

                if (result != null)
                {
                    PedidoenCurso.itemsPedidoActual.Add(result);
                    PedidoenCurso.pedidoActual.subtotal += result.ptotal;
                    PedidoenCurso.pedidoActual.iva21 += 0;
                    PedidoenCurso.pedidoActual.iva105 += 0;
                    PedidoenCurso.pedidoActual.otroiva += 0;
                    PedidoenCurso.pedidoActual.total += result.ptotal;
                    await Toast.Make("Item " + result.descripcion +" agregado correctamente" , ToastDuration.Short).Show();
                }
            }
        }catch (Exception ex)
        {
            await DisplayAlert("Error", "error al agregar producto! " + ex.Message, "Ok");
        }

    }

    private void pckCategoriasInsumos_SelectedIndexChanged(object sender, EventArgs e)
    {
        CargarProductosAsync();
    }

    private void tpClienteenCurso_Tapped(object sender, EventArgs e)
    {
        if (PedidoenCurso.clienteSeleccionado != null)
        {
            var mainPage = (MainPage)Application.Current.MainPage;
            mainPage.Detail = new NavigationPage(new PedidoActualDetalle());
        }
    }
}
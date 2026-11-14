using NPOI.SS.Formula.Functions;
using CommunityToolkit.Maui.Alerts;
using System.Threading;
//using static System.Net.Mime.MediaTypeNames;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Views;
using MathNet.Numerics.Optimization;

namespace SistemaPreventa;

public partial class ProductosPage : ContentPage
{
    public List<fact_insumos> Productos;
    public List<fact_categoria_insum> CategoriasInsumos;
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
            CategoriasInsumos = DatosComunes.categoriaInsumos;
            activityIndicator.IsRunning = true; // Mostrar la barra de estado
            string cat = "%";
            string listacategorias = string.Join(",", CategoriasInsumos.Select (m=>m.id.ToString()).ToArray());
           // await DisplayAlert("alerta",listacategorias,"ok");
            fact_categoria_insum categoria = pckCategoriasInsumos.SelectedItem as fact_categoria_insum;
            if (categoria == null) { cat = listacategorias; } else { cat=categoria.id.ToString(); }    
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

    private async void productosListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        try
        {
            // 1. Limpiar la selección de la ListView para poder volver a tocar el mismo elemento
            ((ListView)sender).SelectedItem = null;

            // 2. Controlar que el elemento casteado no sea nulo
            if (e.Item is not fact_insumos insumoSeleccionado)
                return;

            if (PedidoenCurso.clienteSeleccionado != null)
            {

                var configStock = DatosComunes.confGenerales.FirstOrDefault(c => c.id == 79); // (Ajusta 'id' al nombre de tu propiedad)

                // 3. Usar '<= 0' para cubrir casos donde el stock quede en negativo
                if (insumoSeleccionado.stock_calculado <= 0 && configStock?.valor == "1")
                {
                    await DisplayAlert("Restricción de producto", "No se puede agregar un producto sin stock", "Ok");
                    return; // Detiene la ejecución aquí
                }

                fact_itemsPedido result = await MauiPopup.PopupAction.DisplayPopup<fact_itemsPedido>(new ProductoPedidoPop(insumoSeleccionado, null));

                if (result != null)
                {
                    PedidoenCurso.itemsPedidoActual.Add(result);
                    PedidoenCurso.pedidoActual.subtotal += result.ptotal;
                    PedidoenCurso.pedidoActual.iva21 += 0;
                    PedidoenCurso.pedidoActual.iva105 += 0;
                    PedidoenCurso.pedidoActual.otroiva += 0;
                    PedidoenCurso.pedidoActual.total += result.ptotal;

                    await Toast.Make("Item " + result.descripcion + " agregado correctamente", ToastDuration.Short).Show();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error al agregar producto: " + ex.Message, "Ok");
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
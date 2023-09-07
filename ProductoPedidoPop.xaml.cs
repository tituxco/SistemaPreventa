using MauiPopup;
using MauiPopup.Views;
namespace SistemaPreventa;

public partial class ProductoPedidoPop : BasePopupPage
{
    fact_itemsPedido _items;
    fact_insumos _insumos;
    public ProductoPedidoPop(fact_insumos insumoSeleccionado, fact_itemsPedido itemSeleccionado)
    {
        InitializeComponent();
        _insumos = insumoSeleccionado;
        _items = itemSeleccionado;

        if (insumoSeleccionado != null)
        {

            txtPrecio.Text = insumoSeleccionado.precioVenta.ToString();
        }
        if (itemSeleccionado != null)
        {
            txtPrecio.Text = itemSeleccionado.punit.ToString();
            txtCantidad.Text = itemSeleccionado.cantidad.ToString();
        }

    }

    private async void Cancelar_Clicked(object sender, EventArgs e)
    {
        await PopupAction.ClosePopup(null);
    }

    private async void Aceptar_Clicked(object sender, EventArgs e)
    {
        string cantidad = txtCantidad.Text;
        string precio = txtPrecio.Text;

        fact_itemsPedido itemsPedido = new fact_itemsPedido();
        if (decimal.TryParse(cantidad, out decimal cantidadProd) && decimal.TryParse(precio, out decimal precioMod))
        {
            if (_insumos != null)
            {
                itemsPedido.cantidad = cantidadProd;
                itemsPedido.cod = _insumos.codigo;
                itemsPedido.codint = _insumos.id.ToString();
                itemsPedido.descripcion = _insumos.descripcion;
                itemsPedido.punit = precioMod;
                itemsPedido.ptotal = Math.Round(cantidadProd * precioMod, 2);
                itemsPedido.iva = _insumos.iva;
                itemsPedido.id_fact = 0;
                await PopupAction.ClosePopup(itemsPedido);
            }
            if (_items!=null)
            {
                itemsPedido.cantidad = cantidadProd;
                itemsPedido.cod = _items.cod;
                itemsPedido.codint = _items.codint;
                itemsPedido.descripcion = _items.descripcion;
                itemsPedido.punit = precioMod;
                itemsPedido.ptotal = Math.Round(cantidadProd * precioMod, 2);
                itemsPedido.iva = _items.iva;
                itemsPedido.id_fact = 0;
                await PopupAction.ClosePopup(itemsPedido);
            }
        }
    }
}
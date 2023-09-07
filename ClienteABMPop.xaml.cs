using MauiPopup.Views;

namespace SistemaPreventa;

public partial class ClienteABMPop : BasePopupPage
{
	fact_clientes _cliente;
	public ClienteABMPop(fact_clientes clienteSeleccionado)
	{
		
		InitializeComponent();
		_cliente = clienteSeleccionado;
		if (_cliente != null)
		{
			entryCelular.Text = _cliente.celular;
			entryCodClie.Text = _cliente.codClie;
			entryContacto.Text = _cliente.contacto;
			entryCuit.Text = _cliente.cuit;
			entryDirDomicilio.Text = _cliente.dir_domicilio;
			entryEmail.Text = _cliente.email;
			entryNomApellRazon.Text = _cliente.nomapell_razon;
			entryObservaciones.Text = _cliente.observaciones;
			
		}
	}

    private void btnGuardar_Clicked(object sender, EventArgs e)
    {

    }

    private void btnCancelar_Clicked(object sender, EventArgs e)
    {

    }
}
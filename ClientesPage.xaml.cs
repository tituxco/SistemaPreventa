using NPOI.POIFS.FileSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.WebPages;


namespace SistemaPreventa
{
    public partial class ClientesPage : ContentPage
    {
        public List<fact_clientes> Clientes;
        public ClientesPage()
        {
            InitializeComponent();
            //Clientes = new List<fact_clientes>();                        
            //CargarClientesAsync();
            pckSeleccionarLocalidad.ItemsSource = DatosComunes.localidades;
        }
       
        private async Task CargarClientesAsync()
        {
            try
            {
                
                activityIndicator.IsRunning = true; // Mostrar la barra de estado
                string loca = "%";
                string busqueda = "%";
                
                if (!clienteBuscar.Text.IsEmpty()) { busqueda = clienteBuscar.Text; }

                fact_localidad localidad = pckSeleccionarLocalidad.SelectedItem  as fact_localidad;
                if (localidad == null) { loca = "%"; } else { loca = localidad.id.ToString(); }
 
               
                // Realizar la consulta de manera asíncrona
                if (UsuarioConectado.Usuario.privilegios == "VENDEDOR")
                {
                    
                    Clientes = await DatabaseConfig.CargarClientesVendedor(loca,UsuarioConectado.VendedorSeleccionado.id, busqueda );
                    
                }
                else
                {
                    Clientes = await DatabaseConfig.CargarClientes(clienteBuscar.Text);
                }


                // Actualizar el origen de datos del ListView
                clientesListView.ItemsSource = Clientes;
                
                //clientesCarrousel.ItemsSource = Clientes;
                
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

        private async void clientesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            string action = await DisplayActionSheet("Que desea hacer?", "Cancelar", null, "Nuevo pedido", "Editar Cliente", "Eliminar");
            //Debug.WriteLine("Action: " + action);
            if (action == "Editar Cliente")
            {
                if (e.Item is fact_clientes)
                {
                    fact_clientes clienteSeleccionado = e.Item as fact_clientes;
                    var mainPage = (MainPage)Application.Current.MainPage;
                    mainPage.Detail = new NavigationPage(new ClienteDetallePage(clienteSeleccionado));
                }
            }else if (action =="Nuevo pedido")
            {
                PedidoenCurso.clienteSeleccionado = e.Item as fact_clientes;
                
                var mainPage = (MainPage)Application.Current.MainPage;
                mainPage.Detail = new NavigationPage(new ProductosPage());


                PedidoenCurso.pedidoActual.condvta = 1;
                PedidoenCurso.pedidoActual.localidad = PedidoenCurso.clienteSeleccionado.localidadTexto;
                PedidoenCurso.pedidoActual.cuit = PedidoenCurso.clienteSeleccionado.cuit;
                PedidoenCurso.pedidoActual.direccion = PedidoenCurso.clienteSeleccionado.dir_domicilio;
                PedidoenCurso.pedidoActual.fecha = DateTime.Now;
                PedidoenCurso.pedidoActual.id_cliente = PedidoenCurso.clienteSeleccionado.idclientes;
                PedidoenCurso.pedidoActual.razon = PedidoenCurso.clienteSeleccionado.nomapell_razon;
                PedidoenCurso.pedidoActual.vendedor = UsuarioConectado.VendedorSeleccionado.id;
                PedidoenCurso.pedidoActual.tipocontr = PedidoenCurso.clienteSeleccionado.tipoContr;
                PedidoenCurso.pedidoActual.subtotal = 0;
                PedidoenCurso.pedidoActual.iva105 = 0;
                PedidoenCurso.pedidoActual.iva21 = 0;
                PedidoenCurso.pedidoActual.total = 0;
                PedidoenCurso.pedidoActual.otroiva = 0;


            }else if(action=="Eliminar")
            {
                await DisplayAlert("Mensaje", "No esta autorizado", "Ok");
            }
        }

        private async void clienteBuscar_Completed(object sender, EventArgs e)
        {
            await CargarClientesAsync();
        }

        private async void btnClienteAgregar_Clicked(object sender, EventArgs e)
        {
            try
            {
                activityIndicator.IsRunning = true;

                var mainPage = (MainPage)Application.Current.MainPage;
                mainPage.Detail = new NavigationPage(new ClienteDetallePage(null));


            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error: " + ex.Message,"Ok");
                activityIndicator.IsRunning = false;
            }
            finally
            {
                activityIndicator.IsRunning = false;
            }
        }

        private async void pckSeleccionarLocalidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            await CargarClientesAsync();             
        }

        private void swpVisitaClienteRealizada_Invoked(object sender, EventArgs e)
        {           
            DisplayAlert("Ok", "invocado", "OK");
            //fact_clientes clienteSel = clientesCarrousel.CurrentItem as fact_clientes;                     
           // if (clienteSel != null) { DisplayAlert("cliente seleccionado", clienteSel.nomapell_razon,"OK"); }
        }

        private void swpPosponerVisitaCliente_Invoked(object sender, EventArgs e)
        {
            var clienteItem = sender as SwipeItem;
            if (clienteItem != null) { clienteItem.BackgroundColor = Colors.LightPink; }

        }
    }
}

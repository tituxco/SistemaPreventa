using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


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
        }
       
        private async Task CargarClientesAsync()
        {
            try
            {
                activityIndicator.IsRunning = true; // Mostrar la barra de estado

                // Realizar la consulta de manera asíncrona
                if (UsuarioConectado.Usuario.privilegios == "VENDEDOR")
                {
                    Clientes = await DatabaseConfig.CargarClientesVendedor(UsuarioConectado.VendedorSeleccionado.id, clienteBuscar.Text);
                }
                else
                {
                    Clientes = await DatabaseConfig.CargarClientes(clienteBuscar.Text);
                }
                

                // Actualizar el origen de datos del ListView
                clientesListView.ItemsSource = Clientes;
                
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
    }
}

using SQLite;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SistemaPreventa
{
    public class DatabaseConfigSqlite
    { 
        string _dbPath;
        public string StatusMensage { get; set; }

        private SQLiteConnection conn;

        private void Init()
        {
            if (conn is not null)            
                return;
                conn = new(_dbPath);
                conn.CreateTable<fact_Pedido>();
                conn.CreateTable<fact_itemsPedido>();            
        }

        public DatabaseConfigSqlite(string dbPath)
        {
            _dbPath = dbPath;
        }

        public void GuardarPedidoLocal(fact_Pedido pedido, List<fact_itemsPedido> pedidoItems)
        {
            int idPedido;
            try
            {
                Init();
                if (pedido!=null && pedidoItems != null)
                {
                    fact_Pedido ped = new fact_Pedido()
                    {
                        id_cliente = pedido.id_cliente,
                        condvta=pedido.condvta,
                        cuit=pedido.cuit,
                        direccion=pedido.direccion,
                        fecha=pedido.fecha,
                        localidad=pedido.localidad,
                        observaciones= "LOCAL",
                        observaciones2= pedido.observaciones2,
                        razon=pedido.razon,
                        subtotal=pedido.subtotal,
                        tipocontr=pedido.tipocontr,
                        total=pedido.total,
                        vendedor=pedido.vendedor,                       
                        iva105 = pedido.iva105,
                        iva21=pedido.iva21,
                        otroiva=pedido.otroiva,
                    };
                    conn.Insert(ped);
                    idPedido = conn.ExecuteScalar<int>("SELECT last_insert_rowid()");
                    Debug.WriteLine("IdPedido interno: " + idPedido);
                    foreach (fact_itemsPedido item in pedidoItems)
                    {
                        fact_itemsPedido pedidoItem = new fact_itemsPedido();
                        pedidoItem.id_fact = idPedido;
                        pedidoItem.cantidad = item.cantidad;
                        pedidoItem.cod = item.cod.ToString();
                        pedidoItem.ptotal = item.ptotal;
                        pedidoItem.punit = item.punit;
                        pedidoItem.descripcion = item.descripcion;
                        pedidoItem.codint = item.codint;
                        pedidoItem.iva = item.iva;                        
                        conn.Insert(pedidoItem);
                    }

                }
            }catch (Exception ex)
            {
                Debug.WriteLine("Error al guardar el detalle del pedido; " + ex.ToString());
            }
        }    
        public List<fact_Pedido> ObtenerPedidosLocales()
        {
            try
            {
                Init();
                return conn.Table<fact_Pedido>().Where(pedidos=>pedidos.vendedor==UsuarioConectado.VendedorSeleccionado.id).OrderByDescending(pedidos=>pedidos.id).ToList();

            }catch (Exception ex)
            {
                Debug.WriteLine("error al obtener datos: " + ex.Message);
            }
            return new List<fact_Pedido>();
        }
        public List<fact_Pedido> ObtenerPedidosLocalesBuscar(string cliente)
        {
            try
            {
                Init();
                return conn.Query<fact_Pedido>("select * from pedido where razon like '%" + cliente + "%'").Where(pedidos=>pedidos.vendedor==UsuarioConectado.VendedorSeleccionado.id).OrderByDescending(pedidos=>pedidos.id).ToList();                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("error al obtener datos: " + ex.Message);
            }
            return new List<fact_Pedido>();
        }
        public fact_Pedido ObtenerPedidoLocal(int idPedido)
        {
            try
            {
                Init();
                return conn.Table<fact_Pedido>().FirstOrDefault(pedido => pedido.id==idPedido);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("error al obtener datos: " + ex.Message);
            }
            return new fact_Pedido();
        }
        public List<fact_itemsPedido> ObtenerItemsPedidoLocal(int idPedido)
        {
            try
            {
                Init();
                return conn.Query<fact_itemsPedido>("select * from itemsPedido where id_fact =" + idPedido + "").ToList();
                //return conn.Table<fact_Pedido>().Where(pedidos => pedidos.razon.Contains("%" +cliente + "%")).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("error al obtener datos: " + ex.Message);
            }
            return new List<fact_itemsPedido>();
        }
        public void ActualizarDatosPedidoLocalSincro(fact_Pedido pedido, int numeroSincro)
        {
            try
            {
                Init();
                if (pedido != null)
                {
                    fact_Pedido ped = new fact_Pedido()
                    {
                        id=pedido.id,
                        id_cliente = pedido.id_cliente,
                        condvta = pedido.condvta,
                        cuit = pedido.cuit,
                        direccion = pedido.direccion,
                        fecha = pedido.fecha,
                        localidad = pedido.localidad,
                        observaciones = "SINCRO: " + numeroSincro,
                        observaciones2 = pedido.observaciones2,
                        razon = pedido.razon,
                        subtotal = pedido.subtotal,
                        tipocontr = pedido.tipocontr,
                        total = pedido.total,
                        vendedor = pedido.vendedor,
                        iva105 = pedido.iva105,
                        iva21 = pedido.iva21,
                        otroiva = pedido.otroiva,
                    };
                    conn.Update(ped);                    
                }
                //return true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("error al guardar datos: " + ex.Message);
                //return false;
            }
        }
        public bool EliminarPedidoLocal(fact_Pedido pedido)
        {
            try
            {
                Init();
                List<fact_itemsPedido> items = ObtenerItemsPedidoLocal(pedido.id);                
                foreach( fact_itemsPedido item in items)
                {
                    conn.Delete(item);
                }
                conn.Delete(pedido);

                return true;

                
                //return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("error al eliminar Pedido datos: " + ex.Message);
                return false;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPreventa
{
    public class VariablesGlobales
    {
        
    }
    public static class AuthServ
    {
        public static CliAuth UsuarioAutorizado { get; set; } = new CliAuth();
    }
    public static class UsuarioConectado
    {
        public static cm_usuarios Usuario { get; set; }
        public static List <fact_vendedor> Vendedores { get; set; }
        public static fact_empresa Empresa { get; set; }
        public static fact_vendedor VendedorSeleccionado { get; set; }
    }
    public class DatosComunes
    { 
        public static List<fact_localidad> localidades { get; set; }
        public static cm_doc_tipo docTipo { get; set; }
        public static cm_estado_civil estadoCivil { get; set; }
        public static cm_genero genero { get; set; }
        public static cm_nacionalidad nacionalidad { get; set; }
        public static cm_provincias provincias { get; set; }
        public static List<fact_conffiscal> configFiscal { get; set; }
        public static List<fact_categoria_insum> categoriaInsumos { get; set; }
        public static fact_comprobantes_tipo compTipos { get; set; }
        public static fact_condventas condventas { get; set; }
        public static List<fact_configuraciones> confGenerales { get; set;}
        public static List<fact_ivatipo> tiposContribuyentes { get; set; } 
        public static List<fact_listas_precio> listasPrecios { get; set; }
        public static int idCaja { get; set; }
        public static int idAlmacen { get; set; }
        public static int ptoVta { get; set; }
        public static string BaseDeDatosLocal { get; set; }

    }
    public class PedidoenCurso
    {
        public static fact_clientes clienteSeleccionado { get; set; }
        public static fact_Pedido pedidoActual { get; set; } = new fact_Pedido();
        public static List <fact_itemsPedido> itemsPedidoActual { get; set; }=new List<fact_itemsPedido>();

    }
}

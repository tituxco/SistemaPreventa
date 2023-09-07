using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPreventa
{
    [Table("Pedido")]
    public class fact_Pedido
    {
        [PrimaryKey,  AutoIncrement]
        public int id { get; set; }        
        public DateTime fecha { get; set; }
        public int id_cliente { get; set; }
        public string razon { get; set; }
        public string direccion { get; set; }
        public string localidad { get; set; }
        public string tipocontr { get; set; }
        public string cuit { get; set; }
        public int condvta { get; set; }
        public decimal subtotal { get; set; }
        public decimal iva105 { get; set; }
        public decimal iva21 { get; set; }
        public decimal otroiva { get; set; }
        public decimal total { get; set; }
        public int vendedor { get; set; }        
        public string observaciones { get; set; }
        public string observaciones2 { get; set; }        
    }

    [Table("itemsPedido")]
    public class fact_itemsPedido
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string cod { get; set; }        
        public decimal cantidad { get; set; }
        public string descripcion { get; set; }
        public decimal iva { get; set; }
        public decimal punit { get; set; }
        public decimal ptotal { get; set; }
        public int id_fact { get; set; }
        public string codint { get; set; }        
    }
}

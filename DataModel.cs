using System;

namespace SistemaPreventa
{
    public class CliAuth
    {
        public int id { get; set; }
        public string cliente { get; set; }
        public string sistema { get; set; }
        public string usuario { get; set; }
        public string pass { get; set; }
        public string servidor { get; set; }
        public int autorizado { get; set; }
        public string bd { get; set; }
        public string puerto { get; set; }
        public string clave { get; set; }
        public string codus { get; set; }
        public string servidor_resp { get; set; }
        public string modulo { get; set; }
        public int idInt { get; set; }
        public int especialidad { get; set; }
        public int actualizacionauth { get; set; }
        public string direccionactualiza { get; set; }
        public int disponibles { get; set; }
        public int debe { get; set; }
        public string mensaje { get; set; }
    }

    //clases del sistema KIGEST PyME
    public class ariel_itmlistacarga
    {
        public int id { get; set; }
        public int idcomprobante { get; set; }
        public string tipo_envase { get; set; }
        public string cantidad { get; set; }
        public string producto { get; set; }
        public string excedente { get; set; }
    }
    public class cm_archivos
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public byte[] archivo { get; set; }
    }

    public class cm_Asientos
    {
        public int id { get; set; }
        public int codigoAsiento { get; set; }
        public int cuentaDebeId { get; set; }
        public double importeDebe { get; set; }
        public int cuentaHaberId { get; set; }
        public double importeHaber { get; set; }
    }

    public class cm_doc_tipo
    {
        public int id { get; set; }
        public string nombre { get; set; }
    }
    public class cm_ejercicios_cerrados
    {
        public int id { get; set; }
        public int periodofiscal { get; set; }
        public string periodoCierre { get; set; }
    }

    public class cm_estado_civil
    {
        public int id { get; set; }
        public string nombre { get; set; }
    }

    public class cm_genero
    {
        public int id { get; set; }
        public string nombre { get; set; }
    }

    public class cm_libroDiario
    {
        public int id { get; set; }
        public string comprobanteInterno { get; set; }
        public int codigoAsiento { get; set; }
        public DateTime fecha { get; set; }
        public string concepto { get; set; }
        public double totalDebe { get; set; }
        public double totalHaber { get; set; }
        public int NumPartidas { get; set; }
    }

    public class cm_libroMayor
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public string concepto { get; set; }
        public int codigoAsiento { get; set; }
    }
   
    public class cm_nacionalidad
    {
        public int id { get; set; }
        public string nombre { get; set; }
    }

    public class cm_periodos_cerrados
    {
        public int id { get; set; }
        public string periodo { get; set; }
    }

    public class cm_pesoEspecifico
    {
        public int id { get; set; }
        public string peso { get; set; }
    }

    public class cm_planDeCuentas
    {
        public int id { get; set; }
        public string nombreCuenta { get; set; }
        public int grupo { get; set; }
        public int subGrupo { get; set; }
        public int cuenta { get; set; }
        public int subCuenta { get; set; }
        public int cuentaDetalle { get; set; }
        public byte cuentaMovimiento { get; set; }
        public byte cuentaResultado { get; set; }
    }

    public class cm_provincias
    {
        public int idprovincias { get; set; }
        public string nombre { get; set; }
    }

    public class cm_saldos_cuentas
    {
        public int id { get; set; }
        public int idCuenta { get; set; }
        public string periodo { get; set; }
        public double saldo { get; set; }
    }

    public class fact_balances_diarios
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public string total_ventas { get; set; }
        public string total_costo { get; set; }
        public string total_ganancia { get; set; }
        public string total_nocod { get; set; }
        public int id_almacen { get; set; }
        public int id_vendedor { get; set; }
        public TimeSpan fecha_mod { get; set; }
    }

    public class fact_balances_diarios_proveedor
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public string total_ventas { get; set; }
        public string total_costo { get; set; }
        public string total_ganancia { get; set; }
        public int id_proveedor { get; set; }
        public TimeSpan fecha_mod { get; set; }
    }
    public class cm_usuarios
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public string privilegios { get; set; }
        public int idAlmacen { get; set; }
        public int activo { get; set; }
        public string idvendedor { get; set; }
        public int idtecnico { get; set; }
        public string token { get; set; }
    }

    public class fact_vendedor
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public decimal comision { get; set; }
        public int activo { get; set; }
        public int listaPrecios { get; set; }
    }

    public class fact_cajas
    {
        public int id { get; set; }
        public string descripcion { get; set; }
    }

    public class fact_cajas_cierres
    {
        public int id { get; set; }
        public TimeSpan fecha { get; set; }
        public string monto { get; set; }
        public int caja { get; set; }
    }
    public class fact_categoria_insum
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int sincro { get; set; }
    }

    public class fact_cheques
    {
        public int id { get; set; }
        public int cliente { get; set; }
        public string banco { get; set; }
        public string sucursal { get; set; }
        public string serie { get; set; }
        public string importe { get; set; }
        public DateTime fecha_cobro { get; set; }
        public int estado_cheque { get; set; }
        public int comprobante { get; set; }
        public DateTime fecha_emision { get; set; }
        public int comprobante_eg { get; set; }
        public TimeSpan fecha_ultestado { get; set; }
        public int tipo_cheque { get; set; }
        public int cuenta { get; set; }
        public string observaciones { get; set; }
    }

    public class fact_cheques_estado
    {
        public int idcheques_estado { get; set; }
        public string estado { get; set; }
    }

    public class fact_clientes
    {
        public int idclientes { get; set; }
        public string nomapell_razon { get; set; }
        public string dir_domicilio { get; set; }
        public int dir_localidad { get; set; }
        public int iva_tipo { get; set; }
        public string cuit { get; set; }
        public string telefono { get; set; }
        public string contacto { get; set; }
        public string celular { get; set; }
        public string email { get; set; }
        public string observaciones { get; set; }
        public int lista_precios { get; set; }
        public string codClie { get; set; }
        public int vendedor { get; set; }
        public TimeSpan f_alta { get; set; }
        public TimeSpan f_mod { get; set; }
        public string localidadTexto { get; set; }
        public string tipoContr { get; set; }
    }

    public class fact_comprobantes_tipo
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public int disp { get; set; }
    }

    public class fact_condventas
    {
        public int id { get; set; }
        public string condicion { get; set; }
    }

    public class fact_conffiscal
    {
        public int id { get; set; }
        public int confnume { get; set; }
        public string donfdesc { get; set; }
        public string abrev { get; set; }
        public string tip { get; set; }
        public int leg { get; set; }
        public int ptovta { get; set; }
        public string debcred { get; set; }
        public string codfiscal { get; set; }
        public string letra { get; set; }
        public string observaciones { get; set; }
    }

    public class fact_configuraciones
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string valor { get; set; }
    }

    public class fact_cuentaclie
    {
        public int id { get; set; }
        public int idclie { get; set; }
        public int idcomp { get; set; }
        public int pago { get; set; }
    }

    public class fact_cuentaprov
    {
        public int id { get; set; }
        public int idprov { get; set; }
        public int idcomp { get; set; }
        public int pago { get; set; }
    }

    public class fact_localidad
    {
        public int id { get; set; }
        public string nombre{ get; set; }
    }
        public class fact_empresa
    {
        public int id { get; set; }
        public string nombrefantasia { get; set; }
        public string razonsocial { get; set; }
        public string direccion { get; set; }
        public string localidad { get; set; }
        public string otrosdatos { get; set; }
        public string cuit { get; set; }
        public string ingbrutos { get; set; }
        public string ivatipo { get; set; }
        public string inicioact { get; set; }
        public string drei { get; set; }
        public byte[] logo { get; set; }
    }

    public class fact_insumos
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public decimal  precio { get; set; }
        public decimal ganancia { get; set; }
        public string garantia { get; set; }
        public decimal desc_cantidad { get; set; }
        public decimal iva { get; set; }
        public string codprov { get; set; }
        public int categoria { get; set; }
        public int marca { get; set; }
        public int modelo { get; set; }
        public decimal bonif { get; set; }
        public string detalles { get; set; }
        public string cod_bar { get; set; }
        public int moneda { get; set; }
        public decimal utilidad1 { get; set; }
        public decimal utilidad2 { get; set; }
        public string foto { get; set; }
        public int tipo { get; set; }
        public string codigo { get; set; }
        public int calcular_precio { get; set; }
        public int eliminado { get; set; }
        public int unidades { get; set; }
        public string presentacion { get; set; }
        public TimeSpan fechaMod { get; set; }
        public decimal utilidad3 { get; set; }
        public decimal utilidad4 { get; set; }
        public decimal utilidad5 { get; set; }
        public decimal impuestoFijo01 { get; set; }
        public decimal impuestoFijo02 { get; set; }
        public decimal precioVenta { get; set; }
    }
    public class fact_facturas
    {
        public int id { get; set; }
        public int ptovta { get; set; }
        public int num_fact { get; set; }
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
        public int tipofact { get; set; }
        public string observaciones { get; set; }
        public string observaciones2 { get; set; }
        public string remito { get; set; }
        public string cae { get; set; }
        public string vtocae { get; set; }
        public string codbarra { get; set; }        
        public int pago { get; set; }
        public byte[] codigo_qr { get; set; }
    }

    public class fact_items
    {
        public int id { get; set; }
        public string cod { get; set; }
        public string plu { get; set; }
        public decimal cantidad { get; set; }
        public string descripcion { get; set; }
        public decimal iva { get; set; }
        public decimal punit { get; set; }
        public decimal ptotal { get; set; }
        public int tipofact { get; set; }
        public int idAlmacen { get; set; }
        public int idCaja { get; set; }
        public int id_fact { get; set; }
        public int codint { get; set; }
        public decimal impuestoFijo01 { get; set; }
        public decimal impuestoFijo02 { get; set; }
        public TimeSpan fecha_alta { get; set; }
    }
    public class fact_ivatipo
    {
        public int id { get; set; }
        public string tipo { get; set; }
    }

    public class fact_listas_precio
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public decimal utilidad { get; set; }
        public int auxcol { get; set; }
    }

}

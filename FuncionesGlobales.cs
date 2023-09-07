
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using System.Globalization;
using System.Diagnostics;

namespace SistemaPreventa
{
    public class FuncionesGlobales
    {
        public ImageSource GetImageSourceFromBytes(byte[] imageData)
        {
            using (MemoryStream stream = new MemoryStream(imageData))
            {
                return ImageSource.FromStream(() => stream);
            }
        }
        public static int ConvertirMesANumero(string nombreMes)
        {
            DateTimeFormatInfo formato = CultureInfo.CurrentCulture.DateTimeFormat;
            return formato.MonthNames.ToList().FindIndex(m => m.Equals(nombreMes, StringComparison.OrdinalIgnoreCase)) + 1;
        }

        public static decimal CalcularPrecioLista(decimal precio, decimal iva, 
            decimal utilidad0, decimal utilidad1, decimal utilidad2, decimal utilidad3, 
            decimal utilidad4, decimal utilidad5, int moneda, int idLista, int metodoCalculo)
        {
            decimal ganancia0, ganancia1, ganancia2, ganancia3, ganancia4, ganancia5, 
                alicuota,precioFinal,gananciaLista, cotizacion;
            ganancia0 = (utilidad0 + 100) / 100;
            ganancia1 = (utilidad1 + 100) / 100;
            ganancia2 = (utilidad2 + 100) / 100;
            ganancia3 = (utilidad3 + 100) / 100;
            ganancia4 = (utilidad4 + 100) / 100;
            ganancia5 = (utilidad5 + 100) / 100;
            alicuota = (iva + 100) / 100;

            int auxcol = 0;
            List< fact_listas_precio> listaPrecio = DatosComunes.listasPrecios.Where(lista => lista.id == UsuarioConectado.VendedorSeleccionado.listaPrecios).ToList();
            gananciaLista = listaPrecio[0].utilidad;
            gananciaLista = (gananciaLista + 100) / 100;
            auxcol = listaPrecio[0].auxcol;
            //Debug.WriteLine("Usuario - " + UsuarioConectado.VendedorSeleccionado.nombre + " lista: " + UsuarioConectado.VendedorSeleccionado.listaPrecios + " auxcol: " );
            cotizacion = 0;
            precioFinal = 0;
            if (moneda == 1)
            {
                cotizacion = 1;
            }
            if (metodoCalculo == 1)
            {
                switch (auxcol)
                {
                    case 0:
                        precioFinal = Math.Round(precio * alicuota * ganancia0 * gananciaLista * cotizacion, 2);
                        //Debug.WriteLine("0 - " + precio + " * " + alicuota + " * " + ganancia0 + " * " + gananciaLista + " * " + cotizacion);
                        break;
                    case 1:
                        precioFinal = Math.Round(precio * alicuota * ganancia1 * gananciaLista * cotizacion, 2);
                        //Debug.WriteLine("1 - " + precio + " * " + alicuota + " * " + ganancia1 + " * " + gananciaLista + " * " + cotizacion);
                        break;
                    case 2:
                        precioFinal = Math.Round(precio * alicuota * ganancia2 * gananciaLista * cotizacion, 2);
                        //Debug.WriteLine("2 - " + precio + " * " + alicuota + " * " + ganancia2 + " * " + gananciaLista + " * " + cotizacion);
                        break;
                    case 3:
                        precioFinal = Math.Round(precio * alicuota * ganancia3 * gananciaLista * cotizacion, 2);
                        //Debug.WriteLine("3 - " + precio + " * " + alicuota + " * " + ganancia3 + " * " + gananciaLista + " * " + cotizacion);
                        break;
                    case 4:
                        precioFinal = Math.Round(precio * alicuota * ganancia4 * gananciaLista * cotizacion, 2);
                        //Debug.WriteLine("4 - " + precio + " * " + alicuota + " * " + ganancia4 + " * " + gananciaLista + " * " + cotizacion);
                        break;
                    case 5:
                        precioFinal = Math.Round(precio * alicuota * ganancia5 * gananciaLista * cotizacion, 2);
                        //Debug.WriteLine("5 - " + precio + " * " + alicuota + " * " + ganancia5 + " * " + gananciaLista + " * " + cotizacion);
                        break;
                }
            }else if (metodoCalculo == 0)
            {
                switch (auxcol)
                {
                    case 0:
                        precioFinal = Math.Round(precio * alicuota * ((ganancia0 + gananciaLista)-1) * cotizacion, 2);
                        break;
                    case 1:
                        precioFinal = Math.Round(precio * alicuota * ((ganancia1 + gananciaLista) - 1) * cotizacion, 2);
                        break;
                    case 2:
                        precioFinal = Math.Round(precio * alicuota * ((ganancia2 + gananciaLista) - 1) * cotizacion, 2);
                        break;
                    case 3:
                        precioFinal = Math.Round(precio * alicuota * ((ganancia3 + gananciaLista) - 1) * cotizacion, 2);
                        break;
                    case 4:
                        precioFinal = Math.Round(precio * alicuota * ((ganancia4 + gananciaLista) - 1) * cotizacion, 2);
                        break;
                    case 5:
                        precioFinal = Math.Round(precio * alicuota * ((ganancia5 + gananciaLista) - 1) * cotizacion, 2);
                        break;
                }
            }
            return precioFinal;
        }

        public static decimal CompobarNulos(string numero)
        {
            if (string.IsNullOrEmpty(numero))
            {
                return 0;
            }
            else
            {
                return decimal.Parse(numero);
            }

        }
    }
}

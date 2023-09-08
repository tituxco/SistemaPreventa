
using MySqlConnector;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace SistemaPreventa
{


    public static class DatabaseConfig
    {

        private static string _connectionStringAutorizacion = "Server=66.97.35.86;Port=3306;Database=AuthServ;Uid=kiremoto;Pwd=mecago;SslMode=None;";
        private static string _connectionStringCliente;
        
        private static MySqlConnection _databaseCliente;
        private static MySqlConnection _databaseAsyncCliente;
        private static MySqlConnection _databaseAsyncAutorizacion;
        private static MySqlConnectionStringBuilder MySqlConnectionStringBuilderCliente = new MySqlConnectionStringBuilder(_connectionStringCliente);

        private static DatabaseConfigSqlite databaseConfigSqlite = new DatabaseConfigSqlite(FileAccessHelper.GetLocalFilePath("sistemaPreventa.db3"));

        public static bool CheckDatabaseConnectionAutorizacion()
        {
            try
            {
                _databaseAsyncAutorizacion = new MySqlConnection(_connectionStringAutorizacion);                               
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "->" + _connectionStringCliente);
                return false;
            }
        }
        public static bool CheckDatabaseConnectionCliente()
        {
            try
            {
                _databaseCliente = new MySqlConnection(_connectionStringCliente);
                _databaseAsyncCliente = new MySqlConnection(_connectionStringCliente);
                //Debug.WriteLine("Se conecto al servidor del cliente " + _connectionStringCliente);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "->" + _connectionStringCliente);
                return false;
            }
        }
        public static async Task< bool> ComprobarUsuarioValido(string username, string password)
        {
            try
            {
                await _databaseAsyncCliente.OpenAsync();
                if (_databaseAsyncCliente.State == System.Data.ConnectionState.Open)
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM cm_usuarios WHERE usuario like '" + username + "' and contrasena like '" + password + "'  and activo=1", _databaseAsyncCliente))
                    {
                        int i = 0;
                        using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                        {

                            if (await reader.ReadAsync())
                            {
                                i++;
                            }
                        }
                        if (i > 0)
                        {
                            return true;
                        }
                        else { return false; }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error al obtener datos: " + ex.Message);
                return false;
            }
            finally { await _databaseAsyncCliente.CloseAsync(); }
        }
        public static async Task<bool> ComprobarUsuarioAutorizado(string username, string password)
        {
            try
            {
                 await _databaseAsyncAutorizacion.OpenAsync();
                //CliAuth usuarioAutorizado = new CliAuth();
                if (_databaseAsyncAutorizacion.State == System.Data.ConnectionState.Open)
                {
                    Debug.WriteLine("Se conecto al servidor de autorizacion");
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM CliAuth WHERE codus like '" + username + "' and clave like '" + password + "'  and autorizado=1", _databaseAsyncAutorizacion))
                    {
                        using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                        {

                            if (await reader.ReadAsync())
                            {
                                AuthServ.UsuarioAutorizado.autorizado = reader.GetInt32("autorizado");
                                AuthServ.UsuarioAutorizado.servidor = reader.GetString("servidor");
                                AuthServ.UsuarioAutorizado.puerto = reader.GetString("puerto");
                                AuthServ.UsuarioAutorizado.bd = reader.GetString("bd");
                                AuthServ.UsuarioAutorizado.usuario = reader.GetString("usuario");
                                AuthServ.UsuarioAutorizado.pass = reader.GetString("pass");
                                AuthServ.UsuarioAutorizado.idInt = reader.GetInt32("idInt");
                                AuthServ.UsuarioAutorizado.modulo = reader.GetString("modulo");

                                _connectionStringCliente = "Server=" + AuthServ.UsuarioAutorizado.servidor + ";Port=" + AuthServ.UsuarioAutorizado.puerto +
                               ";Database=" + AuthServ.UsuarioAutorizado.bd + ";Uid=" + AuthServ.UsuarioAutorizado.usuario + ";Pwd=" + AuthServ.UsuarioAutorizado.pass + ";SslMode=None;";
                                Debug.WriteLine("Se obtuvo datos de usuario, se esta conectando al servidor");
                                CheckDatabaseConnectionCliente();
                                Debug.WriteLine("Conexion al cliente exitosa...");
                                return true;
                            }                                                        
                            else
                            {
                                Debug.WriteLine("no se pudo autenticar el usuario o no esta autorizado");
                                return false;
                            }
                        }  

                    }
                    
                }
                else
                {
                    Debug.WriteLine("no se pudo conectar al servidor de autorizacion");
                    return false;
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine("error al obtener datos: " + ex.Message);
                return false;
            }
            finally { await _databaseAsyncAutorizacion.CloseAsync(); }
        }
        public static string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    builder.Append(data[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
        public static async Task <cm_usuarios> ObtenerDatosUsuario(string username, string password)
        {
            cm_usuarios usuario = null;
            try
            {
                _databaseAsyncCliente.Open();
                if (_databaseAsyncCliente.State == ConnectionState.Open)
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM cm_usuarios WHERE usuario like '" + username + "' and activo=1", _databaseAsyncCliente))
                    {
                        using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                usuario = new cm_usuarios
                                {
                                    id = reader.GetInt32("id"),
                                    nombre = reader.GetString("nombre"),
                                    apellido = reader.GetString("apellido"),
                                    privilegios = reader.GetString("privilegios"),
                                    idAlmacen = reader.GetInt32("idAlmacen"),
                                    activo = reader.GetInt32("activo"),
                                    idvendedor = reader.GetString("idvendedor"),
                                    idtecnico = reader.GetInt32("idtecnico"),
                                };

                            }
                        }
                        return usuario;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error " + ex);
                return null;
            }
            finally { await _databaseAsyncCliente.CloseAsync(); }
        }
        public static List<fact_vendedor> ObtenerDatosVendedor(string idVendedor)
        {
            _databaseCliente.Open();
            try
            {
                List<fact_vendedor> vendedores = new List<fact_vendedor>();
                if (_databaseCliente.State == ConnectionState.Open)
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM fact_vendedor where id in(" + idVendedor +")", _databaseCliente))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                fact_vendedor vendedor = new fact_vendedor
                                {
                                    id = Convert.ToInt32(reader["id"]),
                                    nombre = Convert.ToString(reader["nombre"]),
                                    apellido = Convert.ToString(reader["apellido"]),
                                    activo = Convert.ToInt32(reader["activo"]),
                                    comision = Convert.ToDecimal(reader["comision"]),
                                    listaPrecios = reader.GetInt32("listaPrecios"),
                                };
                                vendedores.Add(vendedor);
                            }
                        }
                    }
                    return vendedores;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error al obtener datos: " + ex.Message);
                return null;
            }
            finally
            {
                _databaseCliente.CloseAsync();
            }
        }
        public static fact_empresa ObtenerDatosEmpresa()
        {
            fact_empresa empresa = null;
            _databaseCliente.Open();
            try
            {
                if (_databaseCliente.State == ConnectionState.Open)
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM fact_empresa", _databaseCliente))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                empresa = new fact_empresa
                                {
                                    id = Convert.ToInt32(reader["id"]),
                                    nombrefantasia = Convert.ToString(reader["nombrefantasia"]),
                                    razonsocial = Convert.ToString(reader["razonsocial"]),
                                    direccion = Convert.ToString(reader["direccion"]),
                                    localidad = Convert.ToString(reader["localidad"]),
                                    otrosdatos = Convert.ToString(reader["otrosdatos"]),
                                    cuit = Convert.ToString(reader["cuit"]),
                                    ingbrutos = Convert.ToString(reader["ingbrutos"]),
                                    ivatipo = Convert.ToString(reader["ivatipo"]),
                                    inicioact = Convert.ToString(reader["inicioact"]),
                                    drei = Convert.ToString(reader["drei"]),
                                    logo = (byte[])reader["logo"]
                                };
                            }
                        }
                    }
                    return empresa;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error al obtener datos: " + ex.Message);
                return null;
            }
            finally
            {
                _databaseCliente.Close();
            }
        }



        public static async Task<List<fact_clientes>> CargarClientes(string buscar)
        {
            try
            {
                await _databaseAsyncCliente.OpenAsync();
                List<fact_clientes> clientes = new List<fact_clientes>();
                if (_databaseAsyncCliente.State == System.Data.ConnectionState.Open)
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM fact_clientes where nomapell_razon like '%" + buscar.Replace(" ", "%") + "' limit 0,50", _databaseAsyncCliente))
                    {
                        using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                int id = reader.GetInt32("idclientes");
                                string nombre = reader.GetString("nomapell_razon");
                                string direccion = reader.GetString("dir_domicilio");
                                int localidad = reader.GetInt32("dir_localidad");
                                int ivaTipo = reader.GetInt32("iva_tipo");
                                string cuit = reader.GetString("cuit");
                                string telefono = reader.GetString("telefono");
                                string contacto = reader.GetString("contacto");
                                string celular = reader.GetString("celular");
                                string email = reader.GetString("email");
                                string observaciones = reader.GetString("observaciones");
                                int listaPrecios = reader.GetInt32("lista_precios");
                                string codClie = reader.GetString("codClie");
                                int vendedor = reader.GetInt32("vendedor");
                                List<fact_localidad> localidadClie = DatosComunes.localidades.Where(loca => loca.id == localidad).ToList();
                                List<fact_ivatipo> tipoContr = DatosComunes.tiposContribuyentes.Where(contr => contr.id == ivaTipo).ToList();
                                fact_clientes cliente = new fact_clientes
                                {
                                    idclientes = id,
                                    nomapell_razon = nombre,
                                    dir_domicilio = direccion,
                                    dir_localidad = localidad,
                                    iva_tipo = ivaTipo,
                                    cuit = cuit,
                                    telefono = telefono,
                                    contacto = contacto,
                                    celular = celular,
                                    email = email,
                                    observaciones = observaciones,
                                    lista_precios = listaPrecios,
                                    codClie = codClie,
                                    vendedor = vendedor,
                                    localidadTexto = localidadClie[0].nombre,
                                    tipoContr = tipoContr[0].tipo,
                                };
                                clientes.Add(cliente);
                            }
                            reader.Close();
                        }
                    }
                    return clientes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos: " + ex.Message);
                return null;
            }
            finally
            {
                await _databaseAsyncCliente.CloseAsync();
            }

        }
        public static async Task<List<fact_clientes>> CargarClientesVendedor(int idVendedor, string buscar)
        {
            try
            {
                await _databaseAsyncCliente.OpenAsync();
                List<fact_clientes> clientes = new List<fact_clientes>();
                if (_databaseAsyncCliente.State == System.Data.ConnectionState.Open)
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM fact_clientes where vendedor=" + idVendedor + " and nomapell_razon like '%" + buscar.Replace(" ", "%") + "%' limit 0,50", _databaseAsyncCliente))
                    {
                        using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                int id = reader.GetInt32("idclientes");
                                string nombre = reader.GetString("nomapell_razon");
                                string direccion = reader.GetString("dir_domicilio");
                                int localidad = reader.GetInt32("dir_localidad");
                                int ivaTipo = reader.GetInt32("iva_tipo");
                                string cuit = reader.GetString("cuit");
                                string telefono = reader.GetString("telefono");
                                string contacto = reader.GetString("contacto");
                                string celular = reader.GetString("celular");
                                string email = reader.GetString("email");
                                string observaciones = reader.GetString("observaciones");
                                int listaPrecios = reader.GetInt32("lista_precios");
                                string codClie = reader.GetString("codClie");
                                int vendedor = reader.GetInt32("vendedor");
                                List<fact_localidad> localidadClie = DatosComunes.localidades.Where(loca => loca.id == localidad).ToList();
                                List<fact_ivatipo> tipoContr = DatosComunes.tiposContribuyentes.Where(contr => contr.id == ivaTipo).ToList();
                                fact_clientes cliente = new fact_clientes
                                {
                                    idclientes = id,
                                    nomapell_razon = nombre,
                                    dir_domicilio = direccion,
                                    dir_localidad = localidad,
                                    iva_tipo = ivaTipo,
                                    cuit = cuit,
                                    telefono = telefono,
                                    contacto = contacto,
                                    celular = celular,
                                    email = email,
                                    observaciones = observaciones,
                                    lista_precios = listaPrecios,
                                    codClie = codClie,
                                    vendedor = vendedor,
                                    localidadTexto = localidadClie[0].nombre,
                                    tipoContr = tipoContr[0].tipo,
                                    
                                };
                                clientes.Add(cliente);
                            }
                            reader.Close();
                        }
                    }
                    return clientes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos: " + ex.Message);
                return null;
            }
            finally
            {
                await _databaseAsyncCliente.CloseAsync();
            }

        }
        public static async Task<fact_clientes> CargarClienteId(int idCliente)
        {
            try
            {
                await _databaseAsyncCliente.OpenAsync();
                fact_clientes cliente = null;
                if (_databaseAsyncCliente.State == System.Data.ConnectionState.Open)
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM fact_clientes where idclientes="+idCliente, _databaseAsyncCliente))
                    {
                        using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                int id = reader.GetInt32("idclientes");
                                string nombre = reader.GetString("nomapell_razon");
                                string direccion = reader.GetString("dir_domicilio");
                                int localidad = reader.GetInt32("dir_localidad");
                                int ivaTipo = reader.GetInt32("iva_tipo");
                                string cuit = reader.GetString("cuit");
                                string telefono = reader.GetString("telefono");
                                string contacto = reader.GetString("contacto");
                                string celular = reader.GetString("celular");
                                string email = reader.GetString("email");
                                string observaciones = reader.GetString("observaciones");
                                int listaPrecios = reader.GetInt32("lista_precios");
                                string codClie = reader.GetString("codClie");
                                int vendedor = reader.GetInt32("vendedor");
                                
                                List<fact_localidad> localidadClie = DatosComunes.localidades.Where(loca => loca.id == localidad).ToList();
                                List<fact_ivatipo>tipoContr=DatosComunes.tiposContribuyentes.Where(contr=>contr.id==ivaTipo).ToList();
                                cliente = new fact_clientes()
                                {
                                    idclientes = id,
                                    nomapell_razon = nombre,
                                    dir_domicilio = direccion,
                                    dir_localidad = localidad,
                                    iva_tipo = ivaTipo,
                                    cuit = cuit,
                                    telefono = telefono,
                                    contacto = contacto,
                                    celular = celular,
                                    email = email,
                                    observaciones = observaciones,
                                    lista_precios = listaPrecios,
                                    codClie = codClie,
                                    vendedor = vendedor,
                                    localidadTexto = localidadClie[0].nombre,
                                    tipoContr = tipoContr[0].tipo
                                    
                                };                                
                            }
                            reader.Close();
                        }
                        return cliente;
                    }                    
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos: " + ex.Message);
                return null;
            }
            finally
            {
                await _databaseAsyncCliente.CloseAsync();
            }

        }
        public static async Task<bool>GuardarCliente(fact_clientes cliente)
        {
            try
            {
                await _databaseAsyncCliente.OpenAsync();                    
                if (_databaseAsyncCliente.State == System.Data.ConnectionState.Open)
                {
                    using (MySqlCommand command = new MySqlCommand("INSERT INTO fact_clientes (idclientes, nomapell_razon, dir_domicilio, dir_localidad, iva_tipo, cuit, telefono, contacto, celular, email, observaciones,lista_precios,vendedor) " +
                       "VALUES (@idclientes, @nomapell_razon, @dir_domicilio, @dir_localidad, @iva_tipo, @cuit, @telefono, @contacto, @celular, @email, @observaciones,@lista_precios,@vendedor) " +
                       "ON DUPLICATE KEY UPDATE " +
                       "nomapell_razon = VALUES(nomapell_razon), dir_domicilio = VALUES(dir_domicilio), dir_localidad = VALUES(dir_localidad), " +
                       "iva_tipo = VALUES(iva_tipo), cuit = VALUES(cuit), telefono = VALUES(telefono), contacto = VALUES(contacto), " +
                       "celular = VALUES(celular), email = VALUES(email), observaciones = VALUES(observaciones), lista_precios=VALUES(lista_precios), vendedor=VALUES(vendedor)", _databaseAsyncCliente))
                    {                        
                        command.Parameters.AddWithValue("@idclientes", cliente.idclientes);
                        command.Parameters.AddWithValue("@nomapell_razon", cliente.nomapell_razon);
                        command.Parameters.AddWithValue("@dir_domicilio", cliente.dir_domicilio);
                        command.Parameters.AddWithValue("@dir_localidad", cliente.dir_localidad);
                        command.Parameters.AddWithValue("@iva_tipo", cliente.iva_tipo);
                        command.Parameters.AddWithValue("@cuit", cliente.cuit);
                        command.Parameters.AddWithValue("@telefono", cliente.telefono);
                        command.Parameters.AddWithValue("@contacto", cliente.contacto);
                        command.Parameters.AddWithValue("@celular", cliente.celular);
                        command.Parameters.AddWithValue("@email", cliente.email);
                        command.Parameters.AddWithValue("@observaciones", cliente.observaciones);
                        command.Parameters.AddWithValue("@lista_precios", cliente.lista_precios);
                        command.Parameters.AddWithValue("@vendedor", cliente.vendedor);

                        await command.ExecuteNonQueryAsync();                        
                    }
                    return true;
                }
                else 
                { 
                    return false; 
                }             

            }catch (Exception ex) 
            {
                Debug.WriteLine("Error " + ex.Message);
                return false;
            }
            finally
            {
                await _databaseAsyncCliente.CloseAsync();
            }
        }
        public static async Task<List<fact_insumos>> CargarProductos(string buscar, string idcategoria)
        {
            try
            {
                await _databaseAsyncCliente.OpenAsync();
                List<fact_insumos> productos = new List<fact_insumos>();
                if (_databaseAsyncCliente.State == System.Data.ConnectionState.Open)
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM fact_insumos where eliminado=0 and (descripcion like '%" + buscar + "%' or codigo like '%" + buscar + "%') and categoria like '" + idcategoria + "' limit 0,50", _databaseAsyncCliente))
                    {
                        //Debug.WriteLine(command.CommandText);
                        using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                fact_insumos producto = new fact_insumos();

                                producto.id = reader.GetInt32("id");
                                producto.descripcion = reader.GetString("descripcion");                               
                                producto.garantia = reader.GetString("garantia");
                                producto.desc_cantidad = decimal.Parse(reader.GetString("desc_cantidad"));
                                producto.iva = decimal.Parse(reader.GetString("iva"));
                                producto.codprov = reader.GetString("codprov");
                                producto.categoria = reader.GetInt32("categoria");
                                producto.marca = reader.GetInt32("marca");
                                producto.modelo = reader.GetInt32("modelo");
                                
                                producto.detalles = reader.GetString("detalles");
                                producto.cod_bar = reader.GetString("cod_bar");
                                producto.moneda = reader.GetInt32("moneda");

                                producto.precio = FuncionesGlobales.CompobarNulos(reader.GetString("precio"));
                                producto.ganancia = FuncionesGlobales.CompobarNulos(reader.GetString("ganancia"));
                                producto.bonif = FuncionesGlobales.CompobarNulos(reader.GetString("bonif"));
                                producto.utilidad1 =  FuncionesGlobales.CompobarNulos(reader.GetString("utilidad1"));
                                producto.utilidad2 = FuncionesGlobales.CompobarNulos(reader.GetString("utilidad2"));
                                producto.utilidad3 = FuncionesGlobales.CompobarNulos(reader.GetString("utilidad3"));
                                producto.utilidad4 = FuncionesGlobales.CompobarNulos(reader.GetString("utilidad4"));
                                producto.utilidad5 = FuncionesGlobales.CompobarNulos(reader.GetString("utilidad5"));

                                //producto.foto = reader.GetString(reader.GetString("foto"));
                                producto.tipo = reader.GetInt32("tipo");
                                producto.codigo = reader.GetString("codigo");
                                producto.calcular_precio = reader.GetInt32("calcular_precio");
                                producto.eliminado = reader.GetInt32("eliminado");
                                producto.unidades = reader.GetInt32("unidades");
                                producto.presentacion = reader.GetString("presentacion");
                                
                                producto.precioVenta = FuncionesGlobales.CalcularPrecioLista(
                                  FuncionesGlobales.CompobarNulos(reader.GetString("precio")),
                                  FuncionesGlobales.CompobarNulos(reader.GetString("iva")), 
                                  FuncionesGlobales.CompobarNulos(reader.GetString("ganancia")),
                                  FuncionesGlobales.CompobarNulos(reader.GetString("utilidad1")), 
                                  FuncionesGlobales.CompobarNulos(reader.GetString("utilidad2")),
                                  FuncionesGlobales.CompobarNulos(reader.GetString("utilidad3")), 
                                  FuncionesGlobales.CompobarNulos(reader.GetString("utilidad4")),
                                  FuncionesGlobales.CompobarNulos(reader.GetString("utilidad5")), 1,
                                  UsuarioConectado.VendedorSeleccionado.listaPrecios, 1);
                                productos.Add(producto);
                            }
                            reader.Close();
                        }
                    }
                    return productos;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al obtener los datos: " + ex.Message);
                return null;
            }
            finally
            {
                await _databaseAsyncCliente.CloseAsync();
            }

        }
        
        
        public static async Task<List<fact_localidad>> ObtenerLocalidades()
        {
            try
            {
                await _databaseAsyncCliente.OpenAsync();
                List<fact_localidad> localidades = new List<fact_localidad>();
                if (_databaseAsyncCliente.State == System.Data.ConnectionState.Open)
                {
                    using MySqlCommand command = new MySqlCommand("SELECT * FROM cm_localidad ", _databaseAsyncCliente);
                    using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            fact_localidad localidad = new fact_localidad
                            {
                                id = reader.GetInt32("id"),
                                nombre = reader.GetString("nombre")
                            };
                            localidades.Add(localidad);
                        };
                        reader.Close();
                    }
                    //return localidades;
                    return localidades;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos: " + ex.Message);
                return null;
            }
            finally
            {
                await _databaseAsyncCliente.CloseAsync();
            }
        }
        public static async Task<List<fact_conffiscal>> ObtenerConfigFiscal()
        {
            try
            {
                await _databaseAsyncCliente.OpenAsync();
                List<fact_conffiscal> ConfFiscales = new List<fact_conffiscal>();

                if (_databaseAsyncCliente.State == System.Data.ConnectionState.Open)
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM fact_conffiscal ", _databaseAsyncCliente))
                    {
                        using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                fact_conffiscal conffiscal = new fact_conffiscal
                                {
                                    id = reader.GetInt32("id"),
                                    confnume = reader.GetInt32("confnume"),
                                    donfdesc = reader.GetString("donfdesc"),
                                    abrev = reader.GetString("abrev"),
                                    tip = reader.GetString("tip"),
                                    leg = reader.GetInt32("leg"),
                                    ptovta = reader.GetInt32("ptovta"),
                                    debcred = reader.GetString("debcred"),
                                    codfiscal = reader.GetString("codfiscal"),
                                    letra = reader.GetString("letra"),
                                    observaciones = reader.GetString("observaciones"),
                                };
                                ConfFiscales.Add(conffiscal);
                            };
                            reader.Close();
                        }
                    }
                    return ConfFiscales;
                }
                else
                {
                    return null;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos: " + ex.Message);
                return null;
            }
            finally
            {
                await _databaseAsyncCliente.CloseAsync();
            }
        }        
        public static async Task<List<fact_configuraciones>> ObtenerConfiguraciones()
        {
            try
            {
                await _databaseAsyncCliente.OpenAsync();
                List<fact_configuraciones> ConfGenerales = new List<fact_configuraciones>();

                if (_databaseAsyncCliente.State == System.Data.ConnectionState.Open)
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM fact_configuraciones ", _databaseAsyncCliente))
                    {
                        using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                fact_configuraciones configuracion = new fact_configuraciones
                                {
                                    id = reader.GetInt32("id"),
                                    nombre = reader.GetString("nombre"),
                                    valor = reader.GetString("valor"),
                                };
                                ConfGenerales.Add(configuracion);
                            };
                            reader.Close();
                        }
                    }
                    return ConfGenerales;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos: " + ex.Message);
                return null;
            }
            finally
            {
                await _databaseAsyncCliente.CloseAsync();
            }
        }
        public static async Task<List<fact_ivatipo>> ObtenerTiposContibuyente()
        {
            try
            {
                await _databaseAsyncCliente.OpenAsync();
                List<fact_ivatipo> TiposContribuyente = new List<fact_ivatipo>();

                if (_databaseAsyncCliente.State == System.Data.ConnectionState.Open)
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM fact_ivatipo ", _databaseAsyncCliente))
                    {
                        using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                fact_ivatipo configuracion = new fact_ivatipo
                                {
                                    id = reader.GetInt32("id"),
                                    tipo = reader.GetString("tipo"),
                                };
                                TiposContribuyente.Add(configuracion);
                            };
                            reader.Close();
                        }
                    }
                    return TiposContribuyente;
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos: " + ex.Message);
                return null;
            }
            finally
            {
                await _databaseAsyncCliente.CloseAsync();
            }
        }
        public static async Task<List<fact_listas_precio>> ObtenerListasPrecio()
        {
            try
            {
                await _databaseAsyncCliente.OpenAsync();
                List<fact_listas_precio> ListasPrecios = new List<fact_listas_precio>();

                if (_databaseAsyncCliente.State == System.Data.ConnectionState.Open)
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM fact_listas_precio ", _databaseAsyncCliente))
                    {
                        using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                fact_listas_precio listaPrecio = new fact_listas_precio
                                {
                                    id = reader.GetInt32("id"),
                                    nombre = reader.GetString("nombre"),
                                    auxcol = reader.GetInt32("auxcol"),
                                    utilidad = FuncionesGlobales.CompobarNulos(reader.GetString("utilidad")),
                                };
                                ListasPrecios.Add(listaPrecio);
                            };
                            reader.Close();
                        }
                    }
                    return ListasPrecios;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos: " + ex.Message);
                return null;
            }
            finally
            {
                await _databaseAsyncCliente.CloseAsync();
            }
        }
        public static async Task<List<fact_categoria_insum>> ObtenerCategoriasInsumos()
        {
            try
            {
                await _databaseAsyncCliente.OpenAsync();
                List<fact_categoria_insum> CategoriasInsumos = new List<fact_categoria_insum>();

                if (_databaseAsyncCliente.State == System.Data.ConnectionState.Open)
                {
                    using (MySqlCommand command = new MySqlCommand("SELECT * FROM fact_categoria_insum order by nombre asc", _databaseAsyncCliente))
                    {
                        using (MySqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                fact_categoria_insum categoriaInsumo = new fact_categoria_insum
                                {
                                    id = reader.GetInt32("id"),
                                    nombre = reader.GetString("nombre"),
                                    sincro = reader.GetInt32("sincro"),
                                };
                                CategoriasInsumos.Add(categoriaInsumo);
                            };
                            reader.Close();
                        }
                    }
                    return CategoriasInsumos;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos: " + ex.Message);
                return null;
            }
            finally
            {
                await _databaseAsyncCliente.CloseAsync();
            }
        }
        
        
        public static async Task<int> ObtenerNumeroComprobante(int ptovta, int tipoComprobante)
        {
            try
            {
                await _databaseAsyncCliente.OpenAsync();
                if (_databaseAsyncCliente.State == System.Data.ConnectionState.Open)
                {
                    int numeroSiguiente = 0;
                    using (MySqlCommand command = new MySqlCommand("update fact_conffiscal set confnume=confnume+1 where ptovta=?ptovta and donfdesc=?donfdesc;" +
                        "SELECT confnume FROM fact_conffiscal where ptovta=?ptovta and donfdesc=?donfdesc", _databaseAsyncCliente))
                    {
                        command.Parameters.AddWithValue("?ptovta", ptovta);
                        command.Parameters.AddWithValue("?donfdesc", tipoComprobante);

                        numeroSiguiente = Convert.ToInt32(await command.ExecuteScalarAsync());
                        Debug.WriteLine("numero siguiente comprobante: " +numeroSiguiente);
                    }
                    return numeroSiguiente;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener los datos: " + ex.Message);
                return 0;
            }
            finally
            {
                await _databaseAsyncCliente.CloseAsync();
            }

        }        
        public static async Task<bool> SincronizarPedidoIndividual(fact_Pedido pedido, List<fact_itemsPedido> itemsPedido, 
            int ptovta, int tipoComprobante,int IdAlmacen, int idCaja)
        {
            try
            {

                int numFact = await DatabaseConfig.ObtenerNumeroComprobante(ptovta, tipoComprobante);

                fact_facturas factFactura = new fact_facturas();
                List<fact_itemsPedido> factItems = new List<fact_itemsPedido>();

                //sincronizamos los datos del pedido con la factura
                //debemos obtener el proximo numero de pedido
                factFactura.ptovta = ptovta;
                factFactura.tipofact = tipoComprobante;
                factFactura.pago = 0;
                factFactura.condvta = 1;
                factFactura.cuit = pedido.cuit;
                factFactura.direccion = pedido.direccion;
                factFactura.fecha = pedido.fecha;
                factFactura.id_cliente = pedido.id_cliente;
                factFactura.iva105 = pedido.iva105;
                factFactura.iva21 = pedido.iva21;
                factFactura.localidad = pedido.localidad;
                factFactura.observaciones = "PENDIENTE";
                if (pedido.observaciones2 != null)
                {
                    factFactura.observaciones2 = pedido.observaciones2;
                }
                else
                {
                    factFactura.observaciones2 = "";
                }
                factFactura.otroiva = pedido.otroiva;
                factFactura.razon = pedido.razon;
                factFactura.subtotal = pedido.subtotal;
                factFactura.tipocontr = pedido.tipocontr;
                factFactura.total = pedido.total;
                factFactura.vendedor = pedido.vendedor;
                factFactura.num_fact = numFact;

                await _databaseAsyncCliente.OpenAsync();
                int id_fact;
                if (_databaseAsyncCliente.State == System.Data.ConnectionState.Open)
                {
                    using (MySqlCommand command = new MySqlCommand("insert into fact_facturas (ptovta,num_fact,fecha,id_cliente,razon,direccion,localidad," +
                        "tipocontr,cuit,condvta,subtotal,iva105,iva21,otroiva,total,vendedor,tipofact,observaciones,observaciones2) values (?ptovta," +
                        "?num_fact,?fecha,?id_cliente,?razon,?direccion,?localidad,?tipocontr,?cuit,?condvta,?subtotal,?iva105,?iva21,?otroiva,?total," +
                        "?vendedor,?tipofact,?observaciones,?observaciones2); SELECT LAST_INSERT_ID()", _databaseAsyncCliente))
                    {
                        command.Parameters.AddWithValue("?ptovta", factFactura.ptovta);
                        command.Parameters.AddWithValue("?num_fact", factFactura.num_fact);
                        command.Parameters.AddWithValue("?fecha", factFactura.fecha);
                        command.Parameters.AddWithValue("?id_cliente", factFactura.id_cliente);
                        command.Parameters.AddWithValue("?razon", factFactura.razon);
                        command.Parameters.AddWithValue("?direccion", factFactura.direccion);
                        command.Parameters.AddWithValue("?localidad", factFactura.localidad);
                        command.Parameters.AddWithValue("?tipocontr", factFactura.tipocontr);
                        command.Parameters.AddWithValue("?cuit", factFactura.cuit);
                        command.Parameters.AddWithValue("?condvta", factFactura.condvta);
                        command.Parameters.AddWithValue("?subtotal", factFactura.subtotal);
                        command.Parameters.AddWithValue("?iva105", factFactura.iva105);
                        command.Parameters.AddWithValue("?iva21", factFactura.iva21);
                        command.Parameters.AddWithValue("?otroiva", factFactura.otroiva);
                        command.Parameters.AddWithValue("?total", factFactura.total);
                        command.Parameters.AddWithValue("?vendedor", factFactura.vendedor);
                        command.Parameters.AddWithValue("?tipofact", factFactura.tipofact);
                        command.Parameters.AddWithValue("?observaciones", factFactura.observaciones);
                        command.Parameters.AddWithValue("?observaciones2", factFactura.observaciones2);
                        //await command.ExecuteNonQueryAsync();
                        //command.CommandText = "SELECT LAST_INSERT_ID()";
                        id_fact = Convert.ToInt32(await command.ExecuteScalarAsync());
                        Debug.WriteLine("ultimo id: " + id_fact);
                    }
                    foreach (fact_itemsPedido item in itemsPedido)
                    {
                        using (MySqlCommand command = new MySqlCommand("insert into fact_items (cod,plu,cantidad,descripcion,iva,punit,ptotal,tipofact," +
                            "idAlmacen,idCaja,id_fact,codint) values (?cod,?plu,?cantidad,?descripcion,?iva,?punit,?ptotal,?tipofact,?idAlmacen," +
                            "?idCaja,?idFact,?codint)", _databaseAsyncCliente))
                        {
                            command.Parameters.AddWithValue("?cod", item.codint);
                            command.Parameters.AddWithValue("?plu", item.cod);
                            command.Parameters.AddWithValue("?cantidad", item.cantidad);
                            command.Parameters.AddWithValue("?descripcion", item.descripcion);
                            command.Parameters.AddWithValue("?iva", item.iva);
                            command.Parameters.AddWithValue("?punit", item.punit);
                            command.Parameters.AddWithValue("?ptotal", item.ptotal);
                            command.Parameters.AddWithValue("?tipofact", tipoComprobante);
                            command.Parameters.AddWithValue("?idAlmacen", IdAlmacen);
                            command.Parameters.AddWithValue("?idCaja", idCaja);
                            command.Parameters.AddWithValue("?idFact", id_fact);
                            command.Parameters.AddWithValue("?codint", item.cod);

                            await command.ExecuteNonQueryAsync();
                        }                        
                    }
                    databaseConfigSqlite.ActualizarDatosPedidoLocalSincro(pedido, numFact);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("error al sincronizar datos: " + ex.Message);
                return false;
            }
            finally
            {
                await _databaseAsyncCliente.CloseAsync();
            }
        }

    }
}
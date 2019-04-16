using EntidadesCompartidas;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServidorConexion.Negocio;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ServidorConexion
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "DAMnificus Server";
            ConsolaDebug.cargarConsola();
            HttpListener httpListener = null;
            try
            {
                ConsolaDebug.escribirEnConsola("INFO", "Servidor iniciado");
                httpListener = new HttpListener
                {
                    Prefixes = { "http://localhost:8080/" },
                };

                httpListener.Start();
                // Esperando sincrónicamente hasta que se reciba la solicitud del cliente para HTTP a través del puerto 8080
                while (true)
                {
                    ConsolaDebug.escribirEnConsola("INFO", "Esperando petición de cliente...");
                    var context = httpListener.GetContext();
                    var request = context.Request;
                    HttpListenerResponse response = context.Response;
                    ConsolaDebug.escribirEnConsola("INFO", "¡Conectado con un cliente!");
                    System.IO.Stream body = request.InputStream;
                    System.Text.Encoding encoding = request.ContentEncoding;
                    System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
                    if (request.ContentType != null)
                    {
                        ConsolaDebug.escribirEnConsola("INFO", "Datos recibidos, leyendo...", request.ContentType);
                        ConsolaDebug.escribirEnConsola("DEBUG", "Tipo de datos recibidos: {0}", request.ContentType);
                    }
                    else
                    {
                        ConsolaDebug.escribirEnConsola("WARNING", "Recibida peticion vacía");
                    }
                    //escribirEnConsola("DEBUG", "Start of client JSON data:");                   
                    string datosJson = reader.ReadToEnd(); // Convierte los datos en una cadena.
                    //escribirEnConsola("DEBUG",datosJson);
                    //escribirEnConsola("DEBUG", "End of client data:");
                    ConsolaDebug.escribirEnConsola("DEBUG", "Parseando la petición Json...");
                    var objetoJSON = JObject.Parse(datosJson);
                    ConsolaDebug.escribirEnConsola("DEBUG", "Peticion: {0}", (string)objetoJSON["peticion"]);
                    //escribirEnConsola("DEBUG", "Usuario: {0}", (string)objetoJSON["usuario"]);
                    //escribirEnConsola("DEBUG", "Contraseña: {0}", (string)objetoJSON["clave"]);
                    //escribirEnConsola("DEBUG", "Token: {0}", (string)objetoJSON["token"]);                   

                    // Serializa el objeto JSON en un objeto .NET
                    Peticion peticionActual = objetoJSON.ToObject<Peticion>();
                    // Procesa cada petición de forma asincrona en un threadpool                                                                        
                    Task.Run(() => procesarPeticion(peticionActual, response)); 
                    
                }
            }
            catch (SocketException e)
            {
                ConsolaDebug.escribirEnConsola("ERROR", "El servidor ha petado debido a:");
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                ConsolaDebug.escribirEnConsola("ERROR", "El servidor ha petado debido a:");
                Console.WriteLine("Exception: {0}", e);
            }
            finally
            {
                ConsolaDebug.escribirEnConsola("INFO", "Pulsa tecla intro para cerrar la ventana");
                Console.Read();
                // Stop listening for new clients.
                httpListener.Close();
            }
        }
        public static void procesarPeticion(Peticion peticionActual, HttpListenerResponse response)
        {
            string nombreHilo = "";
            try
            {                
                if (peticionActual.clave != null)
                {
                    peticionActual.clave = CifradoJson.Descifrado(peticionActual.clave, peticionActual.peticion);
                }
                if (peticionActual.token != null)
                {
                    peticionActual.token = CifradoJson.Descifrado(peticionActual.token, peticionActual.peticion);
                }
                if (peticionActual.usuario != null)
                {
                    peticionActual.usuario = CifradoJson.Descifrado(peticionActual.usuario, peticionActual.peticion);
                }
                nombreHilo = peticionActual.peticion + "-" + peticionActual.usuario;
                Thread.CurrentThread.Name = nombreHilo;
                ConsolaDebug.escribirEnConsola("DEBUG", "Hilo creado con nombre: {0}", nombreHilo);

                //ConsolaDebug.escribirEnConsola("DEBUG", "Usuario descifrado: {0}", peticionActual.usuario);
                //escribirEnConsola("DEBUG", "Contraseña descifrada: " + peticionActual.clave);
                //escribirEnConsola("DEBUG", "Token descifrado: " + peticionActual.token);

                ConexionUsuarios conexUsuarios = new ConexionUsuarios();
                ConexionEnlaces conexEnlaces = new ConexionEnlaces();

                /*****************************************
                 * Tratamiento de peticiones del cliente *
                 *****************************************/
                string claveEncriptada;

                // Petición de STATUS por parte de cliente
                if (peticionActual.peticion.Equals("status"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida petición de status por {0}", peticionActual.usuario);
                    enviarRespuesta("estoyVivo", null, null, null, response);
                }

                // Petición de SALT para login 
                else if (peticionActual.peticion.Equals("requestSalt"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida peticion de SALT por el usuario {0}", peticionActual.usuario);
                    //Consulta clave en la BD
                    claveEncriptada = conexUsuarios.obtenerPassHash(peticionActual);
                    if (!claveEncriptada.Equals("null"))
                    {
                        ConsolaDebug.escribirEnConsola("INFO", "Respondiendo con SALT y esperando peticion login...");
                        enviarRespuesta("usuarioEncontrado", null, Clave.getSal(claveEncriptada), null, response);
                    }
                    else
                    {
                        ConsolaDebug.escribirEnConsola("INFO", "No se ha encontrado el usuario en la BD");
                        enviarRespuesta("noExisteUsuario", null, null, null, response);
                    }

                }

                // Petición de login
                else if (peticionActual.peticion.Equals("login"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida peticion de login por el usuario {0}", peticionActual.usuario);
                    //Consulta clave
                    claveEncriptada = conexUsuarios.obtenerPassHash(peticionActual);
                    //Comprueba que la clave el la misma
                    bool claveValida = Clave.validarClave(peticionActual.clave, claveEncriptada);
                    if (claveValida)
                    {
                        ConsolaDebug.escribirEnConsola("INFO", "La contraseña es valida, generando token...");
                        string token = GeneradorTokens.GenerarToken(64);
                        if (conexUsuarios.actualizarTokenEnBBDD(token, peticionActual.usuario))
                        {
                            ConsolaDebug.escribirEnConsola("INFO", "Token generado y guardado en BD existósamente");
                        }
                        else
                        {
                            ConsolaDebug.escribirEnConsola("WARNING", "¡ATENCIóN! Error al guardar token en BD");
                        }
                        ConsolaDebug.escribirEnConsola("INFO", "Enviando token al cliente");
                        enviarRespuesta("passValida", token, null, null, response);
                    }
                    else
                    {
                        ConsolaDebug.escribirEnConsola("INFO", "Contraseña no valida");
                        enviarRespuesta("passNoValida", null, null, null, response);
                    }

                }

                // Petición de borrado de token
                else if (peticionActual.peticion.Equals("borrarToken"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida peticion de borrado de token");
                    if (conexUsuarios.actualizarTokenEnBBDD("", peticionActual.usuario))
                    {
                        enviarRespuesta("tokenBorrado", null, null, null, response);
                        ConsolaDebug.escribirEnConsola("INFO", "Token del usuario {0} borrado con éxito de la BD", peticionActual.usuario);
                    }
                    else
                    {
                        enviarRespuesta("error", null, null, null, response);
                        ConsolaDebug.escribirEnConsola("WARNING", "¡ATENCIóN! Problema al borrar el token del usuario {0}", peticionActual.usuario);
                    }
                }

                // Petición para obtener los enlaces
                else if (peticionActual.peticion.Equals("obtenerColeccionEnlaces"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida peticion de enlaces del usuario {0}", peticionActual.usuario);
                    List<Enlaces> coleccion = conexEnlaces.obtenerColeccionEnlaces();
                    enviarRespuesta("coleccionEnviada", null, null, coleccion, response);
                    ConsolaDebug.escribirEnConsola("INFO", "Colección enviada al cliente satisfactoriamente");
                }

                // Petición de envio de token a email
                else if (peticionActual.peticion.Equals("emailRegistro"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida petición de token de registro por {0}", peticionActual.usuario);
                    string token = GeneradorTokens.GenerarToken(64);
                    ConsolaDebug.escribirEnConsola("DEBUG", "Token generado: {0}", token);
                    string email = peticionActual.datos["email"];
                    ConsolaDebug.escribirEnConsola("INFO", "Enviando token a: {0}", email);
                    if (EnviarEmail.registro(email, token))
                    {
                        enviarRespuesta("emailConTokenEnviado", token, null, null, response);
                        ConsolaDebug.escribirEnConsola("INFO", "¡Email enviado! Esperando confirmación.");
                    }
                }

                // Petición de confirmación de registro
                else if (peticionActual.peticion.Equals("confirmarRegistro"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida petición de confirmación de registro por {0}", peticionActual.usuario);
                    string usuario = peticionActual.usuario;
                    string email = peticionActual.datos["email"];
                    string pass = peticionActual.clave;
                    string nombre = peticionActual.datos["nombre"];
                    string apellidos = peticionActual.datos["apellidos"];
                    bool registrado = conexUsuarios.introducirUsuarioEnBBDD(usuario, email, pass, nombre, apellidos);
                    if (registrado)
                    {
                        ConsolaDebug.escribirEnConsola("INFO", "Transacción en la BD de usuarios satisfactoria");
                        registrado = conexEnlaces.introducirUsuarioEnBBDD(usuario);
                        if (registrado)
                        {
                            ConsolaDebug.escribirEnConsola("INFO", "Inserción en la BD de enlaces satisfactoria, enviando respuesta...");
                            enviarRespuesta("usuarioRegistrado", null, null, null, response);
                        }
                        else
                        {
                            ConsolaDebug.escribirEnConsola("ERROR", "Error en la inserción en la BD de enlaces, empezando rollback...");
                            bool borrado = conexUsuarios.borrarUsuarioDeBBDD(usuario);
                            if (borrado)
                            {
                                ConsolaDebug.escribirEnConsola("INFO", "Usuario {0} borrado de la BD de usuarios", peticionActual.usuario);
                            }
                            else
                            {
                                ConsolaDebug.escribirEnConsola("ERROR", "Error al borrar el usuario {0} de la BD de usuarios", peticionActual.usuario);
                            }
                            ConsolaDebug.escribirEnConsola("INFO", "Enviando respuesta de error...");
                            enviarRespuesta("errorInsert", null, null, null, response);
                        }
                    }
                    else
                    {
                        ConsolaDebug.escribirEnConsola("ERROR", "Error en la inserción en la BD de usuarios, enviando respuesta...");
                        enviarRespuesta("errorInsert", null, null, null, response);
                    }
                }

                // Peticiones de búsqueda en la BD
                else if (peticionActual.peticion.Equals("buscaEmailenBD") || peticionActual.peticion.Equals("buscaUsuarioenBD"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida petición {0}", peticionActual.peticion);
                    bool duplicado = conexUsuarios.comprobarSiExisteEnBD(peticionActual);
                    if (duplicado)
                    {
                        ConsolaDebug.escribirEnConsola("INFO", "El elemento ya existe en la BD, enviando respuesta: duplicado");
                        enviarRespuesta("duplicado", null, null, null, response);
                    }
                    else
                    {
                        ConsolaDebug.escribirEnConsola("INFO", "El elemento no existe en la BD, enviando respuesta: noDuplicado");
                        enviarRespuesta("noDuplicado", null, null, null, response);
                    }

                }
            }
            catch (Exception e)
            {
                ConsolaDebug.escribirEnConsola("ERROR", "Error procesando la peticion {0}", peticionActual.peticion);
                Console.WriteLine("Exception: {0}", e);
                ConsolaDebug.escribirEnConsola("WARNING", "Cerrando hilo de petición...");
                Thread.CurrentThread.Abort();
            }
            finally
            {
                ConsolaDebug.escribirEnConsola("DEBUG", "Hilo {0} finalizado", nombreHilo);
            }
            

        }
    
        public static async void enviarRespuesta(string resp, string token, string sal, List<Enlaces> colec, HttpListenerResponse response)
        {
            string salCifrada = null;
            string tokenCifrado = null;
            //escribirEnConsola("DEBUG", "Token sin cifrar: {0}, token);
            //escribirEnConsola("DEBUG", "Sal sin cifrar: {0}", sal);
            if (sal != null)
            {
                salCifrada = CifradoJson.Cifrado(sal, resp);
            }
            if (token != null)
            {
                tokenCifrado = CifradoJson.Cifrado(token, resp);
            }
            var respuesta = new Respuesta
            {
                respuesta = resp,
                token = tokenCifrado,
                salt = salCifrada,
                coleccion = colec

            };
            //escribirEnConsola("DEBUG", "Respuesta: {0}, respuesta.respuesta);
            //escribirEnConsola("DEBUG", "Token cifrado: {0}, respuesta.token);
            //escribirEnConsola("DEBUG", "Sal cifrada: {0}", respuesta.salt);

            // Serializa nuestra clase en una cadena JSON
            var stringRespuesta = await Task.Run(() => JsonConvert.SerializeObject(respuesta));
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(stringRespuesta);
            response.ContentType = "application/json";
            response.ContentLength64 = buffer.Length;
            try
            {
                System.IO.Stream output = response.OutputStream;
                //Envia respuesta(Clave) al cliente
                output.Write(buffer, 0, buffer.Length);
                // Cierra output stream.
                output.Close();
                ConsolaDebug.escribirEnConsola("INFO", "Respuesta enviada al cliente");
            }
            catch (System.Net.HttpListenerException)
            {
                ConsolaDebug.escribirEnConsola("WARNING", "Cliente desconectado, imposible enviar respuesta");
            }
        }
        

    }
}
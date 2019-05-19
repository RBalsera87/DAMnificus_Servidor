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
                    Prefixes = { "http://localhost:8080/damnificus/" },
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
                    //ConsolaDebug.escribirEnConsola("DEBUG", "Start of client JSON data:");                   
                    string datosJson = reader.ReadToEnd(); // Convierte los datos en una cadena.
                    //ConsolaDebug.escribirEnConsola("DEBUG",datosJson);
                    //ConsolaDebug.escribirEnConsola("DEBUG", "End of client data:");
                    //ConsolaDebug.escribirEnConsola("DEBUG", "Parseando la petición Json...");
                    var objetoJSON = JObject.Parse(datosJson);
                    //ConsolaDebug.escribirEnConsola("DEBUG", "Peticion: {0}", (string)objetoJSON["peticion"]);
                    //ConsolaDebug.escribirEnConsola("DEBUG", "Usuario: {0}", (string)objetoJSON["usuario"]);
                    //ConsolaDebug.escribirEnConsola("DEBUG", "Contraseña: {0}", (string)objetoJSON["clave"]);
                    //ConsolaDebug.escribirEnConsola("DEBUG", "Token: {0}", (string)objetoJSON["token"]);                   

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
                //ConsolaDebug.escribirEnConsola("DEBUG", "Contraseña descifrada: " + peticionActual.clave);
                //ConsolaDebug.escribirEnConsola("DEBUG", "Token descifrado: " + peticionActual.token);

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

                // Petición de credenciales del usuario
                else if (peticionActual.peticion.Equals("obtenerCredenciales"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida peticion de credenciales por el usuario {0}", peticionActual.usuario);
                    string rango = conexUsuarios.obtenerCredenciales(peticionActual.usuario);
                    if (rango != null)
                    {
                        enviarRespuesta(rango, null, null, null, response);
                        ConsolaDebug.escribirEnConsola("INFO", "Respondiendo con el rango: {0}", rango);
                    }
                    else
                    {
                        enviarRespuesta("error", null, null, null, response);
                        ConsolaDebug.escribirEnConsola("WARNING", "¡ATENCIóN! Problema al obtener las credenciales del usuario {0}", peticionActual.usuario);
                    }
                }

                // Petición para obtener los enlaces
                else if (peticionActual.peticion.Equals("obtenerColeccionEnlaces"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida peticion de enlaces por el usuario {0}", peticionActual.usuario);
                    string asignatura = peticionActual.datos["asignatura"];
                    string usuario = peticionActual.usuario;
                    List<Enlaces> coleccion = conexEnlaces.obtenerColeccionEnlaces(asignatura,usuario);
                    if(coleccion == null)
                    {
                        enviarRespuesta("coleccionEnviada", null, null, null, response);
                        ConsolaDebug.escribirEnConsola("INFO", "Colección vacia");
                    }
                    else
                    {
                        enviarRespuesta("coleccionEnviada", null, null, JArray.FromObject(coleccion), response);
                        ConsolaDebug.escribirEnConsola("INFO", "Colección enviada al cliente satisfactoriamente");
                    }
                    
                }
                //Petición borrar enlace
                else if (peticionActual.peticion.Equals("borrarEnlace"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida peticion de eliminar enlace por el usuario {0}", peticionActual.usuario);

                    int id = int.Parse(peticionActual.datos["id"]);
                    var actualizado = conexEnlaces.borrarEnlace(id);
                    if (actualizado)
                    {
                        enviarRespuesta("correcto", null, null, null, response);
                        ConsolaDebug.escribirEnConsola("INFO", "Enlace borrado correctamente");
                    }
                    else
                    {
                        enviarRespuesta("incorrecto", null, null, null, response);
                        ConsolaDebug.escribirEnConsola("INFO", "Borrado de enlace incorrecto");
                    }

                }
                // Petición para sumar votación a un enlace
                else if (peticionActual.peticion.Equals("sumarYRestarValoracion"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida peticion de sumar valoracion por el usuario {0}", peticionActual.usuario);
                    
                    int id = int.Parse(peticionActual.datos["id"]);
                    string operacion = peticionActual.datos["operacion"];
                    string actualizado = conexEnlaces.sumarYRestarValoracion(id,operacion);
                    if (actualizado.Equals("correcto"))
                    {
                        enviarRespuesta(actualizado, null, null,null, response);
                        ConsolaDebug.escribirEnConsola("INFO", "Actualizacion de valoración realizada correctamente");
                    }
                    else
                    {
                        enviarRespuesta(actualizado, null, null, null, response);
                        ConsolaDebug.escribirEnConsola("INFO", "Actualizacion de valoración incorrecta");
                    }
                    
                }
                else if (peticionActual.peticion.Equals("cambiarActivoRevisionDesactivo")){
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida peticion de cambiar estado Activo/Desactivo/Revisión por el usuario {0}", peticionActual.usuario);

                    int id = int.Parse(peticionActual.datos["id"]);
                    string user = peticionActual.usuario;
                    string actualizado =conexEnlaces.cambiarActivoRevisionDesactivo(id, user);
                    if (actualizado.Equals("correcto"))
                    {
                        string email = "damnificusjovellanos@gmail.com";
                        EnviarEmail.reporte(peticionActual.usuario, email, "Link caído","El link con id " + id + " ha sido reportado como caido. Revísenlo.");
                        enviarRespuesta(actualizado, null, null, null, response);
                        ConsolaDebug.escribirEnConsola("INFO", "Actualizacion enlace Activo/Desactivo/Revisión realizada correctamente");
                    }
                    else
                    {
                        enviarRespuesta(actualizado, null, null, null, response);
                        ConsolaDebug.escribirEnConsola("INFO", "Actualizacion enlace Activo/Desactivo/Revisión incorrecta");
                    }
                }

                // Petición de envio de token a email por registro
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
                    else
                    {
                        enviarRespuesta("error", null, null, null, response);
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

                // Petición de cambiar clave
                else if (peticionActual.peticion.Equals("cambiarPass"))
                {
                    
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida petición de cambiar contraseña por el usuario {0}", peticionActual.usuario);
                    // Consulta clave
                    claveEncriptada = conexUsuarios.obtenerPassHash(peticionActual);
                    // Comprueba que la clave es la misma
                    bool claveValida = Clave.validarClave(peticionActual.clave, claveEncriptada);
                    if (claveValida)
                    {
                        ConsolaDebug.escribirEnConsola("INFO", "La contraseña es válida, cambiando por nueva...");
                        // Se usa espacio del token para la pass nueva
                        if (conexUsuarios.actualizarPassEnBBDD(peticionActual.token, peticionActual.usuario))
                        {
                            ConsolaDebug.escribirEnConsola("INFO", "Nueva contraseña guardada en BD existósamente");
                            enviarRespuesta("passCambiada", null, null, null, response);
                        }
                        else
                        {
                            ConsolaDebug.escribirEnConsola("WARNING", "¡ATENCIóN! Error al guardar la nueva contraseña en BD");
                            enviarRespuesta("passNoCambiada", null, null, null, response);
                        }

                    }
                    else
                    {
                        ConsolaDebug.escribirEnConsola("INFO", "Contraseña no válida");
                        enviarRespuesta("passNoValida", null, null, null, response);
                    }

                }

                // Petición para obtener el curso del usuario
                else if (peticionActual.peticion.Equals("obtenerCurso"))
                {
                    // Comprobamos el token de sesión
                    if (comprobarTokenValido(peticionActual, conexUsuarios, response))
                    {
                        ConsolaDebug.escribirEnConsola("INFO+", "Recibida petición de obtener curso por el usuario {0}", peticionActual.usuario);
                        string curso = conexEnlaces.obtenerCurso(peticionActual.usuario);
                        if (curso != null)
                        {
                            //ConsolaDebug.escribirEnConsola("DEBUG", "Respuesta: {0}", "curso" + curso);
                            enviarRespuesta("curso" + curso, null, null, null, response);
                            ConsolaDebug.escribirEnConsola("INFO", "Respondiendo al usuario con curso: {0}", curso);
                        }
                        else
                        {
                            enviarRespuesta("error", null, null, null, response);
                            ConsolaDebug.escribirEnConsola("WARNING", "¡ATENCIóN! Error al obtener el curso del usuario en la BD");
                        }
                    }

                }

                //Peticion para obtener el ID en la base de datos del usuario que hace la petición.
                else if (peticionActual.peticion.Equals("sacarUsuario"))
                {
                    // Comprobamos el token de sesión
                    if (comprobarTokenValido(peticionActual, conexUsuarios, response))
                    {
                        ConsolaDebug.escribirEnConsola("INFO+", "Recibida petición de obtener el ID de {0}", peticionActual.usuario);
                        string usuario = peticionActual.datos["usuario"];
                        int user = conexEnlaces.sacarUsuario(usuario);
                        if (user != 0)
                        {
                            //ConsolaDebug.escribirEnConsola("DEBUG", "Respuesta: {0}", "curso" + curso);
                            enviarRespuesta(user.ToString(), null, null, null, response);
                            ConsolaDebug.escribirEnConsola("INFO", "Respondiendo al usuario con ID: {0}", user.ToString());
                        }
                        else
                        {
                            enviarRespuesta("error", null, null, null, response);
                            ConsolaDebug.escribirEnConsola("WARNING", "¡ATENCIóN! Error al obtener el curso del usuario en la BD");
                        }
                    }
                }

                // Petición para cambiar el curso actual del usuario
                else if (peticionActual.peticion.Equals("cambiarCurso"))
                {
                    // Comprobamos el token de sesión
                    if (comprobarTokenValido(peticionActual, conexUsuarios, response))
                    {
                        ConsolaDebug.escribirEnConsola("INFO+", "Recibida petición de cambiar curso actual por el usuario {0}", peticionActual.usuario);
                        string curso = peticionActual.datos["curso"];
                        int numcurso = int.Parse(curso.Substring(curso.Length - 1, 1));
                        if (conexEnlaces.cambiarCurso(peticionActual.usuario, numcurso))
                        {
                            enviarRespuesta("cursoCambiado", null, null, null, response);
                            ConsolaDebug.escribirEnConsola("INFO", "Nuevo {0} guardado en BD existósamente", curso);
                        }
                        else
                        {
                            enviarRespuesta("error", null, null, null, response);
                            ConsolaDebug.escribirEnConsola("WARNING", "¡ATENCIóN! Error al cambiar el curso del usuario en la BD");
                        }
                    }

                }

                // Petición de envio de reporte por email
                else if (peticionActual.peticion.Equals("emailReporte"))
                {
                    // Comprobamos el token de sesión
                    if (comprobarTokenValido(peticionActual, conexUsuarios, response))
                    {
                        ConsolaDebug.escribirEnConsola("INFO+", "Recibida petición de envio de reporte por {0}", peticionActual.usuario);
                        string email = "damnificusjovellanos@gmail.com"; // Cambiar esta dirección por una buena
                        if (EnviarEmail.reporte(peticionActual.usuario, email, peticionActual.datos["titulo"], peticionActual.datos["reporte"]))
                        {
                            enviarRespuesta("emailReporteEnviado", null, null, null, response);
                            ConsolaDebug.escribirEnConsola("INFO", "¡Email de reporte enviado!");
                        }
                        else
                        {
                            enviarRespuesta("error", null, null, null, response);
                        }
                    }
                }

                // Petición de envio de token a email por contraseña perdida
                else if (peticionActual.peticion.Equals("emailPassPerdida"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida petición de token para restaurar contraseña por {0}", peticionActual.usuario);
                    string token = GeneradorTokens.GenerarToken(64);
                    ConsolaDebug.escribirEnConsola("DEBUG", "Token generado: {0}", token);
                    string email = peticionActual.datos["email"];
                    ConsolaDebug.escribirEnConsola("INFO", "Enviando token a: {0}", email);
                    if (EnviarEmail.passPerdida(email, token))
                    {
                        enviarRespuesta("emailConTokenEnviado", token, null, null, response);
                        ConsolaDebug.escribirEnConsola("INFO", "¡Email enviado! Esperando confirmación.");
                    }
                    else
                    {
                        enviarRespuesta("error", null, null, null, response);
                    }
                }

                // Petición de restaurar clave
                else if (peticionActual.peticion.Equals("restaurarPass"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida petición de restaurar contraseña desde email: {0}", peticionActual.usuario);
                    // Consulta clave
                    claveEncriptada = conexUsuarios.obtenerPassHash(peticionActual);
                    if (conexUsuarios.actualizarPassEnBBDD(peticionActual.clave, peticionActual.usuario))
                    {
                        ConsolaDebug.escribirEnConsola("INFO", "Nueva contraseña guardada en BD existósamente");
                        enviarRespuesta("passCambiada", null, null, null, response);
                    }
                    else
                    {
                        ConsolaDebug.escribirEnConsola("WARNING", "¡ATENCIóN! Error al guardar la nueva contraseña en BD");
                        enviarRespuesta("passNoCambiada", null, null, null, response);
                    }

                }

                // Petición para obtener el nombre de las asignaturas de un curso.
                else if (peticionActual.peticion.Equals("obtenerNombreAsignaturas"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida peticion de asignaturas por el usuario {0}", peticionActual.usuario);
                    string curso = peticionActual.datos["curso"];
                    List<string> coleccion = conexEnlaces.obtenerNombreAsignaturas(curso);
                    if (coleccion == null)
                    {
                        enviarRespuesta("coleccionEnviada", null, null, null, response);
                        ConsolaDebug.escribirEnConsola("INFO", "Colección vacia");
                    }
                    else
                    {
                        enviarRespuesta("coleccionEnviada", null, null, JArray.FromObject(coleccion), response);
                        ConsolaDebug.escribirEnConsola("INFO", "Colección enviada al cliente satisfactoriamente");
                    }
                }

                // Petición para obtener el nombre de los temas de una asignatura.
                else if (peticionActual.peticion.Equals("obtenerNombreTemas"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida peticion de temas por el usuario {0}", peticionActual.usuario);
                    string asignatura = peticionActual.datos["asignatura"];
                    List<string> coleccion = conexEnlaces.obtenerNombreTemas(asignatura);
                    if (coleccion == null)
                    {
                        enviarRespuesta("coleccionEnviada", null, null, null, response);
                        ConsolaDebug.escribirEnConsola("INFO", "Colección vacia");
                    }
                    else
                    {
                        enviarRespuesta("coleccionEnviada", null, null, JArray.FromObject(coleccion), response);
                        ConsolaDebug.escribirEnConsola("INFO", "Colección enviada al cliente satisfactoriamente");
                    }
                }

                // Petición para subir un nuevo enlace
                else if (peticionActual.peticion.Equals("subirEnlace"))
                {
                    // Comprobamos el token de sesión
                    if (comprobarTokenValido(peticionActual, conexUsuarios, response))
                    {
                        ConsolaDebug.escribirEnConsola("INFO+", "Recibida petición de subir enlace por el usuario {0}", peticionActual.usuario);
                        string rango = conexUsuarios.obtenerCredenciales(peticionActual.usuario);
                        string titulo = peticionActual.datos["titulo"];
                        string imagen = peticionActual.datos["imagen"];
                        string descripcion = peticionActual.datos["descripcion"];
                        string tipo = peticionActual.datos["tipo"];
                        string enlace = peticionActual.datos["enlace"];
                        string tema = peticionActual.datos["tema"];
                        
                        if (conexEnlaces.introducirNuevoEnlace(peticionActual.usuario, titulo, imagen, descripcion, tipo, enlace, tema, rango))
                        {
                            ConsolaDebug.escribirEnConsola("INFO", "Nuevo enlace guardado en BD existósamente");
                            enviarRespuesta("enlaceInsertado", null, null, null, response);
                        }
                        else
                        {
                            ConsolaDebug.escribirEnConsola("WARNING", "¡ATENCIóN! Error al guardar el nuevo enlace en BD");
                            enviarRespuesta("errorInsercion", null, null, null, response);
                        }
                    }
                        

                }

                // Petición para obtener el listado completo de las notas del curso en el que está matriculado el usuario.
                else if (peticionActual.peticion.Equals("recogidaNotas"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida peticion de notas individuales por el usuario {0}", peticionActual.usuario);
                    string curso = peticionActual.datos["curso"];
                    string user = peticionActual.datos["usuario"];
                    List<double> coleccion = conexEnlaces.recogidaNotas(curso, user);
                    if (coleccion == null)
                    {
                        enviarRespuesta("coleccionEnviada", null, null, null, response);
                        ConsolaDebug.escribirEnConsola("INFO", "Colección vacia");
                    }
                    else
                    {
                        enviarRespuesta("coleccionEnviada", null, null, JArray.FromObject(coleccion), response);
                        ConsolaDebug.escribirEnConsola("INFO", "Colección enviada al cliente satisfactoriamente");
                    }
                }

                // Peticion para obtener el listado de las medias de las notas del curso en el que esta matriculado el usuario.
                else if (peticionActual.peticion.Equals("mediaNotas"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida peticion de notas medias por el usuario {0}", peticionActual.usuario);
                    string curso = peticionActual.datos["curso"];
                    string user = peticionActual.datos["usuario"];
                    List<double> coleccion = conexEnlaces.mediaNotas(curso, user);
                    if (coleccion == null)
                    {
                        enviarRespuesta("coleccionEnviada", null, null, null, response);
                        ConsolaDebug.escribirEnConsola("INFO", "Colección vacia");
                    }
                    else
                    {
                        enviarRespuesta("coleccionEnviada", null, null, JArray.FromObject(coleccion), response);
                        ConsolaDebug.escribirEnConsola("INFO", "Colección enviada al cliente satisfactoriamente");
                    }
                }

                // Petición para saber si hay una nota introducida > 0 en la base de datos
                else if(peticionActual.peticion.Equals("hayNota"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida peticion de comprobación de nota por el usuario {0}", peticionActual.usuario);
                    string trimestre = peticionActual.datos["trimestre"];
                    string asignatura = peticionActual.datos["asignatura"];
                    string usuario = peticionActual.datos["usuario"];
                    string hay = conexEnlaces.hayNota(trimestre, asignatura, usuario);
                    enviarRespuesta(hay, null, null, null, response);
                    ConsolaDebug.escribirEnConsola("INFO", "Información enviada correctamente");
                }

                // Petición para cambiar la nota al usuario
                else if (peticionActual.peticion.Equals("agregarNota"))
                {
                    ConsolaDebug.escribirEnConsola("INFO+", "Recibida peticion de cambio de nota por el usuario {0}", peticionActual.usuario);
                    string nota = peticionActual.datos["nota"];
                    string trimestre = peticionActual.datos["trimestre"];
                    string asignatura = peticionActual.datos["asignatura"];
                    string usuario = peticionActual.datos["usuario"];
                    conexEnlaces.agregarNota(nota, trimestre, asignatura, usuario);
                    enviarRespuesta(null, null, null, null, response);
                    ConsolaDebug.escribirEnConsola("INFO", "Nota cambiada");
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
    
        public static async void enviarRespuesta(string resp, string token, string sal, JArray colec, HttpListenerResponse response)
        {
            string salCifrada = null;
            string tokenCifrado = null;
            //ConsolaDebug.escribirEnConsola("DEBUG", "Token sin cifrar: {0}, token);
            //ConsolaDebug.escribirEnConsola("DEBUG", "Sal sin cifrar: {0}", sal);
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
            //ConsolaDebug.escribirEnConsola("DEBUG", "Respuesta: {0}, respuesta.respuesta);
            //ConsolaDebug.escribirEnConsola("DEBUG", "Token cifrado: {0}, respuesta.token);
            //ConsolaDebug.escribirEnConsola("DEBUG", "Sal cifrada: {0}", respuesta.salt);

            // Serializa nuestra clase en una cadena JSON
            var stringRespuesta = await Task.Run(() => JsonConvert.SerializeObject(respuesta));
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(stringRespuesta);
            response.ContentType = "application/json";
            response.ContentLength64 = buffer.Length;
            try
            {
                System.IO.Stream output = response.OutputStream;
                // Envia respuesta al cliente
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
        public static bool comprobarTokenValido(Peticion peticionActual, ConexionUsuarios conexUsuarios, HttpListenerResponse response)
        {

            string tokenBD = conexUsuarios.obtenerToken(peticionActual.usuario);
            if (tokenBD.Equals(peticionActual.token))
            {
                ConsolaDebug.escribirEnConsola("INFO", "Token del cliente {0} válido", peticionActual.usuario);
                return true;
            }else
            {
                ConsolaDebug.escribirEnConsola("WARNING", "¡ATENCIóN! Token del usuario no válido, petición {0} rechazada", peticionActual.peticion);
                enviarRespuesta("tokenNoValido", null, null, null, response);
                return false;
            }
            
        }

    }
}
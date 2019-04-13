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
            HttpListener httpListener = null;
            try
            {
                // Create an instance of HTTP Listener class
                httpListener = new HttpListener
                {
                    Prefixes = { "http://localhost:8080/" },                
                };

                httpListener.Start();
                // Waiting synchronously till the request is received from the client for HTTP over port 80
                while (true)
                {   
                    //Meto las siguientes cuatro lineas en el bucle porque al estar fuera repite el bucle 
                    //ya que no vuelve a esperar ninguna conexión y al dar la segunda vuelta pierde el json y peta
                    Console.Write("Esperando a conectar... ");
                    var context = httpListener.GetContext();
                    var request = context.Request;
                    HttpListenerResponse response = context.Response;
                    Console.WriteLine("¡Conectado!");
                    System.IO.Stream body = request.InputStream;
                    System.Text.Encoding encoding = request.ContentEncoding;
                    System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
                    if (request.ContentType != null)
                    {
                        Console.WriteLine("\nClient data content type {0}", request.ContentType);
                    }
                    Console.WriteLine("Client data content length {0}", request.ContentLength64);
                    Console.WriteLine("Start of client JSON data:");
                    // Convert the data to a string and display it on the console.
                    string s = reader.ReadToEnd();
                    Console.WriteLine(s);                
                    Console.WriteLine("End of client data:");
                    Console.WriteLine("Parsing the JSON Request Body.....");
                    var objetoJSON = JObject.Parse(s);
                    Console.WriteLine("Peticion : " + (string)objetoJSON["peticion"]);
                    Console.WriteLine("Usuario : " + (string)objetoJSON["usuario"]);
                    Console.WriteLine("Contraseña : " + (string)objetoJSON["clave"]);
                    Console.WriteLine("Token : " + (string)objetoJSON["token"]);

                    //Serializa el objeto JSON en un objeto .NET
                    Peticion peticionActual = objetoJSON.ToObject<Peticion>();
                    
                    //Descifra la peticion
                    if (peticionActual.clave != null)
                    {                        
                        peticionActual.clave = CifradoJson.Descifrado(peticionActual.clave, peticionActual.peticion);
                    }
                    if (peticionActual.token != null)
                    {
                        peticionActual.token = CifradoJson.Descifrado(peticionActual.token, peticionActual.peticion);
                    }
                    peticionActual.usuario = CifradoJson.Descifrado(peticionActual.usuario, peticionActual.peticion);
                    // Para depurar: La mostramos descifrada
                    Console.WriteLine("Usuario descifrado : " + peticionActual.usuario);
                    Console.WriteLine("Contraseña descifrada: " + peticionActual.clave);
                    Console.WriteLine("Token descifrado: " + peticionActual.token);

                    ConexionUsuarios conexUsuarios = new ConexionUsuarios();
                    ConexionEnlaces conexEnlaces = new ConexionEnlaces();
                    //Crea variable string de respuesta para el cliente

                    //Consulta la clave del usuario en la BD si clave es null retorna "sal" al cliente si no comprueba
                    //si la clave es la misma y retorna "Token" falta hacer JSON
                    string claveEncriptada;
                    
                    if (peticionActual.peticion.Equals("requestSalt"))
                    {
                        Console.WriteLine("Recibida peticion de sal por el usuario {0}", peticionActual.usuario);
                        //Consulta clave 
                        claveEncriptada = conexUsuarios.consultaPeticion(peticionActual);
                        if (!claveEncriptada.Equals("null"))
                        {
                            Console.WriteLine("Respondiendo con SALT\nEsperando peticion login...");
                            enviarRespuesta("usuarioEncontrado", null, Clave.getSal(claveEncriptada), null, response);
                        }
                        else
                        {
                            Console.WriteLine("Respuesta: No se ha encontrado el usuario en la BBDD");
                            enviarRespuesta("noExisteUsuario", null, null, null, response);
                        }

                    }
                    else if (peticionActual.peticion.Equals("login"))
                    {
                        Console.WriteLine("Recibida peticion de login por el usuario {0}", peticionActual.usuario);
                        //Consulta clave
                        claveEncriptada = conexUsuarios.consultaPeticion(peticionActual);
                        //Comprueba que la clave el la misma
                        bool claveValida = Clave.validarClave(peticionActual.clave, claveEncriptada);
                        if (claveValida)
                        {
                            Console.WriteLine("La contraseña es valida");
                            string token = GeneradorTokens.GenerarToken(64);
                            if (conexUsuarios.actualizarTokenEnBBDD(token, peticionActual.usuario))
                            {
                                Console.WriteLine("Token guardado en BD existósamente");
                            }else
                            {
                                Console.WriteLine("¡ATENCIóN! Error al guardar token en BD");
                            }
                            Console.WriteLine("Enviando token: {0}", token);
                            enviarRespuesta("passValida", token, null, null, response);
                        }
                        else
                        {
                            Console.WriteLine("Contraseña no valida");
                            enviarRespuesta("passNoValida", null, null, null, response);
                        }

                    }
                    else if (peticionActual.peticion.Equals("borrarToken"))
                    {
                        if (conexUsuarios.actualizarTokenEnBBDD("", peticionActual.usuario))
                        {
                            enviarRespuesta("tokenBorrado", null, null, null, response);
                            Console.WriteLine("Token del usuario {0} borrado con éxito de la BD", peticionActual.usuario);
                        }
                        else
                        {
                            enviarRespuesta("error", null, null, null, response);
                            Console.WriteLine("¡ATENCIóN! Problema al borrar el token del usuario {0}", peticionActual.usuario);
                        }
                    }
                    else if (peticionActual.peticion.Equals("obtenerColeccionEnlaces"))
                    {
                        Console.WriteLine("Recibida peticion de enlaces");
                        List<Enlaces> coleccion = conexEnlaces.obtenerColeccionEnlaces();
                        enviarRespuesta("coleccionEnviada", null, null,coleccion, response);
                        Console.WriteLine("Colección enviada al cliente satisfactoriamente");
                    }
                    else if (peticionActual.peticion.Equals("emailRegistro"))
                    {
                        string token = GeneradorTokens.GenerarToken(64);
                        string email = peticionActual.datos["email"];
                        if(EnviarEmail.registro(email, token))
                        {
                            enviarRespuesta("emailConTokenEnviado", token, null, null, response);
                        }
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
            finally
            {
                Console.Read();
                // Stop listening for new clients.
                httpListener.Close();
            }
        }
        public static async void enviarRespuesta(string resp, string token, string sal, List<Enlaces> colec, HttpListenerResponse response)
        {
            string salCifrada = null;
            string tokenCifrado = null;
            Console.WriteLine("Token sin cifrar: {0}\nSal sin cifrar: {1}", token, sal);
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
            Console.WriteLine("Enviando:\nRespuesta: {0}\nTokenCifrado: {1}\nSalCifrada: {2}", respuesta.respuesta, respuesta.token, respuesta.salt);
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
                Console.WriteLine("\nENVIADO!");
            }
            catch (System.Net.HttpListenerException e)
            {
                Console.WriteLine("\nCliente no disponible");
            }
            

        }










        static void metodoMainAntiguo()//static void Main(string[] args)
        {
                TcpListener server = null;
                try
                {
                    Int32 port = 8080; //O cualquier otro siempre que no interfiera con los ya existentes
                    IPAddress localAddr = IPAddress.Parse("127.0.0.1"); //Nos mantenemos a la escucha en "localhost"

                    server = new TcpListener(localAddr, port);

                    server.Start(); //Nos mantenemos a la espera de nuevas peticiones
                    Byte[] bytes = new Byte[1000]; //Array donde guardaremos el resultado
                    String data = null; //Cadena de caracteres que contendrá los datos una vez procesados

                    // Enter the listening loop.
                    while (true)
                    {
                        Console.Write("Esperando a conectar... ");

                        TcpClient client = server.AcceptTcpClient(); //Aceptamos la conexión entrante
                        Console.WriteLine("¡Conectado!");

                        data = null;

                        NetworkStream stream = client.GetStream(); //Obtenemos el stream para lectura y escritura
                        int i = stream.Read(bytes, 0, bytes.Length); //Leemos en el array "bytes" y almacenamos en i el numero de bytes leidos.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i); //Convertimos la cadena
                        Console.WriteLine("Recibido: {0}", data); //Mostramos por pantalla el resultado.

                        //Dividimos el mensaje en un array de strings
                        var lista = data.Split(' ');
                        //Por si acaso la petición viene vacía.
                        if (lista.Length < 3) continue;
                        //El primer elemento de la lista será la instrucción
                        var instruccion = lista[0];
                        //El segundo elemento de la lista será la ruta
                        var ruta = lista[1];
                        //El tercer elemento antes del salto de carro, será el protocolo
                        string protocolo = lista[2].Split('\n')[0];
                        Console.WriteLine(data);
                        //Finalmente mostramos los datos por pantalla
                        Console.WriteLine("Instruccion: {0}\nRuta: {1}\nProtocolo: {2}", instruccion, ruta, protocolo);
                        byte[] msg;

                        //Comprobamos que estemos recibiendo la peticion login
                        if (ruta.Equals("/login"))
                        {
                            //Leemos todo el contenido del fichero especificado
                            //var fichero = File.ReadAllText("home.html");
                            //Redactamos la cabecera de respuesta.
                            string response = "HTTP/1.1 200 OK\r\n\r\n\r\n";
                            //Agregamos a la cabecera la informacion del fichero.
                            //response = response + fichero;
                            //Mostramos por pantalla el resultado
                            Console.WriteLine("Sent: {0}", response);
                            //Codificamos el texto que hemos cargado en un array de bytes
                            msg = System.Text.Encoding.ASCII.GetBytes(response);
                            //Escribimos en el stream el mensaje codiificado
                            stream.Write(msg, 0, msg.Length);
                        }
                        else
                        {
                            //Redactamos una cabecera de fichero no encontrado
                            string response = "HTTP/1.1 404 Not Found";
                            //Mostramos por pantalla el resultado
                            Console.WriteLine("Sent: {0}", response);
                            //Codificamos, exactamente igual que en la parte superior
                            msg = System.Text.Encoding.ASCII.GetBytes(response);
                            //Escribimos en el stream el mensaje codificado
                            stream.Write(msg, 0, msg.Length);
                        }

                        // Send back a response.
                        // Shutdown and end connection
                        client.Close();
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e);
                }
                finally
                {
                    // Stop listening for new clients.
                    server.Stop();
                }


                Console.WriteLine("\nPulsa INTRO para continuar...");
                Console.Read();
            }
        }
    }

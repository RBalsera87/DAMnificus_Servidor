using EntidadesCompartidas;
using MySql.Data.MySqlClient;
using ServidorConexion.Negocio;
using System;
using System.Collections.Generic;

namespace ServidorConexion
{   
    class ConexionUsuarios
    {
        private string cadenaConexion;
        private MySqlConnection conexion;

       
        public ConexionUsuarios() { }

        private void conectar()
        {
            try
            {
                
                //if (conexion != null && conexion.State != ConnectionState.Open)
                //{
                    cadenaConexion = "Server=localhost;Database=damnificus_usuarios;Uid=root;Pwd=;";
                    conexion = new MySqlConnection(cadenaConexion);
                    conexion.Open();
                //}
                    
            }
            catch (MySqlException)
            {
                throw;
            }
        }

        public string obtenerPassHash(Peticion peticion)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            //La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            cmd.CommandText = "Select pass from usuarios where BINARY usuario = @user";
            cmd.Parameters.AddWithValue("@user", peticion.usuario);
            //cmd.Parameters.AddWithValue("@pass", pass);
            cmd.Connection = conexion;
            MySqlDataReader login = cmd.ExecuteReader();
            if (login.Read())
            {
                string clave = login.GetString(0);
                conexion.Close();
                return clave;
            }
            else
            {
                conexion.Close();
                return "null";
            }

        }
        public bool actualizarTokenEnBBDD(string token, string usuario)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            // La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            string sql = "UPDATE credenciales SET Token = @token WHERE Id = (SELECT Id FROM usuarios WHERE BINARY Usuario = @user)";
            cmd.Parameters.AddWithValue("@token", token);
            cmd.Parameters.AddWithValue("@user", usuario);
            cmd.CommandText = sql;
            cmd.Connection = conexion;
            if (cmd.ExecuteNonQuery() == 1) // El token se ha guardado
            {                
                conexion.Close();
                return true;
            }else
            {
                conexion.Close();
                return false;
            }
        }
        public bool comprobarSiExisteEnBD(Peticion peticionActual)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();            
            if (peticionActual.peticion.Equals("buscaEmailenBD"))
            {
                cmd.CommandText = "Select email from usuarios where BINARY email = @arg";
                cmd.Parameters.AddWithValue("@arg", peticionActual.datos["email"]);
            }
            else if (peticionActual.peticion.Equals("buscaUsuarioenBD"))
            {
                cmd.CommandText = "Select usuario from usuarios where BINARY usuario = @arg";
                cmd.Parameters.AddWithValue("@arg", peticionActual.usuario);
            }
            cmd.Connection = conexion;
            MySqlDataReader resultado = cmd.ExecuteReader();
            if (resultado.HasRows)
            {
                // Ya existe en BD
                conexion.Close();
                return true;
            }
            else
            {
                // No exxiste en BD
                conexion.Close();
                return false;
            }

        }
        public bool introducirUsuarioEnBBDD(string usuario, string email, string pass, string nombre, string apellidos)
        {
            conectar();
            MySqlTransaction Transaccion;
            Transaccion = conexion.BeginTransaction();
            ConsolaDebug.escribirEnConsola("INFO", "Comenzando transacción en BD");
            MySqlCommand cmd = new MySqlCommand();            
            cmd.Connection = conexion;
            cmd.Transaction = Transaccion;
            try
            {
                // Primer insert de la transacción
                string sql = "INSERT INTO usuarios VALUES( null, @user, @email, @pass, @name, @apell )";
                cmd.Parameters.AddWithValue("@user", usuario);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@pass", pass);
                cmd.Parameters.AddWithValue("@name", nombre);
                cmd.Parameters.AddWithValue("@apell", apellidos);
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                ConsolaDebug.escribirEnConsola("INFO", "Insert en usuarios ejecutado satisfactoriamente");
                // Segundo insert de la transacción
                sql = "INSERT INTO credenciales VALUES( (SELECT Id FROM usuarios WHERE usuario = @user), 'token', 'normal' )";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                Transaccion.Commit();
                ConsolaDebug.escribirEnConsola("INFO", "Insert en credenciales ejecutado satisfactoriamente");
                return true;
            }
            catch (Exception e)
            {
                try
                {
                    ConsolaDebug.escribirEnConsola("WARNING", "Problema en transacción, comenzando ROLLBACK");
                    Transaccion.Rollback();
                    ConsolaDebug.escribirEnConsola("INFO", "ROLLBACK ejecutado satisfactoriamente");
                }
                catch (MySqlException ex)
                {
                    ConsolaDebug.escribirEnConsola("WARNING", "Problema en ROLLBACK");
                    if (Transaccion.Connection != null)
                    {
                        ConsolaDebug.escribirEnConsola("ERROR","Excepción lanzada: {0}", ex.Message);
                    }
                }
                ConsolaDebug.escribirEnConsola("WARNING", "No se ha insertado nada en la BD");
                ConsolaDebug.escribirEnConsola("ERROR", "Excepción lanzada: {0}", e.Message);
                return false;
            }
            finally
            {
                conexion.Close();                
            }

        }

        public bool borrarUsuario(int id)
        {
            conectar();
            MySqlTransaction Transaccion;
            Transaccion = conexion.BeginTransaction();
            ConsolaDebug.escribirEnConsola("INFO", "Comenzando transacción de borrado en BD enlaces...");
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.Transaction = Transaccion;
            try
            {
                // Primer delete de la transacción
                string sql = "DELETE FROM credenciales WHERE id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                ConsolaDebug.escribirEnConsola("INFO", "Delete en credenciales ejecutado satisfactoriamente");
                // Segundo delete de la transacción
                sql = "DELETE FROM usuarios WHERE id = @id";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                Transaccion.Commit();
                ConsolaDebug.escribirEnConsola("INFO", "delete en usuarios ejecutado satisfactoriamente");
                return true;
            }
            catch (Exception e)
            {
                try
                {
                    ConsolaDebug.escribirEnConsola("WARNING", "Problema en transacción, comenzando ROLLBACK");
                    Transaccion.Rollback();
                    ConsolaDebug.escribirEnConsola("INFO", "ROLLBACK ejecutado satisfactoriamente");
                }
                catch (MySqlException ex)
                {
                    ConsolaDebug.escribirEnConsola("WARNING", "Problema en ROLLBACK");
                    if (Transaccion.Connection != null)
                    {
                        ConsolaDebug.escribirEnConsola("ERROR", "Excepción lanzada: {0}", ex.Message);
                    }
                }
                ConsolaDebug.escribirEnConsola("WARNING", "No se ha insertado nada en la BD");
                ConsolaDebug.escribirEnConsola("ERROR", "Excepción lanzada: {0}", e.Message);
                return false;
            }
            finally
            {
                conexion.Close();
            }

        }

        public bool borrarUsuarioDeBBDD(string usuario)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            // La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            string sql = "DELETE FROM usuarios WHERE usuario = @user";
            cmd.Parameters.AddWithValue("@user", usuario);
            cmd.CommandText = sql;
            cmd.Connection = conexion;
            if (cmd.ExecuteNonQuery() == 1) // El usuario se ha borrado
            {
                conexion.Close();
                return true;
            }
            else
            {
                conexion.Close();
                return false;
            }

        }
        public bool actualizarPassEnBBDD(string pass, string usuario)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            string sql = "";
            // La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            if (usuario.Contains("@"))
            {
                sql = "UPDATE usuarios SET pass = @pass WHERE email = @user";
            }                
            else
            {
                sql = "UPDATE usuarios SET pass = @pass WHERE usuario = @user";
            }               
            cmd.Parameters.AddWithValue("@pass", pass);
            cmd.Parameters.AddWithValue("@user", usuario);
            cmd.CommandText = sql;
            cmd.Connection = conexion;
            if (cmd.ExecuteNonQuery() == 1) // La pass se ha cambiado
            {
                conexion.Close();
                return true;
            }
            else
            {
                conexion.Close();
                return false;
            }
        }
        public string obtenerToken(string usuario)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "Select Token from credenciales where Id = (SELECT Id FROM usuarios WHERE usuario = @user)";
            cmd.Parameters.AddWithValue("@user", usuario);
            cmd.Connection = conexion;
            MySqlDataReader login = cmd.ExecuteReader();
            if (login.Read())
            {
                string token = login.GetString(0);
                conexion.Close();
                return token;
            }
            else
            {
                conexion.Close();
                return "null";
            }
        }
        public string obtenerCredenciales(string usuario)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "Select Rango from credenciales where Id = (SELECT Id FROM usuarios WHERE usuario = @user)";
            cmd.Parameters.AddWithValue("@user", usuario);
            cmd.Connection = conexion;
            MySqlDataReader retorno = cmd.ExecuteReader();
            if (retorno.Read())
            {
                string rango = retorno.GetString(0);
                conexion.Close();
                return rango;
            }
            else
            {
                conexion.Close();
                return "null";
            }
        }

        public List<Usuario> obtenerColeccionUsuarios(string user)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            
            cmd.CommandText = "SELECT u.id, u.usuario, u.email, u.nombre, u.apellidos, c.rango FROM usuarios u inner join credenciales c on u.id = c.Id where u.usuario <> @user; ";
            cmd.Parameters.AddWithValue("@user", user);
            cmd.Connection = conexion;
            MySqlDataReader Datos = cmd.ExecuteReader();
            List<Usuario> listaUsuarios = null;
            if (Datos.HasRows)
            {
                listaUsuarios = new List<Usuario>();
                while (Datos.Read())
                {   
                    Usuario usuario = new Usuario();
                    usuario.Id= Datos.GetString(0);
                    usuario.User = Datos.GetString(1);
                    usuario.Email = Datos.GetString(2);
                    usuario.Nombre = Datos.GetString(3);
                    usuario.Apellidos = Datos.GetString(4);
                    usuario.Credenciales = Datos.GetString(5);
                    if (!usuario.User.Equals("admin")){
                        listaUsuarios.Add(usuario);
                    }
                }
            }

            conexion.Close();
            return listaUsuarios;

        }

        public Dictionary<string,string> comprobarPermisosBorrarUsuarios(int idUserBorrar,string credenciales, string user)
        {
            string usuarioBorrar = "";
            string credencialesBorrar = "";
            Dictionary<string, string> permisos = new Dictionary<string, string>();
            string mensaje = "";
            string rango = "";
            conectar();
            MySqlCommand cmd = new MySqlCommand();

            cmd.CommandText = "SELECT u.usuario, c.rango FROM usuarios u join credenciales c on u.id = c.Id WHERE u.id = @id";
            cmd.Parameters.AddWithValue("@id", idUserBorrar);
            cmd.Connection = conexion;
            MySqlDataReader DatosBD = cmd.ExecuteReader();
            if (DatosBD.Read())
            {
                usuarioBorrar = DatosBD.GetString(0);
                credencialesBorrar = DatosBD.GetString(1);
                if (!usuarioBorrar.Equals("admin"))//No llegaría nunca al servidor la petición de borrar al usuario admin pero valido por seguridad
                {
                    if (credenciales.Equals("admin"))
                    {
                        if (user.Equals("admin"))
                        {
                            rango = "true";
                        }
                        else if (!credencialesBorrar.Equals("admin"))
                        {
                            rango = "true";
                        }
                        else
                        {
                            rango = "false";
                            mensaje = "Un usuario distinto a admin esta intentando borrar a un usuario con privilegios admin";
                        }
                    }
                    else
                    {
                        mensaje = "Usuario sin credenciales necesarias esta intentando borrar usuarios";
                    }
                }
                else
                {
                    mensaje = "Se esta intentando borrar al usuario Administrador de la BBDD";
                }
            }
            else
            {
                mensaje = "No existe el usuario en la BD";
            }
            conexion.Close();

            permisos.Add("rango", rango);
            permisos.Add("mensaje", mensaje);
            permisos.Add("usuarioBorrar", usuarioBorrar);
            return permisos;
        }

        public bool cambiarRango(int id, string usuario, string newRango)
        {
            conectar();
            //Comprobamos que existe el usuario en la BD ya que puede haber sido borrar a la vez por otro admin
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "Select rango from credenciales WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Connection = conexion;
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string rango = reader.GetString(0);
                if (rango.Equals(newRango))
                {
                    return true;
                }
                if (rango.Equals("admin") && !usuario.Equals("admin"))
                {
                    conexion.Close();
                    return false;
                }                
                conexion.Close();
                conectar();
                cmd = new MySqlCommand();
                string sql = "UPDATE credenciales SET rango = @newRango WHERE id = @id";
                cmd.Parameters.AddWithValue("@newRango", newRango);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandText = sql;
                cmd.Connection = conexion;
                if (cmd.ExecuteNonQuery() == 1) // El campo se ha modificado
                {
                    conexion.Close();
                    return true;
                }
                else
                {
                    conexion.Close();
                    return false;
                }

            }
            else
            {
                conexion.Close();
                return false;
            }


        }

    }
}

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
            // La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            string sql = "UPDATE usuarios SET pass = @pass WHERE Usuario = @user";
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

        
    }
}

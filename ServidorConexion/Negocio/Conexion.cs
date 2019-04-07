using MySql.Data.MySqlClient;
using System.Data;

namespace ServidorConexion
{   
    class Conexion
    {
        private string cadenaConexion;
        private MySqlConnection conexion;

       
        public Conexion() { }

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
            catch (MySqlException e)
            {
                throw;
            }
        }

        public string consultaPeticion(Peticion peticion)
        {
            if (peticion.peticion.Equals("requestSalt") || peticion.peticion.Equals("login"))
            {
                conectar();
                MySqlCommand cmd = new MySqlCommand();
                //La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
                cmd.CommandText = "Select pass_hash from usuarios where BINARY usuario=@cod";
                cmd.Parameters.AddWithValue("@cod", peticion.usuario);
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
            }else if (peticion.peticion.Equals("otraCosaNoprogramadaTodavia"))
            {
                return "foo";
            }
            else
            {
                return "bar";
            }
            
        }
        public bool actualizarTokenEnBBDD(string token, string usuario)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            //La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            string sql = "UPDATE credenciales SET Token=@token WHERE Id=(SELECT Id FROM usuarios WHERE BINARY Nombre=@user)";
            cmd.Parameters.AddWithValue("@token", token);
            cmd.Parameters.AddWithValue("@user", usuario);
            cmd.CommandText = sql;
            cmd.Connection = conexion;
            if (cmd.ExecuteNonQuery() == 1)
            {
                //El token se ha guardado
                conexion.Close();
                return true;
            }else
            {
                conexion.Close();
                return false;
            }
        }
    }
}

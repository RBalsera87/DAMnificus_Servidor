using MySql.Data.MySqlClient;

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
                cadenaConexion = "Server=localhost;Database=damnificus_usuarios;Uid=root;Pwd=;";
                conexion = new MySqlConnection(cadenaConexion);
                conexion.Open();
            }
            catch (MySqlException e)
            {
                throw;
            }
        }

        public string consultaPeticion(Peticion peticion)
        {
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            //La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            cmd.CommandText = "Select pass_hash from usuarios where BINARY usuario=@cod "; 
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
        }
    }
}

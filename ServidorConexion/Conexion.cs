using MySql.Data.MySqlClient;

namespace ServidorConexion
{   
    class Conexion
    {
        private string conex;
        private MySqlConnection conec;

       
        public Conexion() { }

        private void conectar()
        {
            try
            {
                conex = "Server=localhost;Database=damnificus_usuarios;Uid=root;Pwd=;";
                conec = new MySqlConnection(conex);
                conec.Open();
            }
            catch (MySqlException e)
            {
                throw;
            }
        }

        public string consultarClave(string usuario)
        {
            //string token = "";
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            //La palabra BINARY sirve para hacer distinción de mayúsculas y minúsculas
            cmd.CommandText = "Select pass from usuarios where BINARY usuario=@cod "; 
            cmd.Parameters.AddWithValue("@cod", usuario);
            //cmd.Parameters.AddWithValue("@pass", pass);
            cmd.Connection = conec;
            MySqlDataReader login = cmd.ExecuteReader();
            if (login.Read())
            {
                string clave = login.GetString(0);
                conec.Close();
                return clave;
            }
            else
            {
                conec.Close();
                return "null";
            }
        }
    }
}

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

        public string validar(string usuario, string pass)
        {
            string token = "";
            conectar();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "Select * from credenciales where codigo=@cod and pass=@pass"; //esto hay que adaptarlo para el hash y la sal
            cmd.Parameters.AddWithValue("@cod", usuario);
            cmd.Parameters.AddWithValue("@pass", pass);
            cmd.Connection = conec;
            MySqlDataReader login = cmd.ExecuteReader();
            if (login.Read())
            {
                conec.Close();
                return token;
            }
            else
            {
                conec.Close();
                return null;
            }
        }


    }
}

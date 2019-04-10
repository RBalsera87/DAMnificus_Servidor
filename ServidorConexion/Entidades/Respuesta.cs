using Newtonsoft.Json;
using ServidorConexion.Negocio;
using System.Collections.Generic;

namespace ServidorConexion
{
    class Respuesta
    {
        [JsonProperty("respuesta")]
        public string respuesta { get; set; }

        [JsonProperty("token")]
        public string token { get; set; }

        [JsonProperty("salt")]
        public string salt { get; set; }

        [JsonProperty("coleccion")]
        public List<Enlaces> coleccion { get; set; }
    }
}

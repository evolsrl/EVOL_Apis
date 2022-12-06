using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class LavUsuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Contrasenia { get; set; }
        public string Telefono { get; set; }
        public int IdEdificio { get; set; }

        public string Calle { get; set; }

        public int Altura { get; set; }

        public string Piso { get; set; }

        public string Departamento { get; set; }

        public int IdEstado { get; set; }

        public string Token { get; set; }

        public string Mensaje { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entidades
{
    public class UsuarioSim
    {
        public int IdAfiliado { get; set; }
        public string CorreoElectronico { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int IdWordpress { get; set; }
    }
}

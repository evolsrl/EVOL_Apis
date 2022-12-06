using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSim.Core.Models
{
    public class UsuarioWordpress
    {
        public string IdTipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string CorreoElectronico { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
    public class UsuarioSim
    {
        public int IdAfiliado { get; set; }
        public string CorreoElectronico { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int IdWordpress { get; set; }
    }
    public class Ids
    {
        public string IdAfiliado { get; set; }
        public string IdWordPress { get; set; }
    }
    public class DocumentoTipo
    {
        public int IdTipoDocumento { get; set; }
        public string TipoDocumento { get; set; }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entidades
{
    public class Control
    {
        public int IdWordpressPaginasControl { get; set; }
        public int IdCamposTipo { get; set; }
        public string Nombre { get; set; }
        public string Mostrar { get; set; }
        public string StoreProcedure { get; set; }
        public string Pagina { get; set; }
        public int IdEstado { get; set; }
        public int IdRefWordpressPaginasControl { get; set; }
        public int Orden { get; set; }
        public bool Opcional { get; set; }
        public int IdDependencia { get; set; }
        public string TablaValor { get; set; }
        public string Tabla { get; set; }
    }
}

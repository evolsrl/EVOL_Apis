using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    
    public class LavPuntosVentasDetalle 
    {
        public int IdPuntoVenta { get; set; }
        public int IdDia { get; set; }
        public string Dia { get; set; }
        public TimeSpan HoraDesde { get; set; }
        public TimeSpan HoraHasta { get; set; }
     
     
    }
}

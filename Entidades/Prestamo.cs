using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entidades
{
    public class Prestamo
    {
        public int IdPrestamoSimulacion { get; set; }
        public int IdPrestamoPlan { get; set; }
        public Decimal ImporteSolicitado { get; set; }
        public int CantidadCuotas { get; set; }
        public string IP { get; set; }
        public string ApellidoNombre { get; set; }
        public string CorreoElectronico { get; set; }
        public string Celular { get; set; }
        public string RangoHorario { get; set; }
        public string Observacion { get; set; }
    }
}

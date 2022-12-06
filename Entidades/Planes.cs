using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entidades
{
    public class Planes
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int CantidadCuotas { get; set; }
        public int CantidadCuotasHasta { get; set; }
        public decimal ImporteDesde { get; set; }
        public decimal ImporteHasta { get; set; }
    }
}

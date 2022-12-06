using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    [Serializable]
    public class LavPuntosVentas 
    {
        public int IdPuntoVenta { get; set; }
        public string Descripcion { get; set; }
        public string Contacto { get; set; }
        public string Direccion { get; set; }
        public int NumeroDireccion { get; set; }
        public string CodigoPostal { get; set; }
        public string Localidad { get; set; }
        public string Partido { get; set; }
        public string Provincia { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }


        List<LavPuntosVentasDetalle> _puntosVentasDetalles;

        public List<LavPuntosVentasDetalle> PuntosVentasDetalles
        {
            get { return _puntosVentasDetalles == null ? (_puntosVentasDetalles = new List<LavPuntosVentasDetalle>()) : _puntosVentasDetalles; }
            set { _puntosVentasDetalles = value; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class LavEdificios
    {
        
        public int IdEdificio { get; set; }
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
    }
}

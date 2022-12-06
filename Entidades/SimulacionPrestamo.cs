using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Entidades
{
    public class ParametrosTabla
    {
        public int IdTipoOperacion { get; set; }
        public int IdPrestamoPlan { get; set; }
        public int IdPrestamoPlanTasa { get; set; }
        public int CantidadCuotas { get; set; }
        public decimal ImporteSolicitado { get; set; }
    }
    public class PrestamoDetalleCuota
    {
        public int idPrestamoSimulacion { get; set; }
        public decimal CuotaNumero { get; set; }
        public DateTime CuotaFechaVencimiento { get; set; }
        public decimal ImporteCuota { get; set; }
        public decimal ImporteInteres { get; set; }
        public decimal ImporteAmortizacion { get; set; }
        public decimal ImporteSaldo { get; set; }
        public decimal importeCapitalSocial { get; set; }
        public decimal ImporteNetoAmortizacion { get; set; }
        public string Moneda { get; set; }
    }
}

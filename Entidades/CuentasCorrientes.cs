using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades
{
    public class CuentasCorrientes
    {
        public string IdCuentaCorriente { get; set; }
        public string IdAfiliado { get; set; }
        public string Periodo { get; set; }
        public string FechaMovimiento { get; set; }
        public string Concepto { get; set; }
        public string TipoMovimientoIdTipoMovimiento { get; set; }
        public string TipoMovimientoTipoMovimiento { get; set; }
        public string ImporteDebito { get; set; }
        public string ImporteCredito { get; set; }
        public string Importe { get; set; }
        public string ImporteCobrado { get; set; }
        public string ImporteEnviar { get; set; }
        public string SaldoActual { get; set; }
        public string TipoValorTipoValor { get; set; }
        public string EstadoIdEstado { get; set; }
        public string EstadoDescripcion { get; set; }
        public string FormaCobroFormaCobro { get; set; }
        public string TipoCargoConcepto { get; set; }
        public string MotivoRechazo { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string NumeroSocio { get; set; }
        public string CodigoZonaGrupo { get; set; }
        public string Categoria { get; set; }
        public string MonedaMoneda { get; set; }
        public string MonedaIdMoneda { get; set; }
    }
}

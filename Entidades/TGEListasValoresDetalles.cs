using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class TGEListasValoresDetalles
    {
        public int IdListaValorDetalle { get; set; }
        public string CodigoValor { get; set; }
        public string Descripcion { get; set; }
    }
}

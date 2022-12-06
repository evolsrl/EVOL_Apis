using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades
{
    public class CampoDinamico
    {
        public string IdCampo { get; set; }
        public string Nombre { get; set; }
        public string Mostrar { get; set; }
        public string StoreProcedure { get; set; }
        public string IdCamposTipo { get; set; }
        public string IdCampoValor { get; set; }
        public string Valor { get; set; }
        public Byte[] CampoValorSelloTiempo { get; set; }
    }
}

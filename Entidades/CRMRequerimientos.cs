using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class CRMRequerimientos
    {
        public int IdMaquina { get; set; }
        public int IdTipoReparacion { get; set; }
        public string Descripcion { get; set; }
        public int IdUsuario { get; set; }
        public int IdPuntoVenta { get; set; }
        public int IdTipoDenunciaPuntoVenta { get; set; }
        public int IdPaquete { get; set; }

        public int IdHorario { get; set; }

        public int IdEdificio { get; set; }
    }
}

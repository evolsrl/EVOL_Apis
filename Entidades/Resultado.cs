using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class Resultados
    {
        public int Resultado { get; set; }
        public bool Validar { get; set; }
        public string Mensaje { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades
{
    public class RegistroUsuario
    {
        public EAfiliados afiliado { get; set; }
        public FormasCobrosDinamico camposDinamicosFormasCobros { get; set; }
        public List<EAfiliados> familiares { get; set; }
        public string[] camposDinamicos { get; set; }

    }
}

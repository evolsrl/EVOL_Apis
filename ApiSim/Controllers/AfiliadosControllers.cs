
using Servicio.AccesoDatos;
using Afiliados.Entidades;
using Afiliados;
using System.Web.Mvc;
using System.Web.Http;

namespace ApiSim.Afiliados
{
    public class AfiliadosController : ApiController
    {
        public AfiAfiliados Get(int id)
        {
            AfiAfiliados af = new AfiAfiliados();
            af.IdAfiliado = id;

            //return BaseDatos.ObtenerBaseDatos().Obtener<AfiAfiliados>("",af);
            return AfiliadosF.AfiliadosObtenerDatos(af);
        }
    }

}
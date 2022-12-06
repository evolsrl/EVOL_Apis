using ApiSim.Core.Helpers;
using ApiSim.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Entidades;
using System.Linq;
using System.Data;

namespace ApiSim.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LavaYaController : ControllerBase
    {
        public IConfiguration _configuration { get; }
        private IUserService _userService;
        private readonly IDapper _dapper;
        private string DefaultConnection;
        public LavaYaController(IUserService userService, IDapper dapper, IConfiguration configuration)
        {
            _userService = userService;
            _dapper = dapper;
            _configuration = configuration;
            DefaultConnection = _configuration.GetConnectionString("DefaultConnection");
        }

        [Authorize]
        [HttpGet("conexion")]
        public int Conexion()
        {
            return 10;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        #region Puntos de Ventas
        [Authorize]
        [HttpGet("puntosventasobtener")]
        public async Task<List<LavPuntosVentas>> PuntosVentasObtener()
        {

            var dbparams = new DynamicParameters();

            var puntosVentas = await Task.FromResult(_dapper.GetAll<LavPuntosVentas>("[ApiLavPuntosVentasObtenerTodos]", dbparams, commandType: CommandType.StoredProcedure));

            var puntosVentasDetalle = await Task.FromResult(_dapper.GetAll<LavPuntosVentasDetalle>("[ApiLavPuntosVentasObtenerTodosDetalle]", dbparams, commandType: CommandType.StoredProcedure));

            if (puntosVentas.Count > 0 && puntosVentasDetalle.Count > 0)
            {
                foreach (LavPuntosVentas item in puntosVentas)
                {
                    item.PuntosVentasDetalles.AddRange(puntosVentasDetalle.Where(x => x.IdPuntoVenta == item.IdPuntoVenta).ToList());
                }
            }
            return puntosVentas;

        }

        [Authorize]
        [HttpGet("puntosventasobtenermascercanos/{latitud}/{longitud}")]
        public async Task<List<LavPuntosVentas>> PuntosVentasObtenerMasCercanos(decimal latitud, decimal longitud)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("Latitud", latitud, DbType.Decimal);
            dbparams.Add("Longitud", longitud, DbType.Decimal);
            var puntosVentas = await Task.FromResult(_dapper.GetAll<LavPuntosVentas>("[ApiLavPuntosVentasObtenerMasCercanos]", dbparams, commandType: CommandType.StoredProcedure));
            //var puntosVentasDetalle = await Task.FromResult(_dapper.GetAll<LavPuntosVentasDetalle>("[ApiLavPuntosVentasObtenerTodosDetalle]", dbparams, commandType: CommandType.StoredProcedure));

            //if (puntosVentas.Count > 0 && puntosVentasDetalle.Count > 0)
            //{
            //    foreach (LavPuntosVentas item in puntosVentas)
            //    {
            //        item.PuntosVentasDetalles.AddRange(puntosVentasDetalle.Where(x => x.IdPuntoVenta == item.IdPuntoVenta).ToList());
            //    }
            //}
            return puntosVentas;

        }
        #endregion

        #region Edificios
        [Authorize]
        [HttpGet("edificiosobtener")]
        public async Task<List<LavEdificios>> EdificiosObtener()
        {

            var dbparams = new DynamicParameters();

            var puntosVentas = await Task.FromResult(_dapper.GetAll<LavEdificios>("[ApiLavEdificiosObtenerTodos]", dbparams, commandType: CommandType.StoredProcedure));

            return puntosVentas;

        }
        [Authorize]
        [HttpGet("edificiosobtenerporid/{idEdificio}")]
        public async Task<LavEdificios> EdificiosObtenerPorId(int idEdificio)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdEdificio", idEdificio, DbType.Int32);
            var result = await Task.FromResult(_dapper.Get<LavEdificios>("[ApiLavEdificiosObtenerPorId]", dbparams, commandType: CommandType.StoredProcedure));


            return result;
        }
        [Authorize]
        [HttpGet("edificiosobtenerpordireccion/{direccion}")]
        public async Task<List<LavEdificios>> EdificiosObtenerPorDireccion(string direccion)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("Direccion", direccion, DbType.String);
            var result = await Task.FromResult(_dapper.GetAll<LavEdificios>("[ApiLavEdificiosObtenerPorDireccion]", dbparams, commandType: CommandType.StoredProcedure));
            return result;
        }

        [Authorize]
        [HttpGet("usuariovalidaredificio/{direccion},{numero}")]
        public async Task<Resultados> UsuarioValidarEdificio(string direccion, int numero)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("Direccion", direccion, DbType.String);
            dbparams.Add("Numero", numero, DbType.Int32);
            var result = await Task.FromResult(_dapper.Get< Resultados>("[ApiLavUsuarioValidarEdificio]", dbparams, commandType: CommandType.StoredProcedure));
            return result;
        }

        [Authorize]
        [HttpGet("maquinasobtenerporidedificio/{idEdificio}")]
        public async Task<List<LavMaquinas>> MaquinasObtenerPorIdEdificio(int idEdificio)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdEdificio", idEdificio, DbType.Int32);
            var result = await Task.FromResult(_dapper.GetAll<LavMaquinas>("[ApiLavMaquinasObtenerPorIdEdificio]", dbparams, commandType: CommandType.StoredProcedure));
            return result;
        }

        #endregion
                
        #region Usuarios
        [Authorize]
        [HttpGet("usuarioseleccionarporidusuario/{IdUsuario}")]
        public async Task<List<LavUsuario>> UsuarioSeleccionarPorIdUsuario(int IdUsuario)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdUsuario", IdUsuario, DbType.Int32);
            var result = await Task.FromResult(_dapper.GetAll<LavUsuario>("[ApiLavUsuariosSeleccionar]", dbparams, commandType: CommandType.StoredProcedure));
            return result;
        }

        [Authorize]
        [HttpPost("usuarioregistrar")]
        public async Task<LavUsuario> UsuarioRegistrar(LavUsuario usuario)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdEdificio", usuario.IdEdificio, DbType.Int32);
            dbparams.Add("Nombre", usuario.Nombre, DbType.String);
            dbparams.Add("Email", usuario.Email, DbType.String);
            dbparams.Add("Contrasenia", usuario.Contrasenia, DbType.String);
            dbparams.Add("Telefono", usuario.Telefono, DbType.String);
            dbparams.Add("Token", usuario.Token, DbType.String);
            dbparams.Add("Piso", usuario.Piso, DbType.String);
            dbparams.Add("Departamento", usuario.Departamento, DbType.String);
            var result = await Task.FromResult(_dapper.Get<LavUsuario>("[ApiLavUsuariosRegistrar]", dbparams, commandType: CommandType.StoredProcedure));

            return result;
        }

        [Authorize]
        [HttpPost("usuarioactualizar")]
        public async Task<bool> UsuarioActualizar(LavUsuario usuario)
        {

            var dbparams = new DynamicParameters();
            dbparams.Add("IdEdificio", usuario.IdEdificio, DbType.Int32);
            dbparams.Add("Nombre", usuario.Nombre, DbType.String);
            dbparams.Add("Email", usuario.Email, DbType.String);
            dbparams.Add("Contrasenia", usuario.Contrasenia, DbType.String);
            dbparams.Add("Telefono", usuario.Telefono, DbType.String);
            dbparams.Add("IdUsuario", usuario.IdUsuario, DbType.Int32);
            dbparams.Add("Piso", usuario.Piso, DbType.String);
            dbparams.Add("Departamento", usuario.Departamento, DbType.String);
            var result = await Task.FromResult(_dapper.Get<bool>("[ApiLavUsuariosActualizar]", dbparams, commandType: CommandType.StoredProcedure));

            return result;
        }

        #endregion

        [Authorize]
        [HttpGet("listasObtenerporcodigo/{codigoValor}")]
        public async Task<List<TGEListasValoresDetalles>> ListasObtenerPorCodigo(string codigoValor)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("CodigoValor", codigoValor, DbType.String);
            var result = await Task.FromResult(_dapper.GetAll<TGEListasValoresDetalles>("[ApiTGEListasValoresDetallesObtenerPorCodigoValor]", dbparams, commandType: CommandType.StoredProcedure));
            return result;
        }

        [Authorize]
        [HttpGet("novedadesobtenerultimas/")]
        public async Task<List<TGEListasValoresDetalles>> NovedadesObtenerUltimas()
        {
            var dbparams = new DynamicParameters();
            var result = await Task.FromResult(_dapper.GetAll<TGEListasValoresDetalles>("[ApiTGEListasValoresDetallesObtenerNovedades]", dbparams, commandType: CommandType.StoredProcedure));
            return result;
        }

        #region CRM
        [Authorize]
        [HttpPost("reparacionesagregar")]
        public async Task<int> ReparacionesAgregar(CRMRequerimientos data)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdMaquina", data.IdMaquina, DbType.Int32);
            dbparams.Add("IdTipoReparacion", data.IdTipoReparacion, DbType.Int32);
            dbparams.Add("Descripcion", data.Descripcion, DbType.String);
            dbparams.Add("IdUsuario", data.IdUsuario, DbType.Int32);

            var result = await Task.FromResult(_dapper.Get<int>("[ApiCRMRequerimientosInsertar]", dbparams, commandType: CommandType.StoredProcedure));

            return result;

        }

        [Authorize]
        [HttpPost("denunciaspuntosventasagregar")]
        public async Task<int> DenunciasPuntosVentasAgregar(CRMRequerimientos data)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdPuntoVenta", data.IdPuntoVenta, DbType.Int32);
            dbparams.Add("IdTipoDenunciaPuntoVenta", data.IdTipoReparacion, DbType.Int32);
            dbparams.Add("Descripcion", data.Descripcion, DbType.String);
            dbparams.Add("IdUsuario", data.IdUsuario, DbType.Int32);

            var result = await Task.FromResult(_dapper.Get<int>("[ApiCRMRequerimientosInsertarDenunciaPuntoVenta]", dbparams, commandType: CommandType.StoredProcedure));

            return result;

        }

        [Authorize]
        [HttpPost("ventasfichasagregar")]
        public async Task<int> VentasFichasAgregar(CRMRequerimientos data)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdEdificio", data.IdEdificio, DbType.Int32);
            dbparams.Add("IdPaquete", data.IdPaquete, DbType.Int32);
            dbparams.Add("IdHorario", data.IdHorario, DbType.Int32);
            dbparams.Add("Descripcion", data.Descripcion, DbType.String);
            dbparams.Add("IdUsuario", data.IdUsuario, DbType.Int32);

            var result = await Task.FromResult(_dapper.Get<int>("[ApiCRMRequerimientosInsertarVentasFichas]", dbparams, commandType: CommandType.StoredProcedure));

            return result;

        }
        #endregion
    }
}
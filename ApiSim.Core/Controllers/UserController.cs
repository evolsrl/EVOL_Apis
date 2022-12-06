using ApiSim.Core.Helpers;
using ApiSim.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WordPressPCL;
using WordPressPCL.Models;
using Entidades;
using System.Linq;
using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace ApiSim.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        public IConfiguration _configuration { get; }
        private IUserService _userService;
        private readonly IDapper _dapper;
        private string DefaultConnection;
        public ApiController(IUserService userService, IDapper dapper, IConfiguration configuration)
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

        [Authorize]
        [HttpPost("SubirKeys")]
        public async Task<bool> SubirKeysWooCommerce(WooCommerce datos)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("WordpressUrl", datos.url, DbType.String);
            dbparams.Add("KeyPublica", datos.keyPublica, DbType.String);
            dbparams.Add("KeyPrivada", datos.keyPrivada, DbType.String);

            var result = await Task.FromResult(_dapper.Get<bool>("[WordpressTGEParametrosValoresInsertar]", dbparams, commandType: CommandType.StoredProcedure));

            return result;
        }

        [Authorize]
        [HttpGet("existedocumento/{documento},{tipo}")]
        public async Task<bool> ExisteDocumento(int documento, int tipo)
        { 

            var dbparams = new DynamicParameters();
            dbparams.Add("IdTipoDocumento", tipo, DbType.Int32);
            dbparams.Add("NumeroDocumento", documento, DbType.Int32);
            var result = await Task.FromResult(_dapper.Get<bool>("[WordPressAfiAfiliadosValidacionesRegistroAsociados]", dbparams, commandType: CommandType.StoredProcedure));

            return !(result);
        }

        [Authorize]
        [HttpGet("getusuario/{documento},{tipo}")]
        public async Task<EAfiliados> GetUsuario(int documento, int tipo)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdTipoDocumento", tipo, DbType.Int32);
            dbparams.Add("NumeroDocumento", documento, DbType.Int32);

            EAfiliados retorno = new EAfiliados();

            retorno = await Task.FromResult(_dapper.Get<EAfiliados>("[WordPressAfiAfiliadosSeleccionarTipoNumeroDocumento]", dbparams, commandType: CommandType.StoredProcedure));

            return retorno;
        }

        [Authorize]
        [HttpGet("getfamiliares/{idAfiliado}")]
        public async Task<List<EAfiliados>> GetUsuario(int idAfiliado)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdAfiliado", idAfiliado, DbType.Int32);

            List<EAfiliados> listaFamiliares = new List<EAfiliados>();

            listaFamiliares = await Task.FromResult(_dapper.GetAll<EAfiliados>("[WordPressAfiAfiliadosSeleccionarParientes]", dbparams, commandType: CommandType.StoredProcedure));

            return listaFamiliares;
        }

        [Authorize]
        [HttpPost("registrarusuario")]
        public async Task<int> RegistrarUsuario(RegistroUsuario data)
        {
            bool flag = false;

            int contador = 0;
            var dbparams = new DynamicParameters();

            using (SqlConnection Connect = new SqlConnection(DefaultConnection))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("[WordpressAfiAfiliadosInsertar]", Connect);

                Connect.Open();

                SqlCommand command = new SqlCommand();
                command.CommandText = "[WordpressAfiAfiliadosInsertar]";
                command.Connection = Connect;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@IdAfiliadoRef", SqlDbType.Int).Value = data.afiliado.IdAfiliadoRef;
                command.Parameters.Add("@Nombre", SqlDbType.NVarChar).Value = data.afiliado.Nombre;
                command.Parameters.Add("@Apellido", SqlDbType.NVarChar).Value = data.afiliado.Apellido;
                command.Parameters.Add("@IdTipoDocumento", SqlDbType.Int).Value = data.afiliado.IdTipoDocumento;
                command.Parameters.Add("@NumeroDocumento", SqlDbType.Int).Value = data.afiliado.NumeroDocumento;
                if (data.afiliado.CUIL == null)
                {
                    command.Parameters.Add("@CUIL", SqlDbType.Int).Value = 0;
                }
                else
                {
                    command.Parameters.Add("@CUIL", SqlDbType.Int).Value = data.afiliado.CUIL;
                }
                command.Parameters.Add("@IdSexo", SqlDbType.Int).Value = data.afiliado.IdSexo;
                command.Parameters.Add("@FechaNacimiento", SqlDbType.DateTime).Value = data.afiliado.FechaNacimiento;
                command.Parameters.Add("@FechaIngreso", SqlDbType.DateTime).Value = data.afiliado.FechaIngreso;
                command.Parameters.Add("@IdEstadoCivil", SqlDbType.Int).Value = data.afiliado.IdEstadoCivil;
                command.Parameters.Add("@MatriculaIAF", SqlDbType.NVarChar).Value = data.afiliado.MatriculaIAF;
                command.Parameters.Add("@CorreoElectronico", SqlDbType.NVarChar).Value = data.afiliado.CorreoElectronico;
                command.Parameters.Add("@IdCategoria", SqlDbType.Int).Value = data.afiliado.IdCategoria;
                command.Parameters.Add("@IdGrupoSanguieno", SqlDbType.Int).Value = data.afiliado.IdGrupoSanguieno;
                command.Parameters.Add("@IdGrado", SqlDbType.Int).Value = data.afiliado.IdGrado;
                command.Parameters.Add("@FechaRetiro", SqlDbType.DateTime).Value = data.afiliado.FechaRetiro;
                command.Parameters.Add("@FechaFallecimiento", SqlDbType.DateTime).Value = data.afiliado.FechaFallecimiento;
                command.Parameters.Add("@FechaBaja", SqlDbType.DateTime).Value = data.afiliado.FechaBaja;
                command.Parameters.Add("@IdParentesco", SqlDbType.Int).Value = data.afiliado.IdParentesco;
                command.Parameters.Add("@FechaAlta", SqlDbType.DateTime).Value = data.afiliado.FechaAlta;
                command.Parameters.Add("@IdAfiliadoTipo", SqlDbType.Int).Value = data.afiliado.IdAfiliadoTipo;
                command.Parameters.Add("@IdTipoApoderado", SqlDbType.Int).Value = data.afiliado.IdTipoApoderado;
                command.Parameters.Add("@IdFilial", SqlDbType.Int).Value = data.afiliado.IdFilial;
                command.Parameters.Add("@CodigoCategoria", SqlDbType.Int).Value = data.afiliado.CodigoCategoria;
                command.Parameters.Add("@IdAfiliadoFallecido", SqlDbType.Int).Value = data.afiliado.IdAfiliadoFallecido;
                command.Parameters.Add("@CodigoZonaGrupo", SqlDbType.NVarChar).Value = data.afiliado.CodigoZonaGrupo;
                command.Parameters.Add("@FechaSupervivencia", SqlDbType.DateTime).Value = data.afiliado.FechaSupervivencia;
                command.Parameters.Add("@IdCondicionFiscal", SqlDbType.Int).Value = data.afiliado.IdCondicionFiscal;
                command.Parameters.Add("@IdSituacionRevista", SqlDbType.Int).Value = data.afiliado.IdSituacionRevista;
                command.Parameters.Add("@Detalle", SqlDbType.NVarChar).Value = data.afiliado.Detalle;
                command.Parameters.Add("@ComprobanteExento", SqlDbType.NVarChar).Value = data.afiliado.ComprobanteExento;
                command.Parameters.Add("@IdProcesoProcesamiento", SqlDbType.Int).Value = data.afiliado.IdProcesoProcesamiento;
                command.Parameters.Add("@IdNacionalidad", SqlDbType.Int).Value = data.afiliado.IdNacionalidad;
                command.Parameters.Add("@IdFormaCobro", SqlDbType.Int).Value = data.afiliado.IdFormaCobro;
                command.Parameters.Add("@IdProvincia", SqlDbType.Int).Value = data.afiliado.IdProvincia;
                command.Parameters.Add("@Calle", SqlDbType.NVarChar).Value = data.afiliado.Calle;
                command.Parameters.Add("@Numero", SqlDbType.Int).Value = data.afiliado.Numero;
                command.Parameters.Add("@Piso", SqlDbType.Int).Value = data.afiliado.Piso==string.Empty ? "0" : data.afiliado.Piso;
                command.Parameters.Add("@Departamento", SqlDbType.NVarChar).Value = data.afiliado.Departamento;
                command.Parameters.Add("@IdCodigoPostal", SqlDbType.Int).Value = data.afiliado.IdCodigoPostal;
                command.Parameters.Add("@CodigoPostal", SqlDbType.NVarChar).Value = data.afiliado.CodigoPostal;
                command.Parameters.Add("@Celular", SqlDbType.NVarChar).Value = data.afiliado.Celular;

                if (data.camposDinamicosFormasCobros.camposDinamicos.Length > 0)
                {
                    XDocument xmlFormasCobros = new XDocument();
                    XElement Campos = null;
                    XElement idCampo = null;
                    XElement valor = null;

                    flag = false;

                    foreach (string item in data.camposDinamicosFormasCobros.camposDinamicos)
                    {
                        if (contador % 2 == 0)
                        {
                            idCampo = new XElement("IdCampo", item);
                        }
                        else
                        {
                            valor = new XElement("Valor", item);
                        }

                        if (idCampo != null && valor != null)
                        {
                            if (!flag)
                            {
                                Campos = new XElement("Campos", new XElement("Campo", idCampo, valor));
                                flag = true;
                            }
                            else
                            {
                                Campos.Add(new XElement("Campo", idCampo, valor));
                            }

                            idCampo = null;
                            valor = null;
                        }

                        contador++;
                    }

                    xmlFormasCobros.Add(Campos);
                    command.Parameters.Add("@Campos", SqlDbType.Xml).Value = xmlFormasCobros.ToString();
                }
                else
                {
                    command.Parameters.Add("@Campos", SqlDbType.Xml).Value = null;
                }

                if (data.camposDinamicos.Length > 0)
                {
                    XDocument xmlCamposDinamicos = new XDocument();
                    XElement CamposDinamicos = null;
                    XElement idCampoDinamico = null;
                    XElement valorDinamico = null;

                    flag = false;

                    foreach (string item in data.camposDinamicos)
                    {
                        if (contador % 2 == 0)
                        {
                            idCampoDinamico = new XElement("IdCampo", item);
                        }
                        else
                        {
                            valorDinamico = new XElement("Valor", item);
                        }

                        if (idCampoDinamico != null && valorDinamico != null)
                        {
                            if (!flag)
                            {
                                CamposDinamicos = new XElement("Campos", new XElement("Campo", idCampoDinamico, valorDinamico));
                                flag = true;
                            }
                            else
                            {
                                CamposDinamicos.Add(new XElement("Campo", idCampoDinamico, valorDinamico));
                            }

                            idCampoDinamico = null;
                            valorDinamico = null;
                        }

                        contador++;
                    }

                    xmlCamposDinamicos.Add(CamposDinamicos);
                    command.Parameters.Add("@CamposDinamicos", SqlDbType.Xml).Value = xmlCamposDinamicos.ToString();
                }
                else
                {
                    command.Parameters.Add("@CamposDinamicos", SqlDbType.Xml).Value = null;
                }

                if (data.familiares.Count > 0)
                {
                    XDocument xmlFamiliares = new XDocument();
                    XElement Afiliados = null;
                    XElement Apellido = null;
                    XElement Nombre = null;
                    XElement IdTipoDocumento = null;
                    XElement NumeroDocumento = null;
                    XElement IdSexo = null;
                    XElement IdParentesco = null;

                    flag = false;

                    foreach (EAfiliados familiar in data.familiares)
                    {
                        Apellido = new XElement("Apellido", familiar.Apellido);
                        Nombre = new XElement("Nombre", familiar.Nombre);
                        IdTipoDocumento = new XElement("IdTipoDocumento", familiar.IdTipoDocumento);
                        NumeroDocumento = new XElement("NumeroDocumento", familiar.NumeroDocumento);
                        IdSexo = new XElement("IdSexo", familiar.IdSexo);
                        IdParentesco = new XElement("IdParentesco", familiar.IdParentesco);

                        if (Apellido != null && Nombre != null && IdTipoDocumento != null && NumeroDocumento != null && IdSexo != null && IdParentesco != null)
                        {
                            if (!flag)
                            {
                                Afiliados = new XElement("Afiliados", new XElement("Afiliado", Apellido, Nombre, IdTipoDocumento, NumeroDocumento, IdSexo, IdParentesco));
                                flag = true;
                            }
                            else
                            {
                                Afiliados.Add(new XElement("Afiliado", Apellido, Nombre, IdTipoDocumento, NumeroDocumento, IdSexo, IdParentesco));
                            }

                        }
                    }

                    xmlFamiliares.Add(Afiliados);
                    command.Parameters.Add("@Familiares", SqlDbType.Xml).Value = xmlFamiliares.ToString();
                }
                else
                {
                    command.Parameters.Add("@Familiares", SqlDbType.Xml).Value = null;
                }


                adapter.SelectCommand = command;

                DataSet ds = new DataSet();
                adapter.Fill(ds);
                int retorno = -1;

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        retorno = (int)ds.Tables[0].Rows[0]["IdAfiliado"];
                    }
                }

                return retorno;
            }

        }

        [Authorize]
        [HttpPost("insertarid")]
        public async Task<int> InsertarId(Ids data)
        {
            var dbparams = new DynamicParameters();

            dbparams.Add("IdAfiliado", data.IdAfiliado, DbType.Int32);
            dbparams.Add("IdWordPress", data.IdWordPress, DbType.Int32);

            var result = await Task.FromResult(_dapper.Insert<int>("[WordPressAfiAfiliadosWordPressInsertar]", dbparams, commandType: CommandType.StoredProcedure));
            return result;
        }

        [Authorize]
        [HttpGet("gettablas/{IdTipoOperacion},{IdPrestamoPlan},{IdPrestamoPlanTasa},{CantidadCuotas},{ImporteSolicitado}")]
        public async Task<List<PrestamoDetalleCuota>> GetTablas(int IdTipoOperacion, int IdPrestamoPlan, int IdPrestamoPlanTasa, int CantidadCuotas, int ImporteSolicitado)
        {
            using (SqlConnection Connect = new SqlConnection(DefaultConnection))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("[WordPressPrePrestamosArmarCuponera]", Connect);

                Connect.Open();

                SqlCommand command = new SqlCommand();
                command.CommandText = "[WordPressPrePrestamosArmarCuponera]";
                command.Connection = Connect;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@IdTipoOperacion", SqlDbType.Int).Value = IdTipoOperacion;
                command.Parameters.Add("@IdPrestamoPlan", SqlDbType.Int).Value = IdPrestamoPlan;
                command.Parameters.Add("@IdPrestamoPlanTasa", SqlDbType.Int).Value = IdPrestamoPlanTasa;
                command.Parameters.Add("@CantidadCuotas", SqlDbType.Int).Value = CantidadCuotas;
                command.Parameters.Add("@ImporteSolicitado", SqlDbType.Decimal).Value = ImporteSolicitado;

                adapter.SelectCommand = command;

                DataSet ds = new DataSet();
                adapter.Fill(ds);

                var dbparams = new DynamicParameters();

                dbparams.Add("IdPrestamoPlan", IdPrestamoPlan, DbType.Int32);
                dbparams.Add("ImporteSolicitado", ImporteSolicitado, DbType.Decimal);
                dbparams.Add("CantidadCuotas", CantidadCuotas, DbType.Int32);

                var idPrestamoSimulacion = await Task.FromResult(_dapper.Insert<int>("[WordPressPrestamosSimulacionesInsertar]", dbparams, commandType: CommandType.StoredProcedure));

                List<PrestamoDetalleCuota> listaPrestamoDetalleCuota = new List<PrestamoDetalleCuota>();

                if (ds.Tables.Count > 1)
                {
                    foreach (DataRow r in ds.Tables[1].Rows)
                    {
                        PrestamoDetalleCuota x = new PrestamoDetalleCuota();

                        x.idPrestamoSimulacion = idPrestamoSimulacion;
                        x.CuotaNumero = ds.Tables[1].Columns["CuotaNumero"].DataType== System.Type.GetType("System.Decimal") ? (decimal)r["CuotaNumero"] : (int)r["CuotaNumero"];
                        x.CuotaFechaVencimiento = (DateTime)r["CuotaFechaVencimiento"];
                        x.ImporteCuota = (decimal)r["ImporteCuota"];
                        x.ImporteInteres = (decimal)r["ImporteInteres"];
                        x.ImporteAmortizacion = (decimal)r["ImporteAmortizacion"];
                        x.ImporteSaldo = (decimal)r["ImporteSaldo"];
                        try
                        {
                            x.importeCapitalSocial = (decimal)r["importeCapitalSocial"];
                        }
                        catch
                        {
                            x.importeCapitalSocial = 0;
                        }
                        try
                        {
                            x.ImporteNetoAmortizacion = (decimal)r["ImporteNetoAmortizacion"];
                        }
                        catch
                        {
                            x.ImporteNetoAmortizacion = 0;
                        }
                        x.Moneda = (string)r["Moneda"];

                        listaPrestamoDetalleCuota.Add(x);
                    }
                }
                return listaPrestamoDetalleCuota;
            }
        }

        [Authorize]
        [HttpPost("actualizarprestamo")]
        public async Task ActualizarPrestamo(Prestamo data)
        {
            var dbparams = new DynamicParameters();

            dbparams.Add("IdPrestamoSimulacion", data.IdPrestamoSimulacion, DbType.String);
            dbparams.Add("IP", data.IP, DbType.String);
            dbparams.Add("ApellidoNombre", data.ApellidoNombre, DbType.String);
            dbparams.Add("CorreoElectronico", data.CorreoElectronico, DbType.String);
            dbparams.Add("Celular", data.Celular, DbType.String);
            dbparams.Add("RangoHorario", data.RangoHorario, DbType.String);
            dbparams.Add("Observacion", data.Observacion, DbType.String);

            var result = await Task.FromResult(_dapper.Update<Prestamo>("[WordPressPrestamosSimulacionesActualizar]", dbparams, commandType: CommandType.StoredProcedure));
        }

        [Authorize]
        [HttpGet("getcontrol/{pagina}")]
        public async Task<List<Control>> GetControl(string pagina)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("Pagina", pagina, DbType.String);

            var result = await Task.FromResult(_dapper.GetAll<Control>("[WordpressPaginasControlSeleccionar]", dbparams, commandType: CommandType.StoredProcedure));

            List<Control> listaOrdenada = result.OrderBy(x => x.Orden).ToList();

            return listaOrdenada;
        }

        [Authorize]
        [HttpGet("obtenercontroles/")]
        public async Task<List<Control>> ObtenerControles()
        {
            var dbparams = new DynamicParameters();

            var result = await Task.FromResult(_dapper.GetAll<Control>("[WordpressPaginasControlsSeleccionar]", dbparams, commandType: CommandType.StoredProcedure));

            List<Control> listaOrdenada = result.OrderBy(x => x.Orden).ToList();

            return listaOrdenada;
        }

        [Authorize]
        [HttpGet("select/{idControl}")]
        public async Task<List<Select>> Select(int idControl)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdWordpressPaginasControl", idControl, DbType.Int32);
            var storeProcedure = await Task.FromResult(_dapper.Get<string>("[WordpressPaginasControlSeleccionarStoreProcedure]", dbparams, commandType: CommandType.StoredProcedure));

            var dbparams2 = new DynamicParameters();
            var result = await Task.FromResult(_dapper.GetAll<Select>(storeProcedure, dbparams2, commandType: CommandType.StoredProcedure));

            return result;
        }

        [Authorize]
        [HttpGet("getplanes/")]
        public async Task<List<Planes>> GetPlanes()
        {
            var dbparams = new DynamicParameters();

            var result = await Task.FromResult(_dapper.GetAll<Planes>("[WordpressPrePrestamosPlanesSeleccionar]", dbparams, commandType: CommandType.StoredProcedure));

            return result;
        }

        [Authorize]
        [HttpGet("getcargos/{id}")]
        public async Task<List<CuentasCorrientes>> getCargos(int id)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdAfiliado", id, DbType.Int32);

            var result = await Task.FromResult(_dapper.GetAll<CuentasCorrientes>("[WordPressCarCuentasCorrientesSeleccionarPorAfiliadoDataTable]", dbparams, commandType: CommandType.StoredProcedure));

            return result;
        }

        [Authorize]
        [HttpPost("actualizarusuario")]
        public async Task<bool> ActualizarUsuario(Actualizar data)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdAfiliado", data.IdAfiliado, DbType.Int32);

            dbparams.Add("DatosAfiliado", data.DatosAfiliado, DbType.String);

            var result = await Task.FromResult(_dapper.Get<bool>("[WordpressAfiAfiliadosActualizar]", dbparams, commandType: CommandType.StoredProcedure));

            return result;
        }

        [Authorize]
        [HttpGet("getselectcontrolesdependientes/{idControlDependiente},{idControlDepende}")]
        public async Task<List<Select>> GetSelectControlesDependientes(int idControlDepende, int idControlDependiente)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdWordpressPaginasControl", idControlDependiente, DbType.Int32);
            var storeProcedure = await Task.FromResult(_dapper.Get<string>("[WordpressPaginasControlSeleccionarStoreProcedure]", dbparams, commandType: CommandType.StoredProcedure));

            dbparams = new DynamicParameters();
            dbparams.Add("Id", idControlDepende, DbType.Int32);

            var result = await Task.FromResult(_dapper.GetAll<Select>(storeProcedure, dbparams, commandType: CommandType.StoredProcedure));

            return result;
        }
        [Authorize]
        [HttpGet("getcamposdinamicos18/{tablavalor},{idreftablavalor},{tabla},{idreftabla}")]
        public async Task<List<CampoDinamico>> GetCamposDinamicos18(string tablaValor, int idRefTablaValor, string tabla, int idRefTabla)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("TablaValor", tablaValor, DbType.String);
            dbparams.Add("IdRefTablaValor", idRefTablaValor, DbType.Int32);
            dbparams.Add("Tabla", tabla, DbType.String);
            dbparams.Add("IdRefTabla", idRefTabla, DbType.Int32);

            var result = await Task.FromResult(_dapper.GetAll<CampoDinamico>("[WordpressTGECamposSeleccionarTablaValor]", dbparams, commandType: CommandType.StoredProcedure));

            return result;
        }

        [Authorize]
        [HttpGet("getcamposdinamicos21/{idControl}")]
        public async Task<List<CampoDinamico>> GetCamposDinamicos21(int idControl)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdWordpressPaginasControl", idControl, DbType.Int32);
            var storeProcedure = await Task.FromResult(_dapper.Get<string>("[WordpressPaginasControlSeleccionarStoreProcedure]", dbparams, commandType: CommandType.StoredProcedure));

            var dbparams2 = new DynamicParameters();
            var result = await Task.FromResult(_dapper.GetAll<CampoDinamico>(storeProcedure, dbparams2, commandType: CommandType.StoredProcedure));

            return result;
        }

        [Authorize]
        [HttpPost("getallcamposdinamicos18/")]
        public async Task<List<CampoDinamico>> GetAllCamposDinamicos18(ObtenerCampos18 data)
        {
            List<CampoDinamico> retorno = new List<CampoDinamico>();

            foreach (SeleccionarTablaValor tb in data.tablasValores)
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("TablaValor", tb.TablaValor, DbType.String);
                dbparams.Add("IdRefTablaValor", tb.IdRefTablaValor, DbType.Int32);
                dbparams.Add("Tabla", tb.Tabla, DbType.String);
                dbparams.Add("IdRefTabla", tb.IdRefTabla, DbType.Int32);

                retorno.AddRange(await Task.FromResult(_dapper.GetAll<CampoDinamico>("[WordpressTGECamposSeleccionarTablaValor]", dbparams, commandType: CommandType.StoredProcedure)));
            }
            return retorno;
        }

        [Authorize]
        [HttpGet("getallcamposdinamicos21/{pagina}")]
        public async Task<List<CampoDinamico>> GetAllCamposDinamicos21(string pagina)
        {
            List<CampoDinamico> retorno = new List<CampoDinamico>();

            var dbparams = new DynamicParameters();
            dbparams.Add("Pagina", pagina, DbType.String);
            dbparams.Add("IdCampoTipo", 21, DbType.Int32);
            var controles21 = await Task.FromResult(_dapper.GetAll<Control>("[WordpressPaginasControlSeleccionarControles]", dbparams, commandType: CommandType.StoredProcedure));

            var dbparams2 = new DynamicParameters();
            foreach (Control control21 in controles21)
            {
                retorno.AddRange(await Task.FromResult(_dapper.GetAll<CampoDinamico>(control21.StoreProcedure, dbparams2, commandType: CommandType.StoredProcedure)));
            }

            return retorno;
        }

        [Authorize]
        [HttpGet("getinputs/{store}")]
        public async Task<List<SelectFormasCobros>> GetInputs(string store)
        {
            var dbparams = new DynamicParameters();

            var result = await Task.FromResult(_dapper.GetAll<SelectFormasCobros>(store, dbparams, commandType: CommandType.StoredProcedure));

            return result;
        }

        [Authorize]
        [HttpGet("gettipos/{store}")]
        public async Task<List<Select>> GetTipos(string store)
        {
            var dbparams = new DynamicParameters();

            var result = await Task.FromResult(_dapper.GetAll<Select>(store, dbparams, commandType: CommandType.StoredProcedure));

            return result;
        }

        [Authorize]
        [HttpGet("usuarioautorizado/{mail}")]
        public async Task<bool> UsuarioAutorizado(string mail)
        {
            bool retorno = false;
            var dbparams = new DynamicParameters();

            var mails = await Task.FromResult(_dapper.GetAll<string>("[WordpressSegUsuariosSeleccionar]", dbparams, commandType: CommandType.StoredProcedure));

            foreach (string mailTabla in mails)
            {
                if (mailTabla == mail)
                {
                    retorno = true;
                    break;
                }
            }

            return retorno;
        }

        [Authorize]
        [HttpGet("existemail/{mail}")]
        public async Task<bool> ExisteMail(string mail)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("CorreoElectronico", mail, DbType.String);

            return await Task.FromResult(_dapper.Get<bool>("[WordpressAfiAfiliadosExisteCorreoElectronico]", dbparams, commandType: CommandType.StoredProcedure));
        }

        [Authorize]
        [HttpGet("validarmailactualizardatos/{idAfiliado},{mail}")]
        public async Task<bool> ValidarMailActualizarDatos(int idAfiliado, string mail)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdAfiliado", idAfiliado, DbType.Int32);
            dbparams.Add("CorreoElectronico", mail, DbType.String);

            return await Task.FromResult(_dapper.Get<bool>("[WordpressAfiAfiliadosValidarCorreoElectronico]", dbparams, commandType: CommandType.StoredProcedure));
        }

        [Authorize]
        [HttpGet("cantidaddiasplazos/{idmoneda}")]
        public async Task<List<AhoPlazos>> CantidadDiasPlazos(int idMoneda)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdMoneda", idMoneda, DbType.Int32);

            var resultado = await Task.FromResult(_dapper.GetAll<AhoPlazos>("[WordpressAhoPlazosSeleccionar]", dbparams, commandType: CommandType.StoredProcedure));
            return resultado;
        }

        [Authorize]
        [HttpPost("subirplazofijosimulacion/")]
        public async Task<bool> SubirPlazoFijoSimulacion(SimulacionPlazosFijos data)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdMoneda", data.IdMoneda, DbType.Int32);
            dbparams.Add("PlazoDias", data.PlazoDias, DbType.Int32);
            dbparams.Add("TasaInteres", data.TasaInteres, DbType.Decimal);
            dbparams.Add("ImporteCapital", data.ImporteCapital, DbType.Decimal);
            dbparams.Add("IP", data.IP, DbType.String);
            dbparams.Add("ApellidoNombre", data.ApellidoNombre, DbType.String);
            dbparams.Add("CorreoElectronico", data.CorreoElectronico, DbType.String);
            dbparams.Add("Celular", data.Celular, DbType.String);
            dbparams.Add("RangoHorario", data.RangoHorario, DbType.String);
            dbparams.Add("Observacion", data.Observacion, DbType.String);

            return await Task.FromResult(_dapper.Get<bool>("[WordPressPlazosFijosSimulacionesInsertar]", dbparams, commandType: CommandType.StoredProcedure));
        }


        [Authorize]
        [HttpPost("subirestado")]
        public async Task<bool> SubirEstado(EAfiliados data)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdAfiliado", data.IdAfiliado, DbType.Int32);
            dbparams.Add("IdEstado", data.IdEstado, DbType.Int32);

            return await Task.FromResult(_dapper.Get<bool>("[WordPressAfiAfiliadosActualizarIdEstado]", dbparams, commandType: CommandType.StoredProcedure));
        }

        [Authorize]
        [HttpGet("getcuotasocial/{idAfiliado},{codigoCargo}")]
        public async Task<int> GetCuotaSocial(int idAfiliado, int codigoCargo)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdAfiliado", idAfiliado, DbType.Int32);
            dbparams.Add("CodigoCargo", codigoCargo, DbType.Int32);

            var resultado = await Task.FromResult(_dapper.Get<int>("[WordpressCarTipoCargosCategoriasCuotaSocialSeleccionar]", dbparams, commandType: CommandType.StoredProcedure));
            return resultado;
        }

        [Authorize]
        [HttpPost("subircargocuotasocial")]
        public async Task<bool> SubirCargoCuotaSocial(EAfiliados data)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("IdAfiliado", data.IdAfiliado, DbType.Int32);
            dbparams.Add("CodigoCargo", data.CodigoCargo, DbType.Int32);

            return await Task.FromResult(_dapper.Get<bool>("[WordpressCarProcesosInsertarCargoCuotaSocial]", dbparams, commandType: CommandType.StoredProcedure));
        }


        [Authorize]
        [HttpGet("getselect/{stored}")]
        public async Task<List<Select>> Select(string stored)
        {
            var dbparams = new DynamicParameters();
            var result = await Task.FromResult(_dapper.GetAll<Select>(stored, dbparams, commandType: CommandType.StoredProcedure));

            return result;
        }

        [Authorize]
        [HttpPost("actualizarparientes/")]
        public async Task<bool> ActualizarParientes(List<EAfiliados> parientes)
        {
            var dbparams = new DynamicParameters();
            bool result = true;

            foreach(EAfiliados pariente in parientes)
            {
                dbparams = new DynamicParameters();

                dbparams.Add("IdTipoDocumento", pariente.IdTipoDocumento, DbType.Int32);
                dbparams.Add("NumeroDocumento", pariente.NumeroDocumento, DbType.Int32);
                dbparams.Add("Apellido", pariente.Apellido, DbType.String);
                dbparams.Add("Nombre", pariente.Nombre, DbType.String);
                dbparams.Add("IdSexo", pariente.IdSexo, DbType.Int32);
                dbparams.Add("IdParentesco", pariente.IdParentesco, DbType.Int32);

                if (pariente.IdAfiliado == "0")
                {
                    dbparams.Add("IdAfiliadoRef", pariente.IdAfiliadoRef, DbType.Int32);

                    result = await Task.FromResult(_dapper.Get<bool>("WordpressAfiAfiliadosInsertarPariente", dbparams, commandType: CommandType.StoredProcedure));
                }
                else if(pariente.IdAfiliado != "-1")
                {
                    if (pariente.IdTipoDocumento == "-1" && pariente.NumeroDocumento == "-1" && pariente.Nombre == "-1" && pariente.Apellido == "-1" && pariente.IdSexo == "-1" && pariente.IdParentesco == "-1")
                    {
                        dbparams = new DynamicParameters();
                        dbparams.Add("IdAfiliado", pariente.IdAfiliado, DbType.Int32);

                        result = await Task.FromResult(_dapper.Get<bool>("WordpressAfiAfiliadosEliminarParientes", dbparams, commandType: CommandType.StoredProcedure));
                    }
                    else
                    {
                        dbparams.Add("IdAfiliado", pariente.IdAfiliado, DbType.Int32);

                        result = await Task.FromResult(_dapper.Get<bool>("WordpressAfiAfiliadosActualizarParientes", dbparams, commandType: CommandType.StoredProcedure));
                    }
                }
            }


            return true;
        }
    }
}

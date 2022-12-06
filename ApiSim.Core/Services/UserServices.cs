using System;
using System.Collections.Generic;
using System.Linq;
using ApiSim.Core.Helpers;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Entidades;
using Dapper;
using System.Data;

namespace ApiSim.Core.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>();
        private readonly AppSettings _appSettings;
        private ParametrosValores Username;
        private ParametrosValores Password;
        private readonly IDapper _dapper;

        public UserService(IOptions<AppSettings> appSettings , IDapper dapper)
        {
            _dapper = dapper;
            var dbparams = new DynamicParameters();
            dbparams.Add("IdParemetro", 147, DbType.Int32);
            Username = _dapper.Get<ParametrosValores>("[WordpressTGEParametrosValoresSeleccionarValorActual]", dbparams, commandType: CommandType.StoredProcedure);
            var dbparams2 = new DynamicParameters();
            dbparams2.Add("IdParemetro", 148, DbType.Int32);
            Password = _dapper.Get<ParametrosValores>("[WordpressTGEParametrosValoresSeleccionarValorActual]", dbparams2, commandType: CommandType.StoredProcedure);

            User usuario = new User();
            usuario.Username = Username.ParametroValor;
            usuario.Password = Password.ParametroValor;

            _appSettings = appSettings.Value;
            _users.Add(usuario);
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        // helper methods

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}


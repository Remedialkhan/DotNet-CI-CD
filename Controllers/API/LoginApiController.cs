using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Security.Claims;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using T_Systems.model;
using T_Systems.data;
using System.Web.Http.Cors;
using Azure.Core;
using Azure;

namespace T_Systems.Controllers.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("api/Login")]
    [ApiController]
    public class LoginApiController : ControllerBase
    {
        Login login = new Login();
        UsersManager usersManager = new UsersManager();
        UserCustom userCustom = new UserCustom();

        [HttpPost("login")]
        public async Task<Object> Login(LoginData data)
        {
            try
            {
                int rol = 2;
                ResponseLoginData? res = login.GetToken(data, rol);


                Users Usuarios = new Users();
                var stream = res.id_token;
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = jsonToken as JwtSecurityToken;
                Usuarios.Id = tokenS.Claims.Where(x => x.Type == "oid").Select(x => x.Value).FirstOrDefault();
                Usuarios.FullName = tokenS.Claims.Where(x => x.Type == "name").Select(x => x.Value).FirstOrDefault();
                Usuarios.Email = tokenS.Claims.Where(x => x.Type == "preferred_username").Select(x => x.Value).FirstOrDefault();
                Usuarios.RoleId = 2;
                //Usuarios.Surname = tokenS.Claims.Where(x => x.Type == "name").Select(x => x.Value).FirstOrDefault();
                //Usuarios.GivenName = tokenS.Claims.Where(x => x.Type == "name").Select(x => x.Value).FirstOrDefault();
                userCustom = usersManager.CreateUser(Usuarios);
                userCustom.Users.Token = await login.GetToken2(data);
                if (userCustom.Users.RoleId == 1)
                {
                    if (userCustom.Users.Id != "4b779913-abf3-43e1-a49a-3efb9d6ff8a6")
                    {
                        ResponseLoginData? res2 = login.GetToken(data, 1);
                        res.access_token = res2.access_token;
                    }
                    userCustom.Users.TokenAzure = res.access_token;
                }
                //var prueba = await login.getServiceClient(res.access_token);
                return new
                {
                    response = "Ok",
                    data = userCustom
                };
            }
            catch (Exception e)
            {

                ResponseErrorToken? res = JsonSerializer.Deserialize<ResponseErrorToken>(e.Message);
                if (res.error_description == null || res == null)
                {
                    return new
                    {
                        response = "Error",
                        error_description = e.Message
                    };
                }
                else
                {
                    return new
                    {
                        response = "Error",
                        error = res.error,
                        error_description = res.error_description
                    };
                }

            }

        }
        [HttpGet("test")]
        public async Task<Object> Test()
        {
            try
            {
                return new
                {
                    response = "Ok",
                    data = "Test Complete"
                };
            }
            catch (Exception e)
            {

                return new
                {
                    response = "Error",
                    data = e.Message
                };
            }
        }

    }
}

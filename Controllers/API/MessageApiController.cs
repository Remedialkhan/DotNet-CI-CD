using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.PowerBI.Api.Models;
using T_Systems.data;
using T_Systems.model;
using Message = T_Systems.model.Message;

namespace T_Systems.Controllers.API
{
    [Route("api/Messages")]
    [ApiController]
    [Authorize]

    public class MessageApiController : ControllerBase
    {
        MessageManager mnsj = new();
        UsersManager user = new();

        [HttpPost("Create")]
        public async Task<Object> createMessage(Message mensaje)
        {
            try
            {
                ResponseMessage response = mnsj.CreateMessage(mensaje);
                return new
                {
                    response = response.MessageText,
                    data = response
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
        [HttpGet("getMessages")]
        public async Task<Object> getMessages([FromHeader] String Authorization, String? Filter)
        {
            try
            {
                int role = user.getRoleForToken(Authorization);
                if (role == 1)
                {
                    List<getMessages> messages = mnsj.getMessages(Filter);
                    return new
                    {
                        response = "Ok",
                        data = messages
                    };
                }
                else
                {
                    return new
                    {
                        response = "Error 401",
                        data = "Unauthorize"
                    };
                }

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

        [HttpGet("getMessagesUser")]
        public async Task<Object> getMessages([FromHeader] String Authorization, String id, String? Filter)
        {
            try
            {
                List<getMessages> messages = mnsj.getMessages(id, Filter);
                return new
                {
                    response = "Ok",
                    data = messages
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
        [HttpPut("changeStatus")]
        public async Task<Object> changeStatus(int id)
        {
            try
            {
                bool returnData = mnsj.changeStatus(id);
                return new
                {
                    response = returnData ? "Ok" : "Error",
                    data = returnData
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
        [HttpGet("getListUsers")]
        public async Task<Object> getListUsers([FromHeader] String Authorization, String? name)
        {
            try
            {
                int role = user.getRoleForToken(Authorization);
                String? id = user.getIdForToken(Authorization);

                List<UserListString> responseData = mnsj.getListUsers(role, id, name);

                return new
                {
                    response = "Ok",
                    data = responseData
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

        //[HttpPost("ChangeAdminState")]
        //public async Task<Object> ChangeAdminState(Message mensaje)
        //{
        //    try
        //    {
        //        ResponseMessage response = mnsj.ChangeAdminState(mensaje);
        //        return new
        //        {
        //            response = response.MessageText,
        //            data = response
        //        };
        //    }
        //    catch (Exception e)
        //    {

        //        return new
        //        {
        //            response = "Error",
        //            data = e.Message
        //        };
        //    }
        //}

        //[HttpPost("ChangeUserState")]
        //public async Task<Object> ChangeUserState(Message mensaje)
        //{
        //    try
        //    {
        //        ResponseMessage response = mnsj.ChangeUserState(mensaje);
        //        return new
        //        {
        //            response = response.MessageText,
        //            data = response
        //        };
        //    }
        //    catch (Exception e)
        //    {

        //        return new
        //        {
        //            response = "Error",
        //            data = e.Message
        //        };
        //    }
        //}
    }
}

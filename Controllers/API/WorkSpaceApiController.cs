using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using T_Systems.data;
using T_Systems.model;

namespace T_Systems.Controllers.API
{
    [Route("api/workspace")]
    [ApiController]
    [Authorize]
    public class WorkSpaceApiController : ControllerBase
    {
        private WorkSpaceManager work = new WorkSpaceManager();
        private UsersManager users = new UsersManager();

        [HttpGet("getWorkSpaces")]
        public async Task<Object> getWorkSpaces([FromHeader] String Authorization)
        {
            try
            {
                int role = users.getRoleForToken(Authorization);
                if (role == 1)
                {
                    List<WorkSpaceCustom> returnData = await work.GetWorkSpaces();
                    return new
                    {
                        respone = "Ok",
                        data = returnData
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

        [HttpPost("createWorkSpace")]
        public async Task<Object> createWorkSpace(WorkSpaceCustom workSpace, [FromHeader] String Authorization)
        {
            try
            {
                int role = users.getRoleForToken(Authorization);
                if (role == 1)
                {
                    bool returnData = await work.createWorkSpace(workSpace);
                    return new
                    {
                        response = "Ok",
                        data = returnData
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

        [HttpPut("editWorkSpaces")]
        public async Task<Object> editWorkSpaces([FromHeader] String Authorization, WorkSpaceCustom workSpaceCustom)
        {
            try
            {
                int role = users.getRoleForToken(Authorization);
                if (role == 1)
                {
                    WorkSpaceCustom? response = await work.editWorkSpaces(workSpaceCustom);
                    return new
                    {
                        response = "Ok",
                        data = response
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
        [HttpDelete("deleteWorkSpaces")]
        public async Task<Object> deleteWorkSpaces([FromHeader] String Authorization, String id)
        {
            try
            {
                int role = users.getRoleForToken(Authorization);
                if (role == 1)
                {
                    bool responses = await work.deleteWorkSpaces(id);
                    String dataReturn = responses ? "Ok" : "Error";
                    return new
                    {
                        response = dataReturn,
                        data = responses
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
    }
}

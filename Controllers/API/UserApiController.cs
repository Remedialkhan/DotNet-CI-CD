using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using System.Net;
using T_Systems.data;
using T_Systems.model;

namespace T_Systems.Controllers.API
{
    [Route("api/Users")]
    [ApiController]
    [Authorize]
    public class UserApiController : ControllerBase
    {
        UsersManager user = new();
        [HttpPost("createUser")]
        public async Task<Object> createUser([FromHeader] String Authorization, CreateUser createUser)
        {
            try
            {
                int role = user.getRoleForToken(Authorization);
                if (role == 1)
                {
                    bool response = await user.createUserAzure(createUser);
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
        [HttpDelete("deleteUser")]
        public async Task<Object> deleteUser([FromHeader] String Authorization, String token, String id)
        {
            try
            {
                int role = user.getRoleForToken(Authorization);
                if (role == 1)
                {
                    bool responseData = await user.deleteUserAzure(token, id);
                    return new
                    {
                        response = (responseData) ? "Ok" : "Error",
                        data = (responseData) ? "true" : "",
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
        [HttpPut("editUserWorkSpace")]
        public async Task<Object> editUserWorkSpace([FromHeader] String Authorization, UsersWorkSpace usersWorkSpace)
        {
            try
            {
                int role = user.getRoleForToken(Authorization);
                if (role == 1)
                {
                    bool responseData = await user.editUserWorkSpace(usersWorkSpace);
                    if (responseData)
                    {
                        return new
                        {
                            response = "Ok",
                            data = responseData
                        };
                    }
                    else
                    {
                        return new
                        {
                            response = "Error",
                            data = responseData
                        };
                    }
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
        [HttpPut("deleteUserWorkSpace")]
        public async Task<Object> deleteUserWorkSpace([FromHeader] String Authorization, int id)
        {
            try
            {
                int role = user.getRoleForToken(Authorization);
                if (role == 1)
                {
                    bool responseData = await user.deleteUserWorkSpace(id);
                    if (responseData)
                    {
                        return new
                        {
                            response = "Ok",
                            data = responseData
                        };
                    }
                    else
                    {
                        return new
                        {
                            response = "Error",
                            data = responseData
                        };
                    }
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
        [HttpGet("getUsers")]
        public async Task<Object> getUsers([FromHeader] String Authorization, String? id)
        {
            try
            {
                int role = user.getRoleForToken(Authorization);
                if (role == 1)
                {
                    List<UsersList> returnData = await user.getUsers(id);
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
        [HttpPut("updateUser")]
        public async Task<Object> updateUser([FromHeader] String Authorization, UsersList users, String Token)
        {
            try
            {
                int role = user.getRoleForToken(Authorization);
                if (role == 1)
                {
                    bool responseData = await user.updateUser(users, Token);
                    if (responseData)
                    {
                        return new
                        {
                            response = "Ok",
                            data = responseData
                        };
                    }
                    else
                    {
                        return new
                        {
                            response = "Error",
                            data = responseData
                        };
                    }
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

        //[HttpPut("editUserRole")]
        //public Object editUserRole([FromHeader] String Authorization, Users users)
        //{
        //    try
        //    {
        //        int role = user.getRoleForToken(Authorization);
        //        if (role == 1)
        //        {
        //            bool returnData = user.editUserRole(users);
        //            return new
        //            {
        //                response = "Ok",
        //                data = returnData
        //            };
        //        }
        //        else
        //        {
        //            return new
        //            {
        //                response = "Error 401",
        //                data = "Unauthorize"
        //            };
        //        }
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
        //[HttpPost("createUserWorkSpace")]
        //public async Task<Object> createUserWorkSpace([FromHeader] String Authorization, UsersWorkSpace usersWorkSpace)
        //{
        //    try
        //    {
        //        int role = user.getRoleForToken(Authorization);
        //        if (role == 1)
        //        {
        //            String? response = await user.createUserWorkSpace(usersWorkSpace);
        //            if (response != null)
        //            {
        //                return new
        //                {
        //                    response = "Ok",
        //                    data = response
        //                };
        //            }
        //            else
        //            {
        //                return new
        //                {
        //                    response = "Error",
        //                    data = response
        //                };
        //            }
        //        }
        //        else
        //        {
        //            return new
        //            {
        //                response = "Error 401",
        //                data = "Unauthorize"
        //            };
        //        }
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

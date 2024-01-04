using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using NuGet.Common;
using System.Composition;
using System.Web.Http.Cors;
using T_Systems.data;
using T_Systems.model;
using ReportsModel = T_Systems.model.Reports;
namespace T_Systems.Controllers.API
{
    [Route("api/Reports")]
    [ApiController]
    [Authorize]
    public class ReportsApiController : ControllerBase
    {
        private ReportsManager reports = new ReportsManager();
        private UsersManager users = new UsersManager();
        private AzureAD azureAD = new AzureAD();

        [HttpGet("getReportsUser")]
        public async Task<Object> getReportsUser(String id, String? token, String? date)
        {
            try
            {
                String horaCookie = DateTime.Now.AddHours(-1).ToShortTimeString();
                if (date != null || date != "")
                {
                    horaCookie = date;
                }
                String tiempo = DateTime.Now.ToShortTimeString();
                DateTime fecha1 = Convert.ToDateTime(tiempo);
                DateTime fecha2 = Convert.ToDateTime(horaCookie);
                double minuts = fecha1.Subtract(fecha2).TotalMinutes;

                if (token == null || token == "" || minuts > 59)
                {
                    token = azureAD.GetAccessToken();
                }
                List<ReportsToken> returnData = await reports.getReportsUser(id, token);
                return new
                {
                    Response = "Ok",
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
        [HttpPut("editReport")]
        public async Task<Object> editReport([FromHeader] String Authorization, ReportsModel report)
        {
            try
            {
                int role = users.getRoleForToken(Authorization);
                if (role == 1)
                {
                    List<ListReports> returnData = reports.editReport(report);
                    return new
                    {
                        Response = "Ok",
                        data = returnData
                    };
                }
                else
                {
                    return new
                    {
                        respone = "Error 401",
                        data = "Unauthorize"
                    };
                }
            }
            catch (Exception e)
            {

                return new
                {
                    respone = "Error",
                    data = e.Message
                };
            }


        }
        [HttpPut("activeReport")]
        public async Task<Object> activeReport([FromHeader] String Authorization, String id, bool data)
        {
            try
            {
                int role = users.getRoleForToken(Authorization);
                if (role == 1)
                {
                    List<ListReports> returnData = await reports.activeReport(id, data);
                    return new
                    {
                        Response = "Ok",
                        data = returnData
                    };
                }
                else
                {
                    return new
                    {
                        respone = "Error 401",
                        data = "Unauthorize"
                    };
                }
            }
            catch (Exception e)
            {

                return new
                {
                    respone = "Error",
                    data = e.Message
                };
            }
        }
        [HttpGet("getDeletedReports")]
        public Object getDeletedReports([FromHeader] String Authorization)
        {
            try
            {
                int role = users.getRoleForToken(Authorization);
                if (role == 1)
                {
                    List<ReportsModel> returnData = reports.getDeletedReports();
                    return new
                    {
                        Response = "Ok",
                        data = returnData
                    };
                }
                else
                {
                    return new
                    {
                        respone = "Error 401",
                        data = "Unauthorize"
                    };
                }
            }
            catch (Exception e)
            {

                return new
                {
                    respone = "Error",
                    data = e.Message
                };
            }
        }
        [HttpGet("getNameReport")]
        public async Task<Object> getNameReport([FromHeader] String Authorization, String? name, bool? data)
        {
            try
            {
                int role = users.getRoleForToken(Authorization);
                if (role == 1)
                {
                    List<ListReports> responseData = reports.getNameReport(name, data);
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
                        respone = "Error 401",
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
        [HttpGet("downloadReport")]
        public async Task<Object> downloadReport(String id, String token, String type, String? idDownload)
        {

            try
            {
                DownloadReport responseData = reports.downloadReport(id, token, type, idDownload);
                return new
                {
                    response = "Ok",
                    data = responseData
                };
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}

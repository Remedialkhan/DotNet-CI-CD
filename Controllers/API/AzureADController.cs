using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.PowerBI.Api.Models;
using System.Web.Http.Cors;
using T_Systems.data;
using T_Systems.model;
using ReportsModel = T_Systems.model.Reports;

namespace T_Systems.Controllers.API
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Route("api/AzureAD")]
    [ApiController]
    [Authorize]
    public class AzureADController : ControllerBase
    {
        private AzureAD azureAD = new AzureAD();
        private UsersManager user = new();

        //[HttpGet("getReport")]
        //public async Task<Object> getReport(String? token)
        //{
        //    try
        //    {
        //        Guid workspaceId = new Guid("7da40916-1807-4d9b-bfb0-801b4fae53b3");
        //        Guid reportId = new Guid("c4e90d75-c4d1-440b-ba52-bf53df3cab3f");

        //        EmbeddedReportViewModel viewModel = await azureAD.getReport(workspaceId, reportId, token);
        //        return new
        //        {
        //            response = "Ok",
        //            data = viewModel,
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
        [HttpGet("getReports")]
        public async Task<Object> getReports([FromHeader] String Authorization, String? token, String? date)
        {
            try
            {
                int role = user.getRoleForToken(Authorization);
                if (role == 1)
                {
                    Guid workspaceId = new Guid("7da40916-1807-4d9b-bfb0-801b4fae53b3");
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
                    //String? token = azureAD.GetAccessToken();
                    List<ListWorkSpace> workspaces = await azureAD.getWorkSpaces(token);
                    List<AllWorkSpaces> viewModel = await azureAD.getReports(workspaces, token);
                    return new
                    {
                        response = "Ok",
                        data = viewModel,
                    };
                }
                else
                {
                    return new
                    {
                        response = "Error 401",
                        data = "Unauthorize",
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

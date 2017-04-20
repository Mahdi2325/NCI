using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Report.Excel;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Threading.Tasks;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Interface.NCIA;
using KMHC.SLTC.Business.Entity.Filter.NCIA;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Entity.Filter;
using Newtonsoft.Json;

namespace KMHC.SLTC.WebAPI.Report
{
    [RoutePrefix("api/PersonStatusReport")]
    public class PersonStatusReportController : BaseController
    {
        IPersonStatusReportService service = IOCContainer.Instance.Resolve<IPersonStatusReportService>();
        [Route(""), HttpGet]
        public async Task<object> Get(DateTime? startDate, DateTime? endDate)
        {
            var http = HttpClientHelper.NciHttpClient;
            var response = new BaseResponse<PersonStatusReportModel>();
            response = service.QueryPersonStatusInfo(startDate, endDate.Value.AddDays(1).AddSeconds(-1));
            //长照平台参数
            var request = new PersonStatusFilter();
            request.startDate = startDate;
            request.endDate = endDate.Value.AddDays(1).AddSeconds(-1);
            try
            {
                var result = await http.PostAsJsonAsync("/api/PersonStatusReport", request);
                var resultContent = await result.Content.ReadAsStringAsync();
                var ltcInfo = JsonConvert.DeserializeObject<PersonStatusReportModel>(resultContent);
                response.Data.DrugEntryNum = ltcInfo.DrugEntryNum;
                response.Data.NSCPLNum = ltcInfo.NSCPLNum;
                response.Data.BillV2Num = ltcInfo.BillV2Num;
                response.Data.CostNum = ltcInfo.CostNum;
                response.Data.InHospNum = ltcInfo.InHospNum;
                response.Data.OutHospOfOtherNum = ltcInfo.OutHospOfOtherNum;
                response.Data.OutHospOfDeadNum = ltcInfo.OutHospOfDeadNum;
                response.Data.LeaveHospNum = ltcInfo.LeaveHospNum;
            }
            catch (Exception ex)
            {
                response.ResultCode = -1;
                response.ResultMessage = "长照平台无法连接，请联系管理员！";
            }
            return response;
        }
    }
}

using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter.NCIA;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface.NCIA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using KMHC.SLTC.Business.Interface;

namespace KMHC.SLTC.WebAPI.NCIA
{
     [RoutePrefix("api/AuditYearCert")]
    public class AuditYearCertController : BaseController
    {
         IAuditYearCertService service = IOCContainer.Instance.Resolve<IAuditYearCertService>();
        IMonFeeService monfeeService = IOCContainer.Instance.Resolve<IMonFeeService>();

        [Route(""), HttpGet]
         public IHttpActionResult Get(string name, string idno, string nsId, string status, int CurrentPage, int PageSize)
         {
             var request = new BaseRequest<AuditYearCertFilter>()
             {
                 CurrentPage = CurrentPage,
                 PageSize = PageSize,
                 Data = new AuditYearCertFilter()
                 {
                     Name = name,
                     IdNo = idno,
                     NsID = nsId,
                     Status = status,
                 }
             };

             var response = service.GetAduitYearCertList(request);
             return Ok(response);
         }

         [Route("")]
         public async Task<object>  Post(AuditYearCertModel request)
         {
            request.NsNo = monfeeService.QueryOrgNsHomeByID(request.NsID.ToString()).NSNo;
            var http = HttpClientHelper.NciHttpClient;
             string resultContent = "";
             try
             {
                 var result = await http.PostAsJsonAsync("/api/AuditYearCert", request);
                 resultContent = await result.Content.ReadAsStringAsync();
                 var response = JsonConvert.DeserializeObject<BaseResponse>(resultContent);
                 if (response.ResultCode == 1001)
                 {
                   var res = service.UpdateYearCert(request);
                   if (res.ResultCode == 0)
                   {
                       resultContent = "0";
                   }
                 }
             }
             catch (Exception ex)
             {
                 resultContent = "1";
             }
             return resultContent;
         }

         [Route("ltccert")]
         public IHttpActionResult Post(RegNCIInfo baseRequest)
         {
             var response = service.UpdateAppHospCertInfo(baseRequest);
             return Ok(response);
         }

    }
}

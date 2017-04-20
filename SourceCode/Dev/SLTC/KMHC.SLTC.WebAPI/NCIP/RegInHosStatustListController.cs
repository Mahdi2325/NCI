using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.NCI;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Interface.NCIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;

namespace KMHC.SLTC.WebAPI.NCIP
{
     [RoutePrefix("api/RegInHosStatusRes")]
    public class RegInHosStatustListController : BaseController 
    {
         IRegInHosStatusListService service = IOCContainer.Instance.Resolve<IRegInHosStatusListService>();
        [Route(""), HttpGet]
         public async Task<IHttpActionResult> Get(string name, string idno, string nsId, string status, int CurrentPage, int PageSize)
        {
            var result = await HttpClientHelper.NciHttpClient.GetAsync("/api/leaveHosp/QueryRegInHosStatusList" );
            var resultContent = await result.Content.ReadAsAsync<BaseResponse<IList<RegInHosStatusListEntity>>>();
            var response = new BaseResponse<RegInHosStatusDtlData>();
            response = service.QueryRegInHosStatustList(name, idno, nsId, status, resultContent, CurrentPage, PageSize);
            return Ok(response);
        }
    }
}

using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.NCIA
{
    [RoutePrefix("api/monFee")]
    public class MonFeeController : BaseController
    {
        IMonFeeService service = IOCContainer.Instance.Resolve<IMonFeeService>();
        [Route(""), HttpGet]
        public IHttpActionResult Get(string startTime, string endTime, int orgID, int CurrentPage, int PageSize)
        {
            var response = new BaseResponse<IList<MonFeeModel>>();
            var request = new BaseRequest<MonFeeFilter>()
            {
                CurrentPage = CurrentPage,
                PageSize = PageSize,
                Data = new MonFeeFilter()
                {
                    StartTime = startTime,
                    EndTime = endTime,
                    OrgID = orgID.ToString(),
                }
            };
            response = service.QueryMonFeeModel(request);
            return Ok(response);
        }

         [Route(""), HttpGet]
        public IHttpActionResult Get(string monFeeID)
        {
            var response = service.QueryMonFeeDetial(monFeeID);
            return Ok(response);
        }

        [Route(""), HttpGet]
         public IHttpActionResult Get(int monFeeid,string type)
         {
             var response = service.QueryMonFeeInfo(monFeeid);
             return Ok(response);
         }

         [Route(""), HttpPost]
         public IHttpActionResult Post(MonFeeGrantRequestEntity request)
         {
             var response = new BaseResponse<MonFeeModel>();
             response = service.SaveMonFee(request);
             return Ok(response);
         }
        [Route("GetNsMonfee"), HttpPost]
        public IHttpActionResult GetNsMonfee(BillV2 request)
        {
            var response = service.QueryYearMonthMonFeeInfo(request);
            return Ok(response);
        }
    }
}

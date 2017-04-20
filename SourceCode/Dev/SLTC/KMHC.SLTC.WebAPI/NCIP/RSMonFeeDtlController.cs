using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.NCIA
{
    [RoutePrefix("api/rsMonFeeDtl")]
    public class RSMonFeeDtlController : BaseController
    {
        IRSMonFeeDtlService service = IOCContainer.Instance.Resolve<IRSMonFeeDtlService>();
        [Route(""), HttpGet]
        public IHttpActionResult Get(int CurrentPage, int PageSize, int orgMonFeeId)
        {
            var response = new BaseResponse<IList<ResidentMonFeeModel>>();
            var request = new BaseRequest<MonFeeFilter>()
            {
                CurrentPage = CurrentPage,
                PageSize = PageSize,
                Data = new MonFeeFilter()
                {
                    OrgMonFeeId = orgMonFeeId,
                }
            };
            response = service.QueryRSMonFee(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Get(int CurrentPage, int PageSize, int orgMonFeeId, string feeType)
        {
            var response = new BaseResponse<IList<ResidentMonFeeDtlModel>>();
            var request = new BaseRequest<MonFeeFilter>()
            {
                CurrentPage = CurrentPage,
                PageSize = PageSize,
                Data = new MonFeeFilter()
                {
                    OrgMonFeeId = orgMonFeeId,
                    FeeType = feeType,
                }
            };
            response = service.QueryRSMonFeeDtl(request);
            return Ok(response);
        }
    }
}

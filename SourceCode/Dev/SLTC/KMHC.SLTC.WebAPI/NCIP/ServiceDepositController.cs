using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.NCI;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface.NCIP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.NCIP
{
     [RoutePrefix("api/ServiceDeposit")]
    public class ServiceDepositController : BaseController
    {
         IServiceDepositService service = IOCContainer.Instance.Resolve<IServiceDepositService>();
        [Route(""), HttpGet]
        public IHttpActionResult Get(string yearMonth,string nsId, int CurrentPage, int PageSize)
        {
            var response = new BaseResponse<IList<ServiceDeposit>>();
            var request = new BaseRequest<ServiceDepositFilter>()
            {
                CurrentPage = CurrentPage,
                PageSize = PageSize,
                Data = new ServiceDepositFilter()
                {
                    YearMonth = yearMonth,
                    NSID = nsId
                }
            };
            response = service.QueryServiceDepositList(request);
            return Ok(response);
        }
    }
}

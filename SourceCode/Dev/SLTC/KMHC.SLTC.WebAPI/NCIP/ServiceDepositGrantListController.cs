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
     [RoutePrefix("api/ServiceDepositGrantList")]
    public class ServiceDepositGrantListController : BaseController
    {
         IServiceDepositGrantListService service = IOCContainer.Instance.Resolve<IServiceDepositGrantListService>();
        [Route(""), HttpGet]
        public IHttpActionResult Get(string nsId, int CurrentPage, int PageSize)
        {
            var response = new BaseResponse<IList<ServiceDepositGrant>>();
            var request = new BaseRequest<ServiceDepositGrantFilter>()
            {
                CurrentPage = CurrentPage,
                PageSize = PageSize,
                Data = new ServiceDepositGrantFilter()
                {
                    NSID = nsId
                }
            };
            response = service.QueryServiceDepositGrantList(request);
            return Ok(response);
        }

        [Route("GetServiceDepositGrantByNsNo"), HttpGet]
        public IHttpActionResult GetServiceDepositGrantByNsNo(string nsNo, int CurrentPage, int PageSize)
        {
            var response = service.GetServiceDepositGrantByNsNo(nsNo, CurrentPage, PageSize);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(ServiceDepositGrant request)
        {
            var response = new BaseResponse<ServiceDepositGrant>();
            response = service.DeleteServiceDepositGrant(request);
            return Ok(response);
        }
    }
}

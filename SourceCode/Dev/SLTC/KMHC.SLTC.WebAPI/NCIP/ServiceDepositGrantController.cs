using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
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
    [RoutePrefix("api/ServiceDepositGrant")]
    public class ServiceDepositGrantController : BaseController
    {

        IServiceDepositGrantService service = IOCContainer.Instance.Resolve<IServiceDepositGrantService>();
        

        [Route("")]
        public IHttpActionResult Post(ServiceDepositGrant request)
        {
            var response = new BaseResponse<ServiceDepositGrant>();
            response = service.SaveServiceDepositGrant(request);
            return Ok(response);
        }
    }
}

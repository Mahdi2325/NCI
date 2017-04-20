using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/payGrant")]
    public class PayGrantController : BaseController
    {
        IPayGrantService service = IOCContainer.Instance.Resolve<IPayGrantService>();

        [Route(""), HttpGet]
        public IHttpActionResult Get(string year, string nsid)
        {
            var response = new BaseResponse<List<NCIPayGrantEntity>>();
            response = service.QueryPayGrantList(year, nsid);
            return Ok(response);
        }
         [Route(""), HttpGet]
        public IHttpActionResult Get(string year, string nsno, int type)
        {
            var response = new BaseResponse<List<NCIPayGrantEntity>>();
            response = service.QueryLTCPayGrant(year, nsno);
            return Ok(response);
        }

        [Route(""), HttpPost]
        public IHttpActionResult Post(NCIPayGrantEntity request)
        {
            var response = new BaseResponse<NCIPayGrantEntity>();
            response = service.SavePayGrant(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Get(int grantId, int type)
        {
            var response = service.QueryPayGrantinfo(Convert.ToInt32(grantId));
            return Ok(response);
        }

    }
}

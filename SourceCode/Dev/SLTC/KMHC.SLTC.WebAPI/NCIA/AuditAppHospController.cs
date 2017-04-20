using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter.NCIA;
using KMHC.SLTC.Business.Entity.NCIA;
using KMHC.SLTC.Business.Interface.NCIA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.NCIA
{
    [RoutePrefix("api/auditAppHosp")]
    public class AuditAppHospController : BaseController
    {

        IAuditAppHospService service = IOCContainer.Instance.Resolve<IAuditAppHospService>();
        [Route(""), HttpGet]
        public IHttpActionResult Get(string name, string idno, string nsId, int status, int CurrentPage, int PageSize)
        {
            var response = new BaseResponse<IList<AppHospEntity>>();
            var request = new BaseRequest<AuditAppHospEntityFilter>()
            {
                CurrentPage = CurrentPage,
                PageSize = PageSize,
                Data = new AuditAppHospEntityFilter()
                {
                    Name = name,
                    IdNo = idno,
                    Nsid = nsId,
                    Status = status,
                }
            };
            response = service.QueryAppHospList(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(AppHospEntity request)
        {
            var response = new BaseResponse<AppHospEntity>();
            response = service.AuditAppHosp(request);
            return Ok(response);
        }
    }
}

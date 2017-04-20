using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter.NCIA;
using KMHC.SLTC.Business.Entity.NCI;
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
     [RoutePrefix("api/AuditAppcert")]
    public class AuditAppcertController : BaseController
    {
         IAuditAppcertService service = IOCContainer.Instance.Resolve<IAuditAppcertService>();
        [Route(""), HttpGet]
        public IHttpActionResult Get(string name, string idno, string nsId, int status, int CurrentPage, int PageSize)
        {
            var response = new BaseResponse<IList<AppcertEntity>>();
            var request = new BaseRequest<AuditAppcretEntityFilter>()
            {
                CurrentPage = CurrentPage,
                PageSize = PageSize,
                Data = new AuditAppcretEntityFilter()
                {
                    Name = name,
                    IdNo = idno,
                    Nsid = nsId,
                    Status = status,
                }
            };
            response = service.QueryAppcertList(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Get(string org)
        {
            var response = new BaseResponse<IList<NursingHome>>();
            var request = new BaseRequest<AgencyORGFilter>()
            {
                CurrentPage = 1,
                PageSize = 1000,
                Data = new AgencyORGFilter()
                {
                    Govid = SecurityHelper.CurrentPrincipal.OrgId  //  todo  改成GovID
                }
            };
            response = service.QueryOrgNsHome(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(string code)
        {
            var response = service.GetQue(code);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Get(int appcertId)
        {
            var response = new BaseResponse<AppcertEntity>();
            response = service.QueryAppcert(appcertId);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult get(int appcertId, string mark)
        {
            var request = new BaseRequest<AgencyAsstRecordDataFilter>()
            {
                Data = new AgencyAsstRecordDataFilter
                {
                    AppcertId = appcertId,
                }
            };

            var response = service.GetAdlRec(request);
            return Ok(response);
        }
        [Route(""), HttpGet]
        public IHttpActionResult Get(string nsId, string mark)
        {
            var response = service.QueryCareTypeList(nsId);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(AppcertEntity request)
        {
            var response = new BaseResponse<AppcertEntity>();
            if (request.ActionStatus == 0)
            {

            
            }
            else if (request.ActionStatus == 6)
            {
                request.AgencyResult = 1;
                request.Status = 6;
            }
            else if (request.ActionStatus == 9)
            {
                request.AgencyResult = 0;
                request.Status = 9;
            }
            response = service.SaveAppcert(request);
            return Ok(response);
        }

        [Route("saveADL")]
        public IHttpActionResult Post(AgencyAsstRecordData request)
        {
            var response = service.SaveAdlRec(request);
            return Ok(response);
        }
    }
}

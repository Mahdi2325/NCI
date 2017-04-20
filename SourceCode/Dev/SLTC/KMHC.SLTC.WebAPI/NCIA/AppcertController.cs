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
    [RoutePrefix("api/Appcert")]
    public class AppcertController : BaseController
    {
        IAppcertService service = IOCContainer.Instance.Resolve<IAppcertService>();
        [Route(""), HttpGet]
        public IHttpActionResult Get(string status,string name, string idno, int CurrentPage, int PageSize)
        {
            var response = new BaseResponse<IList<AppcertEntity>>();
            var iStatus=-1;
            if(!string.IsNullOrEmpty(status))
            {
                iStatus = Convert.ToInt32(status);
            }
            var request = new BaseRequest<AppcertEntityFilter>()
            {
                CurrentPage = CurrentPage,
                PageSize = PageSize,
                Data = new AppcertEntityFilter()
                {
                    Name = name,
                    IdNo = idno,
                    Status=iStatus
                }
            };
            response = service.QueryAppcertList(request);
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
        public IHttpActionResult Get(string keyNo)
        {
            var response = new BaseResponse<ApplicantEntity>();
            response = service.QueryApplicant(keyNo);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Get(string idNo, string type)
        {
            var response = service.QueryLTCAppcertInfo(idNo, type);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult get(int appcertId, string mark)
        {
            var request = new BaseRequest<NursingHomeAsstRecordDataFilter>()
            {
                Data = new NursingHomeAsstRecordDataFilter
                {
                    AppcertId = appcertId,
                }
            };

            var response = service.GetAdlRec(request);
            return Ok(response);
        }
        [Route(""), HttpGet]
        public IHttpActionResult Query(string code)
        {
            var response = service.GetQue(code);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query()
        {
            var response = service.QueryCareTypeList();
            return Ok(response);
        }
        [Route("")]
        public IHttpActionResult Post(AppcertEntity request)
        {
            var response = new BaseResponse<AppcertEntity>();
            if (request.ActionStatus == 0)
            {
                response = service.SaveAppcert(request);
            }
            else if (request.ActionStatus == 1)
            {
                response = service.CancelSubmitAppcert(request);
            }
            else if (request.ActionStatus == 3)
            {
                response = service.SubmitAppcert(request);
            }
            else if (request.ActionStatus ==11)
            {
                response = service.SaveReAppcert(request);
            }
            else if (request.ActionStatus == -1)
            {
                response = service.DeleteAppcert(request);
            }
            return Ok(response);
        }

        [Route("saveADL")]
        public IHttpActionResult Post(NursingHomeAsstRecordData request)
        {
            var response = service.SaveAdlRec(request);
            return Ok(response);
        }


    }
}

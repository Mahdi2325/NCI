using System;
using System.Collections.Generic;
using System.Web.Http;
using KM.Common;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.NCIA;
using KMHC.SLTC.Business.Interface.NCIA;
using KMHC.Infrastructure;

namespace KMHC.SLTC.WebAPI.NCIA
{
    [RoutePrefix("api/Applicant")]
    public class ApplicantController : BaseController
    {
        IApplicantService service = IOCContainer.Instance.Resolve<IApplicantService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(string name = "", string idno = "", string nsId = "", int currentPage = 1, int pageSize = 10)
        {
            BaseResponse<IList<ApplicantEntity>> response = new BaseResponse<IList<ApplicantEntity>>();

            try
            {
                var filetr = new ApplicantFiletr();
                filetr.Name = name;
                filetr.IdNo = idno;
                filetr.NsId = nsId;
                BaseRequest<ApplicantFiletr> request = new BaseRequest<ApplicantFiletr>();
                request.Data = filetr;
                request.CurrentPage = currentPage;
                request.PageSize = pageSize;

                response = service.QueryApplicantList(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ResultMessage = ex.Message;
                throw;
            }
        }

        [Route("")]
        public IHttpActionResult Get(string no)
        {
            BaseResponse<ApplicantEntity> response = new BaseResponse<ApplicantEntity>();
            try
            {
                response = service.QueryApplicantInfo(no);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ResultMessage = ex.Message;
                throw;
            }
        }

        [Route("")]
        public IHttpActionResult Post([FromBody] ApplicantEntity baseRequest)
        {
            baseRequest.Updateby = SecurityHelper.CurrentPrincipal.UserId.ToString();
            baseRequest.Updatetime = DateTime.Now;
            var response = service.SaveApplicant(baseRequest);
            return Ok(response);
        }

        [Route("dtlSave")]
        public IHttpActionResult Post([FromBody] SaveApplicantEntity baseRequest)
        {
            var response = new BaseResponse<ApplicantEntity>();
            try
            {
                if (string.IsNullOrEmpty(baseRequest.ApplicantId))
                {
                    baseRequest.Createby = SecurityHelper.CurrentPrincipal.UserId.ToString();
                    baseRequest.Createtime = DateTime.Now;
                    baseRequest.NSID = SecurityHelper.CurrentPrincipal.OrgId;
                    baseRequest.Lastcertresult = 0;
                    baseRequest.Lasthospresult = 0;
                    baseRequest.Isdelete = false;
                }
                else
                {
                    baseRequest.Updateby = SecurityHelper.CurrentPrincipal.UserId.ToString();
                    baseRequest.Updatetime = DateTime.Now;
                    baseRequest.Isdelete = false;
                }

                response = service.SaveApplicantInfo(baseRequest);
            }
            catch (Exception exception)
            {
                response.ResultMessage = exception.Message;
                throw;
            }
            return Ok(response);
        }
    }
}

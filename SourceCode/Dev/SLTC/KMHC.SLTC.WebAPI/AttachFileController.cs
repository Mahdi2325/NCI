using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/AttachFile")]
    public class AttachFileController : BaseController
    {
        IResidentManageService service = IOCContainer.Instance.Resolve<IResidentManageService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int feeNo, int currentPage, int pageSize)
        {
            BaseRequest<AttachFileFilter> request = new BaseRequest<AttachFileFilter>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            request.Data.FeeNo = feeNo;
            var response = service.QueryAttachFile(request);
            return Ok(response);
        }

        [Route("{feeNo}")]
        public IHttpActionResult Get(int feeNo)
        {
            var response = service.GetAttachFile(feeNo);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Post(AttachFile baseRequest)
        {
            var response = service.SaveAttachFile(baseRequest);
            return Ok(response);
        }

        [Route("{feeNo}")]
        public IHttpActionResult Delete(int feeNo)
        {
            var response = service.DeleteAttachFile(feeNo);
            return Ok(response);
        }
    }
}